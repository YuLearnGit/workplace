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

namespace FireWall
{
    /// <summary>
    /// NATConfigurationDetail.xaml 的交互逻辑
    /// </summary>
    public partial class NATConfigurationDetail : Window
    {
        public NATConfigurationDetail()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NATComboBoxBinding();
            ETHComboBoxBinding();
        }
        private void textchanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);
            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num) || textBox.Text.Contains("e") || textBox.Text.Contains(".") || textBox.Text.Contains(","))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
            if (textBox.Name == "orig_dportBox" || textBox.Name == "nat_dportBox")
            {
                if (textBox.Text != "")
                {
                    if (Convert.ToInt32(textBox.Text) >= 0)
                    {
                        textBox.Text = Convert.ToString(Convert.ToInt32(textBox.Text));
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    if (Convert.ToInt32(textBox.Text) > 65535)
                    {
                        textBox.Text = Convert.ToString(65535);
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }
            }
            else
            {
                if (textBox.Text != "")
                {
                    if (Convert.ToInt16(textBox.Text) >= 0)
                    {
                        textBox.Text = Convert.ToString(Convert.ToInt16(textBox.Text));
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    if (Convert.ToInt16(textBox.Text) > 255)
                    {
                        textBox.Text = Convert.ToString(255);
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }
            }
        }

        private void keydown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if (txt == srcStarttextBox_1 || txt == dstStarttextBox_1)
            {
                if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Tab)
                {
                    if (txt.Text == "" && (e.Key == Key.NumPad0 || e.Key == Key.D0))
                    {
                        e.Handled = true;
                        return;
                    }
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Tab || e.Key == Key.Enter)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
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
            string[] nat_type = {"DNAT","SNAT" };
            NATComboBox.ItemsSource = nat_type;
        }
        public void ETHComboBoxBinding()
        {
            string[] iface = {"eth0","eth1","eth2","eth3" };
            ETHComboBox.ItemsSource = iface;
        }

        private void AddNATbutton_Click_1(object sender, RoutedEventArgs e)
        {
            string NATtype =NATComboBox.SelectedItem.ToString();
            string iface =ETHComboBox.SelectedItem.ToString();
            string sIP = srcStarttextBox_1.Text+'.'+ srcStarttextBox_2.Text + '.' + srcStarttextBox_3.Text + '.' + srcStarttextBox_4.Text;
            string dIP = dstStarttextBox_1.Text+'.'+ dstStarttextBox_2.Text + '.' + dstStarttextBox_3.Text + '.' + dstStarttextBox_4.Text;
            bool logtext = Convert.ToBoolean(logcheckBox.IsChecked);
            NATConfigurationApply fw = new NATConfigurationApply();        
            if (NATComboBox.Text == "SNAT")
            {
                if (srcStarttextBox_1.Text != "" && srcStarttextBox_2.Text != "" && srcStarttextBox_3.Text != "" && srcStarttextBox_4.Text != ""
                    && dstStarttextBox_1.Text != "" && dstStarttextBox_2.Text != "" && dstStarttextBox_3.Text != "" && dstStarttextBox_4.Text != "")
                {
                    foreach (var SNAT in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list())
                        if (SNAT.EthName == iface)
                            UserMessageBox.Show("提示", "该网口已经存在源地址映射规则！");
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from SNATrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()
                                           where SNATrules.EthName == iface && SNATrules.origin_devIP == sIP && SNATrules.EthIP==dIP && SNATrules.NATIP == StaticGlobal.FwMACandIP[StaticGlobal.firewallmac]
                                           select SNATrules.EthName).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addSNATRule(new SNATRuleDataTable()
                            { EthName = iface, origin_devIP = sIP, EthIP = dIP, NATIP = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] });
                            this.Close();
                            fw.NATMAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.NATIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.NATComboBox.Text = "SNAT";
                            fw.SNATdataGrid.Visibility = Visibility.Visible;
                            fw.DNATdataGrid.Visibility = Visibility.Collapsed;
                            fw.ShowDialog();
                        }
                        else
                            UserMessageBox.Show("提示","添加失败，已经存在此规则！");
                    }
                    else
                    {
                        string editiface =StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[StaticGlobal.selectedindex].EthName;
                        string editsIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[StaticGlobal.selectedindex].origin_devIP;
                        string editdIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[StaticGlobal.selectedindex].EthIP;
                        List<string> dt= (from SNATrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()
                                          where SNATrules.EthName == iface && SNATrules.origin_devIP == sIP && SNATrules.EthIP == dIP && SNATrules.NATIP == StaticGlobal.FwMACandIP[StaticGlobal.firewallmac]
                                          select SNATrules.EthName).ToList<string>();
                        if(dt.Count==0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[StaticGlobal.selectedindex] = new SNATRuleDataTable()
                            { EthName = iface, origin_devIP = sIP, EthIP = dIP, NATIP = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] };              
                            this.Close();
                            fw.NATMAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.NATIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.NATComboBox.Text = "SNAT";
                            fw.SNATdataGrid.Visibility = Visibility.Visible;
                            fw.DNATdataGrid.Visibility = Visibility.Collapsed;
                            fw.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list()[StaticGlobal.selectedindex] = new SNATRuleDataTable()
                            { EthName = editiface, origin_devIP = editsIP, EthIP = editdIP, NATIP = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] };
                            UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                        }
                    }
                }
                else
                {
                    UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                }
            }
            else if(NATComboBox.Text=="DNAT")
            {
                if (srcStarttextBox_1.Text != "" && srcStarttextBox_2.Text != "" && srcStarttextBox_3.Text != "" && srcStarttextBox_4.Text != ""
                 && dstStarttextBox_1.Text != "" && dstStarttextBox_2.Text != "" && dstStarttextBox_3.Text != "" && dstStarttextBox_4.Text != ""
                 && orig_dportBox.Text!="" && nat_dportBox.Text!="")
                {
                    if(StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from DNATrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()
                                           where DNATrules.origin_dstIP == sIP && DNATrules.origin_dport == orig_dportBox.Text && DNATrules.map_IP == dIP && DNATrules.map_port== nat_dportBox.Text
                                           select DNATrules.origin_dstIP).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addDNATRule(new DNATRuleDataTable
                            { origin_dstIP = sIP, origin_dport = orig_dportBox.Text, map_IP = dIP, map_port = nat_dportBox.Text });                            
                            this.Close();
                            fw.NATMAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.NATIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else
                        {                          
                            UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                        }
                    }
                    else
                    {
                        string editorigin_dstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex].origin_dstIP;
                        string editorigin_dport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex].origin_dport;
                        string editmap_IP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex].map_IP;
                        string editmap_port = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex].map_port;
                        List<string> dt = (from DNATrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()
                                           where DNATrules.origin_dstIP == sIP && DNATrules.origin_dport == orig_dportBox.Text && DNATrules.map_IP == dIP && DNATrules.map_port == nat_dportBox.Text
                                           select DNATrules.origin_dstIP).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex] = new DNATRuleDataTable()
                            { origin_dstIP = sIP, origin_dport = orig_dportBox.Text, map_IP = dIP, map_port = nat_dportBox.Text };
                            this.Close();
                            fw.NATMAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.NATIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list()[StaticGlobal.selectedindex] = new DNATRuleDataTable()
                            { origin_dstIP = editorigin_dstIP, origin_dport = editorigin_dport, map_IP = editmap_IP, map_port = editmap_port };
                            UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                        }
                    }
                }
                else
                {
                    UserMessageBox.Show("提示", "请输入正确的规则！");
                }
            }
        }

        private void NATComboBox_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (NATComboBox.SelectedItem.ToString() == "DNAT")
            {
                iface.Visibility = Visibility.Collapsed;
                ETHComboBox.Visibility = Visibility.Collapsed;
                dstIP.Visibility = Visibility.Visible;
                dstIP.Content = "目的 IP ：";
                srcStarttextBox_1.Visibility = Visibility.Visible; srcStarttextBox_2.Visibility = Visibility.Visible;
                srcStarttextBox_3.Visibility = Visibility.Visible; srcStarttextBox_4.Visibility = Visibility.Visible;
                srcStarttextBox_1.Text = ""; srcStarttextBox_2.Text = ""; srcStarttextBox_3.Text = ""; srcStarttextBox_4.Text = "";
                NATIP.Visibility = Visibility.Visible;
                NATIP.Content = "映射目的 IP ：";
                dstStarttextBox_1.Visibility = Visibility.Visible; dstStarttextBox_2.Visibility = Visibility.Visible;
                dstStarttextBox_3.Visibility = Visibility.Visible; dstStarttextBox_4.Visibility = Visibility.Visible;
                dstStarttextBox_1.Text = ""; dstStarttextBox_2.Text = ""; dstStarttextBox_3.Text = ""; dstStarttextBox_4.Text = "";
                orig_dport.Visibility = Visibility.Visible;orig_dport.Content = "目的端口 ：";
                orig_dportBox.Visibility = Visibility.Visible;
                nat_dport.Visibility = Visibility.Visible; nat_dportBox.Visibility = Visibility.Visible;

            }
            if (NATComboBox.SelectedItem.ToString() == "SNAT")
            {
                iface.Visibility = Visibility.Visible;
                ETHComboBox.Visibility = Visibility.Visible;
                dstIP.Content = "设备IP地址 ："; NATIP.Content = "网口IP :";
                orig_dport.Content = "映射IP地址 ："+ StaticGlobal.FwMACandIP[StaticGlobal.firewallmac]; orig_dportBox.Visibility = Visibility.Collapsed;
                nat_dport.Visibility = Visibility.Collapsed; nat_dportBox.Visibility = Visibility.Collapsed;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
