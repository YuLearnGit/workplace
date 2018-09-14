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

namespace FireWall
{
    /// <summary>
    /// RenameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RenameWindow 
    {
        public RenameWindow()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------
        //函数说明：加载窗口//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TypeBinding();
        }

        private void TypeBinding()
        {
            string[] types = {"asus","BECKOFF","未知设备" };
            PLCtype.ItemsSource = types;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip;
            string IPtext = IPtextBox_1.Text + "." + IPtextBox_2.Text + "." + IPtextBox_3.Text + "." + IPtextBox_4.Text;
            if (IPAddress.TryParse(IPtext, out ip) && PLCtype.Text !="")
            {
                StaticGlobal.newPLC = PLCtype.Text + "  IP: " + IPtext;
                this.Close();
            }
            else
            {
                UserMessageBox.Show("提示", "请输入正确的IP地址！");
            }
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
                if (Convert.ToInt16(textBox.Text) > 0)
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
            if (txt == IPtextBox_1)
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
    }
}
