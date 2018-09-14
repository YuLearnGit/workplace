using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
namespace FireWall
{
    /// <summary>
    /// CNCConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class CNCConfigurationApply : Window
    {
        Thread CNCApplyThread;
        public CNCConfigurationApply()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CNCdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list();
            CNCApplyEnabled();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }

        private void CNCeditclick(object sender, MouseButtonEventArgs e)
        {
            CNCConfigurationDetail fw = new CNCConfigurationDetail();
            int connlimit = (CNCdataGrid.SelectedItem as CNCRuleDataTable).connlimit;
            string[] srcIP = (CNCdataGrid.SelectedItem as CNCRuleDataTable).srcIP.ToString().Split('.');
            string[] dstIP = (CNCdataGrid.SelectedItem as CNCRuleDataTable).dstIP.ToString().Split('.');
            string sport = (CNCdataGrid.SelectedItem as CNCRuleDataTable).sport;
            string dport = (CNCdataGrid.SelectedItem as CNCRuleDataTable).dport;
            bool log = (CNCdataGrid.SelectedItem as CNCRuleDataTable).log;
            if (srcIP[0] != "")
            {
                fw.srcStarttextBox_1.Text = srcIP[0]; fw.srcStarttextBox_2.Text = srcIP[1];
                fw.srcStarttextBox_3.Text = srcIP[2]; fw.srcStarttextBox_4.Text = srcIP[3];
            }
            if (dstIP[0] != "")
            {
                fw.dstStarttextBox_1.Text = dstIP[0]; fw.dstStarttextBox_2.Text = dstIP[1];
                fw.dstStarttextBox_3.Text = dstIP[2]; fw.dstStarttextBox_4.Text = dstIP[3];
            }
            fw.sportBox.Text = sport;  fw.dportBox.Text = dport;
            fw.logcheckBox.IsChecked = log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().IndexOf(CNCdataGrid.SelectedItem as CNCRuleDataTable);
            StaticGlobal.editflag = true;
            this.Close();
            fw.ShowDialog();
        }

        private void CNCdeleteclick(object sender, MouseButtonEventArgs e)
        {
            ICNCRulesManage fw = new CNCRulesManage();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            int connlimit = (CNCdataGrid.SelectedItem as CNCRuleDataTable).connlimit;
            string srcIP = (CNCdataGrid.SelectedItem as CNCRuleDataTable).srcIP;
            string dstIP = (CNCdataGrid.SelectedItem as CNCRuleDataTable).dstIP;
            string sport = (CNCdataGrid.SelectedItem as CNCRuleDataTable).sport;
            string dport = (CNCdataGrid.SelectedItem as CNCRuleDataTable).dport;
            bool log = (CNCdataGrid.SelectedItem as CNCRuleDataTable).log;
            if(fw.DelCNCRules(dev_ip,log,connlimit,srcIP,dstIP,sport,dport))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Remove((CNCdataGrid.SelectedItem as CNCRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }
            else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
        }
        public void CNCApplyEnabled()
        {
            if (StaticGlobal.CNColdrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Count())
            {
                int total = 0;
                for (int i = 0; i < StaticGlobal.PRToldrules.Count; i++)
                {
                    int connlimit = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].connlimit;
                    string srcIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].srcIP;
                    string dstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].dstIP;
                    string sport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].sport;
                    string dport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].dport;
                    bool log = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list()[i].log;
                    int num = (from rules in StaticGlobal.CNColdrules
                               where rules.connlimit == connlimit && rules.srcIP == srcIP && rules.dstIP == dstIP && rules.sport == sport && rules.dport == dport && rules.log == log
                               select rules.srcIP).ToList<string>().Count;
                    total += num;
                }
                if (total == StaticGlobal.CNColdrules.Count)
                    CNCApplybutton.Visibility = Visibility.Collapsed;
                else CNCApplybutton.Visibility = Visibility.Visible;
            }
            else CNCApplybutton.Visibility = Visibility.Visible;   
        }
        private void CNCApplybutton_Click(object sender, RoutedEventArgs e)
        {
            CNCApplyThread = new Thread(new ThreadStart(CNCApplying));
            CNCApplyThread.IsBackground = true;
            CNCApplyThread.Start();
        }
        public void CNCApplying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewCNCRulebutton.IsEnabled = false;
                CNCApplybutton.IsEnabled = false;
                CNCdataGrid.IsEnabled = false;
                Closebutton.IsEnabled = false;
            }));
            //找出需要删除的规则
            var deleteRules = StaticGlobal.CNColdrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var addRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Where(addRule => !StaticGlobal.CNColdrules.Contains(addRule)).ToList();
            bool CNCApplyFlag = true;
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            ICNCRulesManage cncrule = new CNCRulesManage();
            //添加规则
            for (int i = 0; i < addRules.Count(); i++)
            {
                int connlimit = addRules[i].connlimit;
                string srcIP = addRules[i].srcIP;
                string dstIP = addRules[i].dstIP;
                string sport = addRules[i].sport;
                string dport = addRules[i].dport;
                bool log = addRules[i].log;
                if (cncrule.AddCNCRules(dev_ip, log, connlimit, srcIP, dstIP, sport, dport))
                    CNCApplyFlag = true;
                else CNCApplyFlag = false;
            }
            //删除规则
            for (int j = 0; j < deleteRules.Count(); j++)
            {
                int connlimit = deleteRules[j].connlimit;
                string srcIP = deleteRules[j].srcIP;
                string dstIP = deleteRules[j].dstIP;
                string sport = deleteRules[j].sport;
                string dport = deleteRules[j].dport;
                bool log = deleteRules[j].log;
                if(cncrule.DelCNCRules(dev_ip,log,connlimit,srcIP,dstIP,sport,dport))
                    CNCApplyFlag = true;
                else CNCApplyFlag = false;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (CNCApplyFlag)
                {
                    UserMessageBox.Show("提示", "所有规则已成功应用！");
                }
                else
                {
                    UserMessageBox.Show("提示", "部分规则未成功应用，请检查设备之间的连接！");
                }
                this.Close();
            }
           ));
            CNCApplyThread.Abort();
        }
        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewCNCRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            CNCConfigurationDetail fw = new CNCConfigurationDetail();
            fw.ShowDialog();
        }
    }
}
