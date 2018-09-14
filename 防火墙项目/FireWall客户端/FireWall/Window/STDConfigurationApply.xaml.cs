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
    /// STDConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class STDConfigurationApply 
    {
        Thread STDApplyThread;
        public STDConfigurationApply()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            STDdataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list();
            STDApplyEnabled();
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

        private void NewSTDRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            STDConfigurationDetail fw = new STDConfigurationDetail();
            fw.ShowDialog();
        }

        private void STDApplybutton_Click(object sender, RoutedEventArgs e)
        {
            STDApplyThread = new Thread(new ThreadStart(STDApplying));
            STDApplyThread.IsBackground = true;
            STDApplyThread.Start();
        }

        public void STDApplyEnabled()
        {
            if (StaticGlobal.STDoldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Count())
            {
                int total = 0;
                for (int i = 0; i < StaticGlobal.PRToldrules.Count; i++)
                {
                    string protocol = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].protocol;
                    string srcIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].srcIP;
                    string dstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].dstIP;
                    string sport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].sport;
                    string dport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].dport;
                    bool log = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list()[i].log;
                    int num = (from rules in StaticGlobal.STDoldrules
                               where rules.protocol == protocol && rules.srcIP == srcIP && rules.dstIP == dstIP && rules.sport == sport && rules.dport == dport && rules.log == log
                               select rules.protocol).ToList<string>().Count;
                    total += num;
                }
                if (total == StaticGlobal.STDoldrules.Count)
                    STDApplybutton.Visibility = Visibility.Collapsed;
                else STDApplybutton.Visibility = Visibility.Visible;
            }
            else STDApplybutton.Visibility = Visibility.Visible;
        }
        public void STDApplying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewSTDRulebutton.IsEnabled = false;
                STDApplybutton.IsEnabled = false;
                STDdataGrid.IsEnabled = false;
                Closebutton.IsEnabled = false;
            }));
            //找出需要删除的规则
            var deleteRules = StaticGlobal.STDoldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var addRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Where(addRule => !StaticGlobal.STDoldrules.Contains(addRule)).ToList();
            bool STDApplyFlag = true;
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            ISTDRulesManage cncrule = new STDRulesManage();
            //添加规则
            for (int i = 0; i < addRules.Count(); i++)
            {
                string protocol = addRules[i].protocol;
                string srcIP = addRules[i].srcIP;
                string dstIP = addRules[i].dstIP;
                string sport = addRules[i].sport;
                string dport = addRules[i].dport;
                bool log = addRules[i].log;
                if (cncrule.AddSTDRules(dev_ip, log, protocol, srcIP, dstIP, sport, dport))
                    STDApplyFlag = true;
                else STDApplyFlag = false;
            }
            //删除规则
            for (int j = 0; j < deleteRules.Count(); j++)
            {
                string protocol = deleteRules[j].protocol;
                string srcIP = deleteRules[j].srcIP;
                string dstIP = deleteRules[j].dstIP;
                string sport = deleteRules[j].sport;
                string dport = deleteRules[j].dport;
                bool log = deleteRules[j].log;
                if (cncrule.DelSTDRules(dev_ip, log, protocol, srcIP, dstIP, sport, dport))
                    STDApplyFlag = true;
                else STDApplyFlag = false;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (STDApplyFlag)
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
            STDApplyThread.Abort();
        }
        private void STDeditclick(object sender, MouseButtonEventArgs e)
        {
            STDConfigurationDetail fw = new STDConfigurationDetail();
            string protocol= (STDdataGrid.SelectedItem as STDRuleDataTable).protocol;
            string[] srcIP = (STDdataGrid.SelectedItem as STDRuleDataTable).srcIP.ToString().Split('.');
            string[] dstIP = (STDdataGrid.SelectedItem as STDRuleDataTable).dstIP.ToString().Split('.');
            string sport = (STDdataGrid.SelectedItem as STDRuleDataTable).sport;
            string dport = (STDdataGrid.SelectedItem as STDRuleDataTable).dport;
            bool log = (STDdataGrid.SelectedItem as STDRuleDataTable).log;
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
            fw.sportBox.Text = sport; fw.dportBox.Text = dport;
            fw.logcheckBox.IsChecked = log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().IndexOf(STDdataGrid.SelectedItem as STDRuleDataTable);
            StaticGlobal.editflag = true;
            this.Close();
            fw.ShowDialog();
        }

        private void STDdeleteclick(object sender, MouseButtonEventArgs e)
        {
            ISTDRulesManage fw = new STDRulesManage();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string protocol = (STDdataGrid.SelectedItem as STDRuleDataTable).protocol;
            string srcIP = (STDdataGrid.SelectedItem as STDRuleDataTable).srcIP;
            string dstIP = (STDdataGrid.SelectedItem as STDRuleDataTable).dstIP;
            string sport = (STDdataGrid.SelectedItem as STDRuleDataTable).sport;
            string dport = (STDdataGrid.SelectedItem as STDRuleDataTable).dport;
            bool log = (STDdataGrid.SelectedItem as STDRuleDataTable).log;
            if (fw.DelSTDRules(dev_ip, log, protocol, srcIP, dstIP, sport, dport))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Remove((STDdataGrid.SelectedItem as STDRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }
            else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
        }

        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
