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
    /// ResetIP.xaml 的交互逻辑
    /// </summary>
    public partial class ResetIPWindow 
    {
        public ResetIPWindow()
        {
            InitializeComponent();
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

        private void keydown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if (txt == IPStarttextBox_1 )
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

        private void cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if (IPStarttextBox_1.Text != "" && IPStarttextBox_2.Text != "" && IPStarttextBox_3.Text != "" && IPStarttextBox_4.Text != "")
            {
                string resetIP = IPStarttextBox_1.Text + '.' + IPStarttextBox_2.Text + '.' + IPStarttextBox_3.Text + '.' + IPStarttextBox_4.Text;
                StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] = resetIP;
                IResetIP res = new ResetIP();
                for (int i = 0; i < StaticGlobal.fwdev_list.Count(); i++)
                {

                    if (StaticGlobal.fwdev_list[i].getDev_MAC() == StaticGlobal.firewallmac)
                    {
                       if( res.ResetIP(StaticGlobal.fwdev_list[i].getProtecDev_list()[0], resetIP))
                        {
                            UserMessageBox.Show("提示","IP重设成功");
                            StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] = resetIP;
                        }
                        else
                        {
                            UserMessageBox.Show("提示", "无IP模式配置失败！");
                        }
                    }
                }
                this.Close();
                ConfigFW update = new ConfigFW();
                update.FWIP.Text = StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                //update.FWIP.AppendText(StaticGlobal.FwMACandIP[StaticGlobal.firewallmac]);
            }
            else
            {
                UserMessageBox.Show("提示","请输入正确的IP地址");
            }
        }
    }
}
