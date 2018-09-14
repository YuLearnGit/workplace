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
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;

namespace FireWall
{
    /// <summary>
    /// ConfigFW.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigFW : UserControl
    {
        DBOperation db = new DBOperation();
        public ConfigFW()
        {
            InitializeComponent();
        }

        private void window_loaded(object sender, RoutedEventArgs e)
        {
            PropertyBind();
            DNATdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list();
            SNATdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list();
            WHLdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list();
            dataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list();
            APCdataGrid.ItemsSource=StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getAPCRule_list();
            PRTdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list();
            CNCdataGrid.ItemsSource= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list();
            STDdataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list();
        }

        public void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaticGlobal.firewallmac != null)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0://属性
                        DevIPcomboxBinding();
                        break;
                    case 1://NAT
                        NATMAClabel.Content= "MAC: " + StaticGlobal.firewallmac;
                        NATIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        NATComboBoxBinding();
                        break;
                    case 2://白名单
                        MACWHLlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        WHLIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                    case 3://深度包检测DPI
                        MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        DPIIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                    case 4://应用层协议控制APC
                        MACAPClabel.Content= "MAC: " + StaticGlobal.firewallmac;
                        APCIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                    case 5://连接数控制CNC
                        MACCNClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        CNCIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                    case 6://策略路由PRT
                        MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                    case 7://状态检测STD
                        MACSTDlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        STDIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        break;
                }
            }
        }
        string[] pptdev_ip = new string[10];
        string[] pptdev_mac = new string[10];
        string[] pptdev_type = new string[10];

        //属性选项卡
        public void PropertyBind()
        {
            FWname.Text = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getproperty_list()[0].fwname;
            textBox.Text = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getproperty_list()[0].description;
            FWMAC.Text = StaticGlobal.firewallmac;
            FWIP.Text = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
        }
        private void DevIP_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            int i = Array.IndexOf(pptdev_ip, DevIPcombox.SelectedItem);
            DevMAC.Text = pptdev_mac[i];
            DevType.Text = pptdev_type[i];
        }
        public void DevIPcomboxBinding()
        {
            if (StaticGlobal.fwdev_list[0].getProtecDev_list().Count() != 0)
            {
                for (int i = 0; i < StaticGlobal.fwdev_list.Count(); i++)
                {

                    if (StaticGlobal.fwdev_list[i].getDev_MAC() == StaticGlobal.firewallmac)
                    {
                        for (int j = 0; j < StaticGlobal.fwdev_list[i].getProtecDev_list().Count(); j++)
                        {
                            pptdev_ip[j] = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_IP();
                            pptdev_mac[j] = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_MAC();
                            pptdev_type[j] = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_type();
                        }
                    }
                }
                DevIPcombox.ItemsSource = pptdev_ip;
                DevMAC.Text = pptdev_mac[0].ToString();
                DevType.Text = pptdev_type[0].ToString();
            }
        }
        private void NOIP_Click(object sender, RoutedEventArgs e)
        {
            INoIPConfig conf = new NoIPConfig();
            for (int i = 0; i < StaticGlobal.fwdev_list.Count(); i++)
            {

                if (StaticGlobal.fwdev_list[i].getDev_MAC() == StaticGlobal.firewallmac)
                {
                    if (conf.NoipConfig(StaticGlobal.fwdev_list[i]))
                    {
                        UserMessageBox.Show("提示", "无IP模式配置成功！");
                        StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] = "0.0.0.0";
                        FWIP.Text = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                    }
                    else
                    {
                        UserMessageBox.Show("提示", "无IP模式配置失败！");
                        FWIP.Text = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                    }
                }
            }                            
        }
        private void ResetIP_Click(object sender, RoutedEventArgs e)
        {
            ResetIPWindow fw = new ResetIPWindow();
            fw.ShowDialog();                      
        }
        private void SaveConfig_click(object sender, RoutedEventArgs e)
        {
            string updateSql ="update fwproperty set 名称='"+ FWname.Text+"',防火墙IP='"+ FWIP.Text +"',防火墙描述='"+ textBox.Text +
                "' where 防火墙ID='"+ StaticGlobal.firewallmac+"';";
            db.dboperate(updateSql);
            DB_DataGridbinding pro = new DB_DataGridbinding();
            pro.PropertyBind(); 
        }
        /* NAT选项卡*/
        private void AddNATRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            NATConfigurationDetail fw = new NATConfigurationDetail();
            fw.ShowDialog();

        }
        private void NATeditclick(object sender, MouseButtonEventArgs e)
        {
            NATConfigurationDetail fwcd = new NATConfigurationDetail();
            if (NATComboBox.SelectedItem.ToString() == "DNAT")
            {
                fwcd.NATComboBox.Text = "DNAT";
                string[] origin_dstIP = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dstIP.ToString().Split('.');
                string[] map_IP = (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_IP.ToString().Split('.');
                fwcd.NATComboBox.Text = NATComboBox.Text.ToString();
                fwcd.srcStarttextBox_1.Text = origin_dstIP[0]; fwcd.srcStarttextBox_2.Text = origin_dstIP[1];
                fwcd.srcStarttextBox_3.Text = origin_dstIP[2]; fwcd.srcStarttextBox_4.Text = origin_dstIP[3];
                fwcd.dstStarttextBox_1.Text = map_IP[0]; fwcd.dstStarttextBox_2.Text = map_IP[1];
                fwcd.dstStarttextBox_3.Text = map_IP[2]; fwcd.dstStarttextBox_4.Text = map_IP[3];
                fwcd.orig_dportBox.Text = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dport.ToString();
                fwcd.nat_dportBox.Text = (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_port.ToString();
                fwcd.ShowDialog();
            }
            if (NATComboBox.SelectedItem.ToString() == "SNAT")
            {
                fwcd.NATComboBox.Text = "SNAT";
                fwcd.iface.Visibility = Visibility.Visible;
                fwcd.ETHComboBox.Visibility = Visibility.Visible;
                fwcd.dstIP.Content = "设备IP地址 ："; fwcd.NATIP.Content = "网口IP :";
                fwcd.orig_dport.Content = "映射IP地址 ：" + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac]; fwcd.orig_dportBox.Visibility = Visibility.Collapsed;
                fwcd.nat_dport.Visibility = Visibility.Collapsed; fwcd.nat_dportBox.Visibility = Visibility.Collapsed;
                string[] origin_devIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).origin_devIP.ToString().Split('.');
                string[] EthIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthIP.ToString().Split('.');
                //string[] NATIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).NATIP.ToString().Split('.');
                fwcd.ETHComboBox.Text = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthName.ToString();
                fwcd.srcStarttextBox_1.Text = origin_devIP[0]; fwcd.srcStarttextBox_2.Text = origin_devIP[1];
                fwcd.srcStarttextBox_3.Text = origin_devIP[2]; fwcd.srcStarttextBox_4.Text = origin_devIP[3];
                fwcd.dstStarttextBox_1.Text = EthIP[0]; fwcd.dstStarttextBox_2.Text = EthIP[1];
                fwcd.dstStarttextBox_3.Text = EthIP[2]; fwcd.dstStarttextBox_4.Text = EthIP[3];
                fwcd.ShowDialog();
            }
        }
        private void NATdeleteclick(object sender, MouseButtonEventArgs e)
        {
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            IConfigureNAT fw = new ConfigureNAT();
            if (NATComboBox.SelectedItem.ToString() == "DNAT")
            {
                string origin_devIP = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dstIP.ToString();
                string EthIP = (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_IP.ToString();
                string origin_dport = (DNATdataGrid.SelectedItem as DNATRuleDataTable).origin_dport.ToString();
                string map_port = (DNATdataGrid.SelectedItem as DNATRuleDataTable).map_port.ToString();
                if (fw.ConfigDNAT(dev_ip, origin_devIP, origin_dport, EthIP, map_port, false))
                {
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Remove((DNATdataGrid.SelectedItem as DNATRuleDataTable));
                    UserMessageBox.Show("提示", "规则删除成功！");
                }
                else UserMessageBox.Show("提示","规则删除失败，请检查连接！");
            }
            if (NATComboBox.SelectedItem.ToString() == "SNAT")
            {
                string origin_devIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).origin_devIP.ToString();
                string EthIP = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthIP.ToString();
                string EthName = (SNATdataGrid.SelectedItem as SNATRuleDataTable).EthName.ToString();
                if (fw.ConfigSNAT(dev_ip, EthName, origin_devIP, EthIP, false))
                {
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Remove((SNATdataGrid.SelectedItem as SNATRuleDataTable));
                    UserMessageBox.Show("提示", "规则删除成功！");
                }
                else UserMessageBox.Show("提示", "规则删除失败，请检查连接！");
            }
        }

        public void NATComboBoxBinding()
        {
            string[] nat_type = { "DNAT", "SNAT" };
            NATComboBox.ItemsSource = nat_type;
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

        /*白名单选项卡*/
        private void NewWHLRulebutton_Click(object sender, RoutedEventArgs e)
        {
            WHLConfigurationDetail fw = new WHLConfigurationDetail();
            fw.ShowDialog();
        }
        private void WHLeditclick(object sender, MouseButtonEventArgs e)
        {
            WHLConfigurationDetail fw = new WHLConfigurationDetail();
            string[] dst_ip = (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_IP.ToString().Split('.');
            string[] src_IP = (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_IP.ToString().Split('.');
            string dst_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).dst_port.ToString();
            string src_port = (WHLdataGrid.SelectedItem as WHLRuleDataTable).src_port.ToString();
            fw.srcStarttextBox_1.Text = src_IP[0]; fw.srcStarttextBox_2.Text = src_IP[1];
            fw.srcStarttextBox_3.Text = src_IP[2]; fw.srcStarttextBox_4.Text = src_IP[3];
            fw.dstStarttextBox_1.Text = dst_ip[0]; fw.dstStarttextBox_2.Text = dst_ip[1];
            fw.dstStarttextBox_3.Text = dst_ip[2]; fw.dstStarttextBox_4.Text = dst_ip[3];
            fw.sportBox.Text = src_port; fw.dportBox.Text = dst_port;
            fw.logcheckBox.IsChecked = (WHLdataGrid.SelectedItem as WHLRuleDataTable).log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().IndexOf(WHLdataGrid.SelectedItem as WHLRuleDataTable);
            StaticGlobal.editflag = true;       
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
            else UserMessageBox.Show("提示","规则删除失败，请检查设备连接！");       
        }

        /*DPI选项卡*/
        private void AddRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            FireWallConfigurationDetail fwcd = new FireWallConfigurationDetail();
            fwcd.ShowDialog();
        }
        private void editclick(object sender, MouseButtonEventArgs e)
        {
            string[] source = (dataGrid.SelectedItem as FireWallRuleDataTable).source.ToString().Split('.');
            string[] destination = (dataGrid.SelectedItem as FireWallRuleDataTable).destination.ToString().Split('.');
            FireWallConfigurationDetail fwcd = new FireWallConfigurationDetail();
            fwcd.ProtocolComboBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).protocol.ToString();
            if (fwcd.ProtocolComboBox.Text == "modbusTcp")
            {
                fwcd.CoilAddresslabel.Visibility = Visibility.Visible;
                fwcd.CoilAddressStarttextBox.Visibility = Visibility.Visible;
                fwcd.ConnectBorder.Visibility = Visibility.Visible;
                fwcd.CoilAddressEndtextBox.Visibility = Visibility.Visible;
                fwcd.AbledFunctionCodelabel.Visibility = Visibility.Visible;
                fwcd.MinDatalabel.Visibility = Visibility.Visible;
                fwcd.MinDatatextBox.Visibility = Visibility.Visible;
                fwcd.MaxDatalabel.Visibility = Visibility.Visible;
                fwcd.MaxDatatextBox.Visibility = Visibility.Visible;
                fwcd.FunctionCodeComboBox.Visibility = Visibility.Visible;
                fwcd.CodeNum.Visibility = Visibility.Visible;
                fwcd.CodeNumLabel.Visibility = Visibility.Visible;
            }
            else
            {
                fwcd.CoilAddresslabel.Visibility = Visibility.Collapsed;
                fwcd.CoilAddressStarttextBox.Visibility = Visibility.Collapsed;
                fwcd.ConnectBorder.Visibility = Visibility.Collapsed;
                fwcd.CoilAddressEndtextBox.Visibility = Visibility.Collapsed;
                fwcd.MinDatalabel.Visibility = Visibility.Collapsed;
                fwcd.MinDatatextBox.Visibility = Visibility.Collapsed;
                fwcd.MaxDatalabel.Visibility = Visibility.Collapsed;
                fwcd.MaxDatatextBox.Visibility = Visibility.Collapsed;
                fwcd.AbledFunctionCodelabel.Visibility = Visibility.Collapsed;
                fwcd.FunctionCodeComboBox.Visibility = Visibility.Collapsed;
                fwcd.CodeNum.Visibility = Visibility.Collapsed;
                fwcd.CodeNumLabel.Visibility = Visibility.Collapsed;
            }
            if (source[0] != "any")
            {
                fwcd.SourceIPtextBox_1.Text = source[0];
                fwcd.SourceIPtextBox_2.Text = source[1];
                fwcd.SourceIPtextBox_3.Text = source[2];
                fwcd.SourceIPtextBox_4.Text = source[3];
            }
            else
            {
                fwcd.SourceIPtextBox_1.Text = "";
                fwcd.SourceIPtextBox_2.Text = "";
                fwcd.SourceIPtextBox_3.Text = "";
                fwcd.SourceIPtextBox_4.Text = "";
            }
            if (destination[0] != "any")
            {
                fwcd.DestinationIPtextBox_1.Text = destination[0];
                fwcd.DestinationIPtextBox_2.Text = destination[1];
                fwcd.DestinationIPtextBox_3.Text = destination[2];
                fwcd.DestinationIPtextBox_4.Text = destination[3];
            }
            else
            {
                fwcd.DestinationIPtextBox_1.Text = "";
                fwcd.DestinationIPtextBox_2.Text = "";
                fwcd.DestinationIPtextBox_3.Text = "";
                fwcd.DestinationIPtextBox_4.Text = "";
            }

            fwcd.CoilAddressStarttextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressstart.ToString();
            fwcd.CoilAddressEndtextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressend.ToString();
            fwcd.MinDatatextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).mindata.ToString();
            fwcd.MaxDatatextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).maxdata.ToString();
            fwcd.FunctionCodeComboBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).func.ToString();
            fwcd.logcheckBox.IsChecked = (dataGrid.SelectedItem as FireWallRuleDataTable).log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().IndexOf(dataGrid.SelectedItem as FireWallRuleDataTable);
            StaticGlobal.editflag = true;
           // this.Close();
            fwcd.ShowDialog();
        }
        private void deleteclick(object sender, MouseButtonEventArgs e)
        {
            IDPIRulesManage rulesmg = new DPIRulesManage();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string source = (dataGrid.SelectedItem as FireWallRuleDataTable).source.ToString();
            string destination = (dataGrid.SelectedItem as FireWallRuleDataTable).destination.ToString();
            string min_addr = (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressstart.ToString();
            string max_addr = (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressend.ToString();
            string func = (dataGrid.SelectedItem as FireWallRuleDataTable).func.ToString();
            int min_data = (dataGrid.SelectedItem as FireWallRuleDataTable).mindata;
            int max_data = (dataGrid.SelectedItem as FireWallRuleDataTable).maxdata;
            bool log = (dataGrid.SelectedItem as FireWallRuleDataTable).log;
            if (rulesmg.ChangeModbusTcpRules(destination, source, min_addr, max_addr, func, min_data, max_data, dev_ip, log, false))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Remove((dataGrid.SelectedItem as FireWallRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }
            else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
        }


        //应用层协议控制APC选项卡
        private void APCeditclick(object sender, MouseButtonEventArgs e)
        {
            APCConfigurationApply fw = new APCConfigurationApply();
            fw.MACAPClabel.Content = "MAC: " + StaticGlobal.firewallmac;
            fw.APCIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
            fw.ShowDialog();
        }


        /*连接数控制CNC选项卡*/
        private void NewCNCRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            CNCConfigurationDetail fw = new CNCConfigurationDetail();
            fw.ShowDialog();
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
            fw.sportBox.Text = sport; fw.dportBox.Text = dport;
            fw.logcheckBox.IsChecked = log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().IndexOf(CNCdataGrid.SelectedItem as CNCRuleDataTable);
            StaticGlobal.editflag = true;
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
            if (fw.DelCNCRules(dev_ip, log, connlimit, srcIP, dstIP, sport, dport))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Remove((CNCdataGrid.SelectedItem as CNCRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }
            else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");

        }
        /*策略路由PRT选项卡*/
        private void NewPRT_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            PRTConfigurationDetail fwcd = new PRTConfigurationDetail();
            fwcd.ShowDialog();
        }

        private void PRTeditclick(object sender, MouseButtonEventArgs e)
        {
            string[] gate = (PRTdataGrid.SelectedItem as PRTRuleDataTable).Gateway.ToString().Split('.');

            PRTConfigurationDetail fw = new PRTConfigurationDetail();
            
            switch ((PRTdataGrid.SelectedItem as PRTRuleDataTable).route_type.ToString())
            {
                case "默认路由":
                   fw. NetInterface.Visibility = Visibility.Visible;
                    fw.GateIP.Visibility = Visibility.Visible;
                    fw.ETHComboBox.Visibility = Visibility.Visible;
                    fw.destIP.Visibility = Visibility.Collapsed;
                    fw.Mask.Visibility = Visibility.Collapsed;
                    fw.GateStarttextBox_1.Text = ""; fw.GateStarttextBox_2.Text = ""; fw.GateStarttextBox_3.Text = ""; fw.GateStarttextBox_4.Text = "";
                    fw.destIPStarttextBox_1.Visibility = Visibility.Collapsed; fw.destIPStarttextBox_2.Visibility = Visibility.Collapsed;
                    fw.destIPStarttextBox_3.Visibility = Visibility.Collapsed; fw.destIPStarttextBox_4.Visibility = Visibility.Collapsed;
                    fw.destsep1.Visibility = Visibility.Collapsed; fw.destsep2.Visibility = Visibility.Collapsed; fw.destsep3.Visibility = Visibility.Collapsed;
                    fw.MaskStarttextBox_1.Visibility = Visibility.Collapsed; fw.MaskStarttextBox_2.Visibility = Visibility.Collapsed;
                    fw.MaskStarttextBox_3.Visibility = Visibility.Collapsed; fw.MaskStarttextBox_4.Visibility = Visibility.Collapsed;
                    fw.Masksep1.Visibility = Visibility.Collapsed; fw.Masksep2.Visibility = Visibility.Collapsed; fw.Masksep3.Visibility = Visibility.Collapsed;
                    break;
                case "主机路由":
                    string[] host = (PRTdataGrid.SelectedItem as PRTRuleDataTable).host_IP.ToString().Split('.');
                    fw.NetInterface.Visibility = Visibility.Visible;
                    fw.GateIP.Visibility = Visibility.Visible;
                    fw.ETHComboBox.Visibility = Visibility.Visible;
                    fw.destIP.Visibility = Visibility.Visible;
                    fw.Mask.Visibility = Visibility.Collapsed;
                    fw.GateStarttextBox_1.Text = ""; fw.GateStarttextBox_2.Text = ""; fw.GateStarttextBox_3.Text = ""; fw.GateStarttextBox_4.Text = "";
                    fw.destIPStarttextBox_1.Text = ""; fw.destIPStarttextBox_2.Text = ""; fw.destIPStarttextBox_3.Text = ""; fw.destIPStarttextBox_4.Text = "";
                    fw.destIPStarttextBox_1.Visibility = Visibility.Visible; fw.destIPStarttextBox_2.Visibility = Visibility.Visible;
                    fw.destIPStarttextBox_3.Visibility = Visibility.Visible; fw.destIPStarttextBox_4.Visibility = Visibility.Visible;
                    fw.destsep1.Visibility = Visibility.Visible; fw.destsep2.Visibility = Visibility.Visible; fw.destsep3.Visibility = Visibility.Visible;
                    fw.MaskStarttextBox_1.Visibility = Visibility.Collapsed; fw.MaskStarttextBox_2.Visibility = Visibility.Collapsed;
                    fw.MaskStarttextBox_3.Visibility = Visibility.Collapsed; fw.MaskStarttextBox_4.Visibility = Visibility.Collapsed;
                    fw.Masksep1.Visibility = Visibility.Collapsed; fw.Masksep2.Visibility = Visibility.Collapsed;
                    fw.Masksep3.Visibility = Visibility.Collapsed;
                    if(host[0]!="")
                    {
                        fw.destIPStarttextBox_1.Text =host[0];
                        fw.destIPStarttextBox_2.Text = host[1];
                        fw.destIPStarttextBox_3.Text = host[2];
                        fw.destIPStarttextBox_4.Text = host[3];
                    }
                    break;
                case "网络路由":
                    string[] dst = (PRTdataGrid.SelectedItem as PRTRuleDataTable).dstIP.ToString().Split('.');
                    string[] mask = (PRTdataGrid.SelectedItem as PRTRuleDataTable).netmask.ToString().Split('.');
                    fw.NetInterface.Visibility = Visibility.Visible;
                    fw.GateIP.Visibility = Visibility.Visible;
                    fw.ETHComboBox.Visibility = Visibility.Visible;
                    fw.destIP.Visibility = Visibility.Visible;
                    fw.destIP.Content = "网络主机IP ：";
                    fw.Mask.Visibility = Visibility.Visible;
                    fw.GateStarttextBox_1.Text = ""; fw.GateStarttextBox_2.Text = ""; fw.GateStarttextBox_3.Text = ""; fw.GateStarttextBox_4.Text = "";
                    fw.GateStarttextBox_1.Visibility = Visibility.Visible; fw.GateStarttextBox_2.Visibility = Visibility.Visible;
                    fw.GateStarttextBox_3.Visibility = Visibility.Visible; fw.GateStarttextBox_4.Visibility = Visibility.Visible;
                    fw.Gatesep1.Visibility = Visibility.Visible; fw.Gatesep2.Visibility = Visibility.Visible; fw.Gatesep3.Visibility = Visibility.Visible; fw.Gatesep1.Visibility = Visibility.Visible;
                    fw.destIPStarttextBox_1.Text = ""; fw.destIPStarttextBox_2.Text = ""; fw.destIPStarttextBox_3.Text = ""; fw.destIPStarttextBox_4.Text = "";
                    fw.destIPStarttextBox_1.Visibility = Visibility.Visible; fw.destIPStarttextBox_2.Visibility = Visibility.Visible;
                    fw.destIPStarttextBox_3.Visibility = Visibility.Visible; fw.destIPStarttextBox_4.Visibility = Visibility.Visible;
                    fw.destsep1.Visibility = Visibility.Visible; fw.destsep2.Visibility = Visibility.Visible; fw.destsep3.Visibility = Visibility.Visible;
                    fw.MaskStarttextBox_1.Text = ""; fw.MaskStarttextBox_2.Text = ""; fw.MaskStarttextBox_3.Text = ""; fw.MaskStarttextBox_4.Text = "";
                    fw.MaskStarttextBox_1.Visibility = Visibility.Visible; fw.MaskStarttextBox_2.Visibility = Visibility.Visible;
                    fw.MaskStarttextBox_3.Visibility = Visibility.Visible; fw.MaskStarttextBox_4.Visibility = Visibility.Visible;
                    fw.Masksep1.Visibility = Visibility.Visible; fw.Masksep2.Visibility = Visibility.Visible; fw.Masksep3.Visibility = Visibility.Visible;
                    if (dst[0] != "")
                    {
                        fw.destIPStarttextBox_1.Text = dst[0];
                        fw.destIPStarttextBox_2.Text = dst[1];
                        fw.destIPStarttextBox_3.Text = dst[2];
                        fw.destIPStarttextBox_4.Text = dst[3];
                    }
                    if(mask[0]!="")
                    {
                        fw.MaskStarttextBox_1.Text = mask[0];
                        fw.MaskStarttextBox_2.Text = mask[1];
                        fw.MaskStarttextBox_3.Text = mask[2];
                        fw.MaskStarttextBox_4.Text = mask[3];
                    }
                    break;
            }
            if (gate[0] != "")
            {
                fw.GateStarttextBox_1.Text = gate[0];
                fw.GateStarttextBox_2.Text = gate[1];
                fw.GateStarttextBox_3.Text = gate[2];
                fw.GateStarttextBox_4.Text = gate[3];
            }
            fw.ETHComboBox.Text= (PRTdataGrid.SelectedItem as PRTRuleDataTable).ETH.ToString();
            fw.logcheckBox.IsChecked = (PRTdataGrid.SelectedItem as PRTRuleDataTable).log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().IndexOf(PRTdataGrid.SelectedItem as PRTRuleDataTable);
            StaticGlobal.editflag = true;
            fw.ShowDialog();
            
        }

        private void PRTdeleteclick(object sender, MouseButtonEventArgs e)
        {
            IPRTRulesManage PRTrule = new PRTRulesManage();
            string devIP = (from devices in StaticGlobal.fwdev_list
                            where devices.getDev_MAC() == StaticGlobal.firewallmac
                            select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string route_type = (PRTdataGrid.SelectedItem as PRTRuleDataTable).route_type.ToString();                     
            string ETH = (PRTdataGrid.SelectedItem as PRTRuleDataTable).ETH.ToString();
            string Gateway = (PRTdataGrid.SelectedItem as PRTRuleDataTable).Gateway.ToString();
            bool log = (PRTdataGrid.SelectedItem as PRTRuleDataTable).log;
            switch (route_type)
            {
                case "默认路由":
                    if (PRTrule.DefaultRouteConfig(devIP, false, ETH, Gateway))
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Remove((PRTdataGrid.SelectedItem as PRTRuleDataTable));
                        UserMessageBox.Show("提示", "规则删除成功！");
                    }
                    else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
                    break;
                case "主机路由":
                    string host_IP = (PRTdataGrid.SelectedItem as PRTRuleDataTable).host_IP.ToString();
                    if (PRTrule.HostRouteConfig(devIP, false, host_IP, ETH, Gateway))
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Remove((PRTdataGrid.SelectedItem as PRTRuleDataTable));
                        UserMessageBox.Show("提示", "规则删除成功！");
                    }
                    else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
                    break;
                case "网络路由":
                    string dstIP = (PRTdataGrid.SelectedItem as PRTRuleDataTable).dstIP.ToString();
                    string netmask = (PRTdataGrid.SelectedItem as PRTRuleDataTable).netmask.ToString();
                    if (PRTrule.NetRouteConfig(devIP, false, dstIP, netmask, ETH, Gateway))
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Remove((PRTdataGrid.SelectedItem as PRTRuleDataTable));
                        UserMessageBox.Show("提示", "规则删除成功！");
                    }
                    else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
                    break;
            }
        }

        /*状态检测STD选项卡*/
        private void NewSTDRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            STDConfigurationDetail fw = new STDConfigurationDetail();
            fw.ShowDialog();
        }

        private void STDeditclick(object sender, MouseButtonEventArgs e)
        {
            STDConfigurationDetail fw = new STDConfigurationDetail();
            string protocol = (STDdataGrid.SelectedItem as STDRuleDataTable).protocol;
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
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().IndexOf(CNCdataGrid.SelectedItem as CNCRuleDataTable);
            StaticGlobal.editflag = true;
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


    }
}
