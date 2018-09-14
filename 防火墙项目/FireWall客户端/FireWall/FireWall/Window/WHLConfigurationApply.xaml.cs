using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Input;
using System.Threading;
namespace FireWall
{
    /// <summary>
    /// WHLConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class WHLConfigurationApply : Window
    {
        Thread WHLApplyThread;
        public WHLConfigurationApply()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WHLdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list();
            WHLApplyEnabled();
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

        public void WHLApplyEnabled()
        {
            if (StaticGlobal.WHLoldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Count())
            {
                int total = 0;
                for (int i = 0; i < StaticGlobal.WHLoldrules.Count(); i++)
                {
                    string dst_IP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[i].dst_IP;
                    string src_IP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[i].src_IP;
                    string dst_port = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[i].dst_port;
                    string src_port = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[i].src_port;
                    bool log = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[i].log;
                    int num = (from WHLrules in StaticGlobal.WHLoldrules
                               where  WHLrules.dst_IP == dst_IP && WHLrules.src_IP == src_IP
                               && WHLrules.dst_port == dst_port && WHLrules.src_port == src_port
                               select WHLrules.dst_IP).ToList<string>().Count;
                    total += num;
                }
                if (total == StaticGlobal.WHLoldrules.Count)
                    WHLApplybutton.Visibility = Visibility.Collapsed;
                else
                    WHLApplybutton.Visibility = Visibility.Visible;
            }
            else WHLApplybutton.Visibility = Visibility.Visible;
        }

        private void WHLApplybutton_Click(object sender, RoutedEventArgs e)
        {
            WHLApplyThread = new Thread(new ThreadStart(WHLApplying));
            WHLApplyThread.IsBackground = true;
            WHLApplyThread.Start();
        }
        public void WHLApplying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewWHLRulebutton.IsEnabled = false;
                WHLApplybutton.IsEnabled = false;
                WHLdataGrid.IsEnabled = false;
                Closebutton.IsEnabled = false;
            }));
            IConfigWhiteLists WHLrule = new ConfigWhiteLists();
            bool WHLApplyflag = true;       
            //找出需要删除的规则
            var deleteRules = StaticGlobal.WHLoldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var addRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Where(addRule => !StaticGlobal.WHLoldrules.Contains(addRule)).ToList();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            //增加规则
            for (int i = 0; i < addRules.Count(); i++)
            {
                string dst_IP = addRules[i].dst_IP;
                string dst_port = addRules[i].dst_port;
                string src_IP = addRules[i].src_IP;
                string src_port = addRules[i].src_port;
                bool log = addRules[i].log;
                if (WHLrule.ChangeWhiteLists(dev_ip, dst_IP, src_IP, dst_port, src_port, log, true))
                    WHLApplyflag = true;
                else WHLApplyflag = false;
            }
            //删除规则
            for (int j = 0; j < deleteRules.Count(); j++)
            {
                string dst_IP = deleteRules[j].dst_IP;
                string dst_port = deleteRules[j].dst_port;
                string src_IP = deleteRules[j].src_IP;
                string src_port = deleteRules[j].src_port;
                bool log = deleteRules[j].log;
                if (WHLrule.ChangeWhiteLists(dev_ip, dst_IP, src_IP, dst_port, src_port, log, false))
                    WHLApplyflag = true;
                else WHLApplyflag = false;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (WHLApplyflag)
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
            WHLApplyThread.Abort();
        }
        private void WHLeditclick(object sender, MouseButtonEventArgs e)
        {
            WHLConfigurationDetail fw = new WHLConfigurationDetail();
            string[]dst_ip= (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_IP.ToString().Split('.');
            string[] src_IP= (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_IP.ToString().Split('.');
            string dst_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_port.ToString();
            string src_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_port.ToString();      
            fw.srcStarttextBox_1.Text = src_IP[0]; fw.srcStarttextBox_2.Text = src_IP[1];
            fw.srcStarttextBox_3.Text = src_IP[2]; fw.srcStarttextBox_4.Text = src_IP[3];
            fw.dstStarttextBox_1.Text = dst_ip[0]; fw.dstStarttextBox_2.Text = dst_ip[1];
            fw.dstStarttextBox_3.Text = dst_ip[2]; fw.dstStarttextBox_4.Text = dst_ip[3];
            fw.sportBox.Text = src_port;fw.dportBox.Text = dst_port;
            fw.logcheckBox.IsChecked= (WHLdataGrid.SelectedItem as WHLRuleDataTable).log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().IndexOf(WHLdataGrid.SelectedItem as WHLRuleDataTable);
            StaticGlobal.editflag = true;
            this.Close();
            fw.ShowDialog();
        }

        private void WHLdeleteclick(object sender, MouseButtonEventArgs e)
        {
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string dst_ip = (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_IP.ToString();
            string src_IP = (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_IP.ToString();
            string dst_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_port.ToString();
            string src_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_port.ToString();
            bool log = (WHLdataGrid.SelectedItem as WHLRuleDataTable).log;
            IConfigWhiteLists fw = new ConfigWhiteLists();
            if (fw.ChangeWhiteLists(dev_ip, dst_ip, src_IP, dst_port, src_port, log, false))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Remove((WHLdataGrid.SelectedItem as WHLRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }            
        }
        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewWHLRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            WHLConfigurationDetail fwcd = new WHLConfigurationDetail();
            fwcd.ShowDialog();
        }
    }
}
