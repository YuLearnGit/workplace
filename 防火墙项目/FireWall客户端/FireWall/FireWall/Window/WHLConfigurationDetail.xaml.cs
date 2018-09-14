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
    /// WHLConfigurationDetail.xaml 的交互逻辑
    /// </summary>
    public partial class WHLConfigurationDetail : Window
    {
        public WHLConfigurationDetail()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
            if (textBox.Name == "sportBox" || textBox.Name == "dportBox")
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
            if ( txt == srcStarttextBox_1 || txt == dstStarttextBox_1)
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

        private void AddWHL(object sender, RoutedEventArgs e)
        {
            //需要加入的规则信息
            string src_ip = srcStarttextBox_1.Text + '.' + srcStarttextBox_2.Text + '.' + srcStarttextBox_3.Text + '.' + srcStarttextBox_4.Text;
            string dst_ip = dstStarttextBox_1.Text + '.' + dstStarttextBox_2.Text + '.' + dstStarttextBox_3.Text + '.' + dstStarttextBox_4.Text ;
            string sport = sportBox.Text.ToString();
            string dport = dportBox.Text.ToString();
            bool logtext = Convert.ToBoolean(logcheckBox.IsChecked);

            if (src_ip!="" && dst_ip!="" && sport!="" && dport!="")
            {
                WHLConfigurationApply fw = new WHLConfigurationApply();
                if (StaticGlobal.editflag == false)
                {
                    List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()
                                       where rules.dst_IP == dst_ip && rules.src_IP == src_ip && rules.dst_port == dport && rules.src_port == sport
                                       select rules.dst_IP).ToList<string>();
                    if (dt.Count == 0)
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addWHLRule(new WHLRuleDataTable() { dst_IP = dst_ip,
                            src_IP = src_ip, dst_port = dport, src_port = sport, log = logtext });
                        this.Close();
                        fw.MACWHLlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        fw.WHLIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        fw.ShowDialog();
                    }
                    else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                }
                else
                {
                    //被修改的规则信息
                    string editeddstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex].dst_IP;
                    string editedsrcIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex].src_IP;
                    string editeddport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex].dst_port;
                    string editedsport = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex].src_port;
                    bool editedlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex].log;
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex] = new WHLRuleDataTable() { dst_IP = "" };
                    List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()
                                       where  rules.dst_IP == dst_ip && rules.src_IP == src_ip && rules.dst_port == dport && rules.src_port == sport
                                       select rules.dst_IP).ToList<string>();
                    if (dt.Count == 0)
                    {
                        fw.MACWHLlabel.Content = "MAC: " + StaticGlobal.firewallmac;
                        fw.WHLIPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                        fw.ShowDialog();
                    }
                    else
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list()[StaticGlobal.selectedindex] = (new WHLRuleDataTable() {
                            dst_IP = editeddstIP,
                            src_IP = editedsrcIP,
                            dst_port = editeddport,
                            src_port = editedsport,
                            log = editedlog
                        });               
                        UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                    }
                }
            }
            else UserMessageBox.Show("提示", "请输入正确的规则！");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }


    }
}
