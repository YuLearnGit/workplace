using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace FireWall.Window
{
    /// <summary>
    /// AddIPScanRange.xaml 的交互逻辑
    /// </summary>
    public partial class AddIPScanRange 
    {
        public AddIPScanRange()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        string scanstarttext = "";
        string scanendtext = "";
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            scanstarttext = ScanIPStarttextBox_1.Text + "." + ScanIPStarttextBox_2.Text + "." + ScanIPStarttextBox_3.Text + "." + ScanIPStarttextBox_4.Text;
            scanendtext = ScanIPEndtextBox_1.Text + "." + ScanIPEndtextBox_2.Text + "." + ScanIPEndtextBox_3.Text + "." + ScanIPEndtextBox_4.Text;
            IPAddress ip;
            if (IPAddress.TryParse(scanstarttext, out ip) && IPAddress.TryParse(scanendtext, out ip))
            {
                if ((Convert.ToInt16(ScanIPStarttextBox_1.Text) == Convert.ToInt16(ScanIPEndtextBox_1.Text)) && (Convert.ToInt16(ScanIPStarttextBox_2.Text) == Convert.ToInt16(ScanIPEndtextBox_2.Text)) && (Convert.ToInt16(ScanIPStarttextBox_3.Text) == Convert.ToInt16(ScanIPEndtextBox_3.Text)) && (Convert.ToInt16(ScanIPStarttextBox_4.Text) <= Convert.ToInt16(ScanIPEndtextBox_4.Text)))
                {
                    //将设备扫描范围存入配置文件
                    XmlSerializationHelper configContext = new XmlSerializationHelper("Config");
                    GlobalConfig globalconfig = configContext.Get<GlobalConfig>();
                    string Awarry = globalconfig.ScanIPConfig[0].scanip;
                    globalconfig.ScanIPConfig[0].scanip = Awarry + "+" + scanstarttext + "-" + scanendtext;
                    configContext.Save(globalconfig);
                    StaticGlobal.ScanIP = globalconfig.ScanIPConfig[0].scanip;
                    this.Close(); 

                }
                else UserMessageBox.Show("提示", "请输入正确的范围！");
            }
            else UserMessageBox.Show("提示", "请输入正确的IP！");
        }

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
            if (textBox == ScanIPStarttextBox_1)
            {
                ScanIPEndtextBox_1.Text = ScanIPStarttextBox_1.Text;
            }
            if (textBox == ScanIPEndtextBox_1)
            {
                ScanIPStarttextBox_1.Text = ScanIPEndtextBox_1.Text;
            }
            if (textBox == ScanIPStarttextBox_2)
            {
                ScanIPEndtextBox_2.Text = ScanIPStarttextBox_2.Text;
            }
            if (textBox == ScanIPEndtextBox_2)
            {
                ScanIPStarttextBox_2.Text = ScanIPEndtextBox_2.Text;
            }
            if (textBox == ScanIPStarttextBox_3)
            {
                ScanIPEndtextBox_3.Text = ScanIPStarttextBox_3.Text;
            }
            if (textBox == ScanIPEndtextBox_3)
            {
                ScanIPStarttextBox_3.Text = ScanIPEndtextBox_3.Text;
            }
        }


        //只能输入数字
        private void keydown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if (txt == ScanIPStarttextBox_1 || txt == ScanIPEndtextBox_1)
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
