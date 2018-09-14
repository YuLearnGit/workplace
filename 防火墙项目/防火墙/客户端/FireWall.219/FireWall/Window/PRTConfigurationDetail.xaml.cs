using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireWall
{
    /// <summary>
    /// PRTConfigurationDetail.xaml 的交互逻辑
    /// </summary>
    public partial class PRTConfigurationDetail : Window
    {
        public PRTConfigurationDetail()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PRTBinding();
            ETHBinding();
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


        /* IP输入控件*/
        //屏蔽中文输入和非法字符粘贴输入以及判断IP输入是否合法
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

        //只能输入数字
        private void keydown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if (txt== destIPStarttextBox_1 || txt== MaskStarttextBox_1)
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

        /* 路由模式选择控件*/
        private void PRTBinding()
        {
            string[] PRT = { "默认路由", "主机路由", "网络路由" };
            PRTComboBox.ItemsSource = PRT;         
        }
        private void PRTSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            switch (PRTComboBox.SelectedItem.ToString())
            {
                case "默认路由":                  
                    NetInterface.Visibility = Visibility.Visible;
                    GateIP.Visibility = Visibility.Visible;
                    ETHComboBox.Visibility = Visibility.Visible;
                    destIP.Visibility = Visibility.Collapsed;
                    Mask.Visibility = Visibility.Collapsed;
                    GateStarttextBox_1.Text = ""; GateStarttextBox_2.Text = ""; GateStarttextBox_3.Text = ""; GateStarttextBox_4.Text = "";
                    destIPStarttextBox_1.Visibility= Visibility.Collapsed; destIPStarttextBox_2.Visibility = Visibility.Collapsed;
                    destIPStarttextBox_3.Visibility = Visibility.Collapsed; destIPStarttextBox_4.Visibility = Visibility.Collapsed;
                    destsep1.Visibility = Visibility.Collapsed; destsep2.Visibility = Visibility.Collapsed; destsep3.Visibility = Visibility.Collapsed;
                    MaskStarttextBox_1.Visibility = Visibility.Collapsed; MaskStarttextBox_2.Visibility = Visibility.Collapsed;
                    MaskStarttextBox_3.Visibility = Visibility.Collapsed; MaskStarttextBox_4.Visibility = Visibility.Collapsed;
                    Masksep1.Visibility = Visibility.Collapsed; Masksep2.Visibility = Visibility.Collapsed; Masksep3.Visibility = Visibility.Collapsed;
                    break;

                case "主机路由":
                    NetInterface.Visibility = Visibility.Visible;
                    GateIP.Visibility = Visibility.Visible;
                    ETHComboBox.Visibility = Visibility.Visible;
                    destIP.Visibility = Visibility.Visible;
                    destIP.Content = "目的主机IP ：";
                    Mask.Visibility = Visibility.Collapsed;
                    GateStarttextBox_1.Text = ""; GateStarttextBox_2.Text = ""; GateStarttextBox_3.Text = ""; GateStarttextBox_4.Text = "";
                    destIPStarttextBox_1.Text = ""; destIPStarttextBox_2.Text = ""; destIPStarttextBox_3.Text = ""; destIPStarttextBox_4.Text = "";
                    destIPStarttextBox_1.Visibility = Visibility.Visible; destIPStarttextBox_2.Visibility = Visibility.Visible;
                    destIPStarttextBox_3.Visibility = Visibility.Visible; destIPStarttextBox_4.Visibility = Visibility.Visible;
                    destsep1.Visibility = Visibility.Visible; destsep2.Visibility = Visibility.Visible; destsep3.Visibility = Visibility.Visible;
                    MaskStarttextBox_1.Visibility = Visibility.Collapsed; MaskStarttextBox_2.Visibility = Visibility.Collapsed;
                    MaskStarttextBox_3.Visibility = Visibility.Collapsed; MaskStarttextBox_4.Visibility = Visibility.Collapsed;
                    Masksep1.Visibility = Visibility.Collapsed; Masksep2.Visibility = Visibility.Collapsed;
                    Masksep3.Visibility = Visibility.Collapsed;
                    break;
                case "网络路由":
                    NetInterface.Visibility = Visibility.Visible;
                    GateIP.Visibility = Visibility.Visible;
                    ETHComboBox.Visibility = Visibility.Visible;
                    destIP.Visibility = Visibility.Visible;
                    destIP.Content = "网络主机IP ：";
                    Mask.Visibility = Visibility.Visible;
                    GateStarttextBox_1.Text = ""; GateStarttextBox_2.Text = ""; GateStarttextBox_3.Text = ""; GateStarttextBox_4.Text = "";
                    GateStarttextBox_1.Visibility = Visibility.Visible; GateStarttextBox_2.Visibility = Visibility.Visible;
                    GateStarttextBox_3.Visibility = Visibility.Visible; GateStarttextBox_4.Visibility = Visibility.Visible;
                    Gatesep1.Visibility = Visibility.Visible; Gatesep2.Visibility = Visibility.Visible; Gatesep3.Visibility = Visibility.Visible;Gatesep1.Visibility = Visibility.Visible;
                    destIPStarttextBox_1.Text = ""; destIPStarttextBox_2.Text = ""; destIPStarttextBox_3.Text = ""; destIPStarttextBox_4.Text = "";
                    destIPStarttextBox_1.Visibility = Visibility.Visible; destIPStarttextBox_2.Visibility = Visibility.Visible;
                    destIPStarttextBox_3.Visibility = Visibility.Visible; destIPStarttextBox_4.Visibility = Visibility.Visible;
                    destsep1.Visibility = Visibility.Visible; destsep2.Visibility = Visibility.Visible; destsep3.Visibility = Visibility.Visible;
                    MaskStarttextBox_1.Text = ""; MaskStarttextBox_2.Text = ""; MaskStarttextBox_3.Text = ""; MaskStarttextBox_4.Text = "";
                    MaskStarttextBox_1.Visibility = Visibility.Visible; MaskStarttextBox_2.Visibility = Visibility.Visible;
                    MaskStarttextBox_3.Visibility = Visibility.Visible; MaskStarttextBox_4.Visibility = Visibility.Visible;
                    Masksep1.Visibility = Visibility.Visible; Masksep2.Visibility = Visibility.Visible; Masksep3.Visibility = Visibility.Visible; 
                    break;
            }


        }
        //网口选择控件
        private void ETHBinding()
        {
            string[] ETH = { "eth0", "eth1", "eth2","eth3" };
            ETHComboBox.ItemsSource = ETH;        
        }

        private void AddPRT(object sender, RoutedEventArgs e)
        {
            PRTConfigurationApply fw = new PRTConfigurationApply();
            string hosttext = null;string nettext = null;string ifacetext = ETHComboBox.SelectedItem.ToString(); string netmasktext = null;
            string gatewaytext = GateStarttextBox_1.Text+"."+ GateStarttextBox_2.Text+"."+ GateStarttextBox_3.Text+"."+ GateStarttextBox_4.Text;
            string route_typetext = PRTComboBox.SelectedItem.ToString();bool logtext = Convert.ToBoolean(logcheckBox.IsChecked);
            if(route_typetext == "默认路由")
            {
                if( gatewaytext!="")
                {
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                               
                                route_type = route_typetext,
                                host_IP = hosttext,
                                dstIP = nettext,
                                netmask = netmasktext,
                                ETH = ifacetext,
                                Gateway = gatewaytext,
                                log = logtext
                            });
                            this.Close();
                            fw.MACPRTlabel.Content= "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content= "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                    }
                    else
                    {//被修改的规则信息
                        string editroute_type= StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].route_type;
                        string edithost = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].host_IP;
                        string editnet = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].dstIP;
                        string editnetmask = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].netmask;
                        string editiface = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].ETH;
                        string editgateway = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].Gateway;
                        bool editlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].log;
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex] = new PRTRuleDataTable() { route_type = "" };
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex] = new PRTRuleDataTable()
                            {
                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            };
                            this.Close();
                            fw.MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            });
                            UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                        }
                    }
                }
                else UserMessageBox.Show("提示", "请输入正确的规则！");
            }
            if (route_typetext == "主机路由")
            {
                hosttext = destIPStarttextBox_1.Text + '.' + destIPStarttextBox_2.Text + '.' + destIPStarttextBox_3.Text + '.' + destIPStarttextBox_4.Text;
                if (gatewaytext != "" &&hosttext!="")
                {
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where  PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                                route_type = route_typetext,
                                host_IP = hosttext,
                                dstIP = nettext,
                                netmask = netmasktext,
                                ETH = ifacetext,
                                Gateway = gatewaytext,
                                log = logtext
                            });
                            this.Close();
                            fw.MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                    }
                    else
                    {//被修改的规则信息
                        string editroute_type = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].route_type;
                        string edithost = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].host_IP;
                        string editnet = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].dstIP;
                        string editnetmask = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].netmask;
                        string editiface = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].ETH;
                        string editgateway = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].Gateway;
                        bool editlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].log;
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex] = new PRTRuleDataTable() { route_type = "" };
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {

                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            });
                            this.Close();
                            fw.MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            });
                            UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                        }
                    }
                }
                else UserMessageBox.Show("提示", "请输入正确的规则！");
            }
            if (route_typetext == "网络路由")
            {
                netmasktext = MaskStarttextBox_1.Text + '.' + MaskStarttextBox_2.Text + '.' + MaskStarttextBox_3.Text + '.' + MaskStarttextBox_4.Text ;
                nettext= destIPStarttextBox_1.Text + '.' + destIPStarttextBox_2.Text + '.' + destIPStarttextBox_3.Text + '.' + destIPStarttextBox_4.Text;
                if (gatewaytext != "" && nettext!="" && netmasktext!="")
                {
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                                route_type = route_typetext,
                                host_IP = hosttext,
                                dstIP = nettext,
                                netmask = netmasktext,
                                ETH = ifacetext,
                                Gateway = gatewaytext,
                                log = logtext
                            });
                            this.Close();
                            fw.MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                    }
                    else
                    {//被修改的规则信息
                        string editroute_type = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].route_type;
                        string edithost = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].host_IP;
                        string editnet = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].dstIP;
                        string editnetmask = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].netmask;
                        string editiface = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].ETH;
                        string editgateway = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].Gateway;
                        bool editlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex].log;
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[StaticGlobal.selectedindex] = new PRTRuleDataTable() { route_type = "" };
                        List<string> dt = (from PRTrules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()
                                           where  PRTrules.route_type == route_typetext && PRTrules.host_IP == hosttext
                                           && PRTrules.dstIP == nettext && PRTrules.ETH == ifacetext && PRTrules.netmask == netmasktext && PRTrules.Gateway == gatewaytext
                                           select PRTrules.route_type).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {
                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            });
                            this.Close();
                            fw.MACPRTlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fw.PRTIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fw.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallPRTRule(new PRTRuleDataTable()
                            {         
                                route_type = editroute_type,
                                host_IP = edithost,
                                dstIP = editnet,
                                netmask = editnetmask,
                                ETH = editiface,
                                Gateway = editgateway,
                                log = editlog
                            });
                            UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                        }
                    }
                }
                else UserMessageBox.Show("提示", "请输入正确的规则！");
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
