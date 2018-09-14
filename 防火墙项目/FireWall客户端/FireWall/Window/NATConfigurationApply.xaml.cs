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
    /// NATConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class NATConfigurationApply 
    {
        DBOperation db_operate = new DBOperation();
        Thread NATApplyThread;
        public NATConfigurationApply()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SNATdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list();
            DNATdataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list();
            NATComboBoxBinding();
      
            NATApplyEnabled();
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
        public void NATComboBoxBinding()
        {
            string[] nat_type = { "DNAT", "SNAT" };
            NATComboBox.ItemsSource = nat_type;
        }

        public void NATApplyEnabled()
        {
            if (StaticGlobal.SNAToldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Count()
                && StaticGlobal.DNAToldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Count())
            {
                int stotal = 0; int dtotal = 0;
                for (int i = 0; i < StaticGlobal.SNAToldrules.Count(); i++)
                {
                    string origin_devIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[i].origin_devIP;
                    string EthName = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[i].EthName;
                    string NATIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[i].NATIP;
                    string EthIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[i].EthIP;
                    int snum = (from SNATrules in StaticGlobal.SNAToldrules
                                where SNATrules.origin_devIP == origin_devIP && SNATrules.EthName == EthName && SNATrules.NATIP == NATIP && SNATrules.EthIP == EthIP
                                select SNATrules.EthName).ToList<string>().Count;
                    stotal += snum;
                }
                for (int j = 0; j < StaticGlobal.DNAToldrules.Count(); j++)
                {
                    string origin_dstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[j].origin_dstIP;
                    string origin_dport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[j].origin_dport;
                    string map_IP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[j].map_IP;
                    string map_port = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[j].map_port;
                    int dnum = (from DNATrules in StaticGlobal.DNAToldrules
                                where DNATrules.origin_dstIP == origin_dstIP && DNATrules.origin_dport == origin_dport && DNATrules.map_IP == map_IP && DNATrules.map_port == map_port
                                select DNATrules.origin_dstIP).ToList<string>().Count;
                    dtotal += dnum;
                }
                if (stotal == StaticGlobal.SNAToldrules.Count() && dtotal == StaticGlobal.DNAToldrules.Count())
                    NATApplybutton.Visibility = Visibility.Collapsed;
                else NATApplybutton.Visibility = Visibility.Visible;
            }
            else NATApplybutton.Visibility = Visibility.Visible;
        }

        private void NATApplybutton_Click(object sender, RoutedEventArgs e)
        {
            NATApplyThread = new Thread(new ThreadStart(NATApplying));
            NATApplyThread.IsBackground = true;
            NATApplyThread.Start();
        }
        private void NATApplying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewNATRulebutton.IsEnabled = false;
                NewNATRulebutton.IsEnabled = false;
                SNATdataGrid.IsEnabled = false;
                DNATdataGrid.IsEnabled = false;
            }));
            //找出需要删除的规则
            var SNATdeleteRules = StaticGlobal.SNAToldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Contains(deleteRule)).ToList();
            var DNATdeleteRules = StaticGlobal.DNAToldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var SNATaddRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Where(addRule => !StaticGlobal.SNAToldrules.Contains(addRule)).ToList();
            var DNATaddRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Where(addRule => !StaticGlobal.DNAToldrules.Contains(addRule)).ToList();

            bool SNATapplyflag = true;  bool DNATapplyflag = true;
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            IConfigureNAT NATrules = new ConfigureNAT();

            /*添加NAT规则*/
            for (int i = 0; i < SNATaddRules.Count(); i++)
            {
                string origin_devIP = SNATaddRules[i].origin_devIP;
                string EthName = SNATaddRules[i].EthName;
                string EthIP = SNATaddRules[i].EthIP;
                string NATIP = SNATaddRules[i].NATIP;
                if (NATrules.ConfigSNAT(dev_ip, EthName, origin_devIP, EthIP, true))
                    SNATapplyflag = true;
                else
                    SNATapplyflag = false;
            }
            for (int i = 0; i < DNATaddRules.Count(); i++)
            {
                string origin_dstIP = DNATaddRules[i].origin_dstIP;
                string origin_dport = DNATaddRules[i].origin_dport;
                string map_IP = DNATaddRules[i].map_IP;
                string map_port = DNATaddRules[i].map_port;
                if (NATrules.ConfigDNAT(dev_ip, origin_dstIP, origin_dport, map_IP, map_port, true))
                    DNATapplyflag = true;
                else
                    DNATapplyflag = false;
            }         
            /*删除NAT规则*/
            for (int i = 0; i < SNATdeleteRules.Count(); i++)
            {
                string origin_devIP = SNATdeleteRules[i].origin_devIP;
                string EthName = SNATdeleteRules[i].EthName;
                string EthIP = SNATdeleteRules[i].EthIP;
                string NATIP = SNATdeleteRules[i].NATIP;
                if (NATrules.ConfigSNAT(dev_ip, EthName, origin_devIP, EthIP, false))
                    SNATapplyflag = true;
                else
                    SNATapplyflag = false;
            }
            for (int i = 0; i < DNATdeleteRules.Count(); i++)
            {
                string origin_dstIP = DNATdeleteRules[i].origin_dstIP;
                string origin_dport = DNATdeleteRules[i].origin_dport;
                string map_IP = DNATdeleteRules[i].map_IP;
                string map_port = DNATdeleteRules[i].map_port;
                if (NATrules.ConfigDNAT(dev_ip, origin_dstIP, origin_dport, map_IP, map_port, false))
                    DNATapplyflag = true;
                else
                    DNATapplyflag = false;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (SNATapplyflag && DNATapplyflag)
                    UserMessageBox.Show("提示", "所有规则已成功应用！");
                else
                {
                    if (SNATapplyflag)
                    {
                        UserMessageBox.Show("提示", "所有SNAT规则已成功应用！");
                    }
                    if (DNATapplyflag)
                    {
                        UserMessageBox.Show("提示", "所有DNAT规则已成功应用！");
                    }
                    else
                    {
                        UserMessageBox.Show("提示", "部分规则未成功应用，请检查设备之间的连接！");
                    }
                }
                this.Close();
            }
));
            NATApplyThread.Abort();
        }

        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NATeditclick(object sender, MouseButtonEventArgs e)
        {
            NATConfigurationDetail fwcd = new NATConfigurationDetail();
            if (NATComboBox.SelectedItem.ToString() == "DNAT")
            {
                fwcd.NATComboBox.Text = "DNAT";
                string[] origin_dstIP =(DNATdataGrid.SelectedItem as DNATRuleDataTable ).origin_dstIP.ToString().Split('.');
                string[] map_IP= (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_IP.ToString().Split('.');           
                fwcd.NATComboBox.Text = NATComboBox.Text.ToString();
                fwcd.srcStarttextBox_1.Text = origin_dstIP[0]; fwcd.srcStarttextBox_2.Text = origin_dstIP[1];
                fwcd.srcStarttextBox_3.Text = origin_dstIP[2]; fwcd.srcStarttextBox_4.Text = origin_dstIP[3];
                fwcd.dstStarttextBox_1.Text = map_IP[0]; fwcd.dstStarttextBox_2.Text = map_IP[1];
                fwcd.dstStarttextBox_3.Text = map_IP[2]; fwcd.dstStarttextBox_4.Text = map_IP[3];
                fwcd.orig_dportBox.Text = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dport.ToString();
                fwcd.nat_dportBox.Text= (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_port.ToString();
                StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().IndexOf(DNATdataGrid.SelectedItem as DNATRuleDataTable);
                StaticGlobal.editflag = true;
                this.Close();fwcd.ShowDialog();
            }
            if (NATComboBox.SelectedItem.ToString() == "SNAT")
            {
                fwcd.NATComboBox.Text = "SNAT";
                string[] origin_devIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).origin_devIP.ToString().Split('.');
                string[] EthIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthIP.ToString().Split('.');
                //string[] NATIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).NATIP.ToString().Split('.');
                fwcd.ETHComboBox.Text = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthName.ToString();
                fwcd.srcStarttextBox_1.Text = origin_devIP[0]; fwcd.srcStarttextBox_2.Text = origin_devIP[1];
                fwcd.srcStarttextBox_3.Text = origin_devIP[2]; fwcd.srcStarttextBox_4.Text = origin_devIP[3];
                fwcd.dstStarttextBox_1.Text = EthIP[0]; fwcd.dstStarttextBox_2.Text = EthIP[1];
                fwcd.dstStarttextBox_3.Text = EthIP[2]; fwcd.dstStarttextBox_4.Text = EthIP[3];
                StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().IndexOf(SNATdataGrid.SelectedItem as SNATRuleDataTable);
                StaticGlobal.editflag = true;
                this.Close(); fwcd.ShowDialog();
            }
        }

        private void NATdeleteclick(object sender, MouseButtonEventArgs e)
        {
            //if (NATComboBox.SelectedItem.ToString() == "DNAT")
            //{
            //    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Remove((DNATdataGrid.SelectedItem as DNATRuleDataTable));
            //    NATApplyEnabled();
            //}
            //if(NATComboBox.SelectedItem.ToString() == "SNAT")
            //{
            //    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Remove((SNATdataGrid.SelectedItem as SNATRuleDataTable));
            //    NATApplyEnabled();
            //}
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            IConfigureNAT fw = new ConfigureNAT();

            string origin_devIP = (DNATdataGrid.SelectedItem as SNATRuleDataTable).origin_devIP.ToString();
            string EthIP = (DNATdataGrid.SelectedItem as SNATRuleDataTable).EthIP.ToString();
            string origin_dport = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dport.ToString();
            string map_port = (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_port.ToString();
            if (fw.ConfigDNAT(dev_ip, origin_devIP, origin_dport, EthIP, map_port, false))
                UserMessageBox.Show("提示", "规则删除成功！");
        }

        private void AddNATRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            this.Close();
            NATConfigurationDetail fw = new NATConfigurationDetail();
            fw.ShowDialog();
        }

        private void NATComboBox_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (NATComboBox.SelectedItem.ToString() == "DNAT")
            {
                DNATdataGrid.Visibility = Visibility.Visible;
                SNATdataGrid.Visibility = Visibility.Collapsed;
            }
            if (NATComboBox.SelectedItem.ToString() == "SNAT")
            {
                DNATdataGrid.Visibility = Visibility.Collapsed;
                SNATdataGrid.Visibility = Visibility.Visible;
            }
        }
    }

}
