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
    /// FireWallConfigurationDetail.xaml 的交互逻辑
    /// </summary>
    public partial class FireWallConfigurationDetail : Window
    {
        public FireWallConfigurationDetail()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FunctionCodeBinding();
            ProtocolBinding();
            CodeNum.Text = FunctionCodeComboBox.SelectedItems.Count().ToString();
        }

        private void ProtocolBinding()
        {
            string[] protocol = { "modbusTcp", "OPC","DNP3" };
            ProtocolComboBox.ItemsSource = protocol;
        }

        private void ProtocolSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (ProtocolComboBox.SelectedItem.ToString() == "modbusTcp")
            {
                CoilAddresslabel.Visibility = Visibility.Visible;
                CoilAddressStarttextBox.Visibility = Visibility.Visible;
                ConnectBorder.Visibility = Visibility.Visible;
                CoilAddressEndtextBox.Visibility = Visibility.Visible;
                MinDatalabel.Visibility = Visibility.Visible;
                MinDatatextBox.Visibility = Visibility.Visible;
                MaxDatalabel.Visibility = Visibility.Visible;
                MaxDatatextBox.Visibility = Visibility.Visible;
                AbledFunctionCodelabel.Visibility = Visibility.Visible;
                FunctionCodeComboBox.Visibility = Visibility.Visible;
                CodeNum.Visibility = Visibility.Visible;
                CodeNumLabel.Visibility = Visibility.Visible;
                SourceIPtextBox_1.Text = "";
                SourceIPtextBox_2.Text = "";
                SourceIPtextBox_3.Text = "";
                SourceIPtextBox_4.Text = "";
                DestinationIPtextBox_1.Text = "";
                DestinationIPtextBox_2.Text = "";
                DestinationIPtextBox_3.Text = "";
                DestinationIPtextBox_4.Text = "";
                CoilAddressStarttextBox.Text = "";
                CoilAddressEndtextBox.Text = "";
                MinDatatextBox.Text = null;
                MaxDatatextBox.Text = null;
                FunctionCodeComboBox.SelectedItems.Clear();
            }
            else
            {
                CoilAddresslabel.Visibility = Visibility.Collapsed;
                CoilAddressStarttextBox.Visibility = Visibility.Collapsed;
                ConnectBorder.Visibility = Visibility.Collapsed;
                CoilAddressEndtextBox.Visibility = Visibility.Collapsed;
                MinDatalabel.Visibility = Visibility.Collapsed;
                MinDatatextBox.Visibility = Visibility.Collapsed;
                MaxDatalabel.Visibility = Visibility.Collapsed;
                MaxDatatextBox.Visibility = Visibility.Collapsed;
                AbledFunctionCodelabel.Visibility = Visibility.Collapsed;
                FunctionCodeComboBox.Visibility = Visibility.Collapsed;
                CodeNum.Visibility = Visibility.Collapsed;
                CodeNumLabel.Visibility = Visibility.Collapsed;
                SourceIPtextBox_1.Text = "";
                SourceIPtextBox_2.Text = "";
                SourceIPtextBox_3.Text = "";
                SourceIPtextBox_4.Text = "";
                DestinationIPtextBox_1.Text = "";
                DestinationIPtextBox_2.Text = "";
                DestinationIPtextBox_3.Text = "";
                DestinationIPtextBox_4.Text = "";
                CoilAddressStarttextBox.Text = "";
                CoilAddressEndtextBox.Text = "";
                MinDatatextBox.Text = null;
                MaxDatatextBox.Text = null;
                FunctionCodeComboBox.SelectedItems.Clear();
            }
        }


        private void FunctionCodeBinding()
        {
            List<string> list = new List<string>();
            for (int i = 1; i <= StaticGlobal.FunctionCodeNumber; i++)
            {
                list.Add(Convert.ToString(i, 10).PadLeft(2,'0'));
            }
            String[] functioncode = new String[list.Count()];
            functioncode=list.ToArray();
            FunctionCodeComboBox.ItemsSource = functioncode;
        }

        //*----------------------------------------------------------------
        //函数说明：窗口拖动事件
        //输入：无
        //输出：无
        //----------------------------------------------------------------*//  
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

        private void Addbutton_Click(object sender, RoutedEventArgs e)
        {
            //需要加入的规则信息
            string protocoltext = ProtocolComboBox.SelectedItem.ToString();           
            string sourcetext;
            if(SourceIPCheck.IsChecked == false )
            {
                sourcetext = SourceIPtextBox_1.Text + "." + SourceIPtextBox_2.Text + "." + SourceIPtextBox_3.Text + "." + SourceIPtextBox_4.Text;
            }
            else
            {
                sourcetext = "any";
            }         
            string destinationtext;
            if(DestinationIPCheck.IsChecked == false)
            {
                destinationtext = DestinationIPtextBox_1.Text + "." + DestinationIPtextBox_2.Text + "." + DestinationIPtextBox_3.Text + "." + DestinationIPtextBox_4.Text;
            }
            else
            {
                destinationtext = "any";
            }
            string coiladdressstarttext = CoilAddressStarttextBox.Text;
            string coiladdressendtext = CoilAddressEndtextBox.Text;
            int mindatatext = Convert.ToInt16( MinDatatextBox.Text);
            int maxdatatext = Convert.ToInt16(MaxDatatextBox.Text);
            string functioncodetext = FunctionCodeComboBox.Text;
            bool logtext = Convert.ToBoolean(logcheckBox.IsChecked);            
            IPAddress ip;
            if (ProtocolComboBox.Text == "modbusTcp")
            {
                //IPAddress.TryParse(sourcetext, out ip) && IPAddress.TryParse(destinationtext, out ip) &&
                if (coiladdressstarttext != "" && coiladdressendtext != "" && functioncodetext != "" && mindatatext >= 0 && maxdatatext >= 0)
                {
                    FireWallConfigurationApply fwca = new FireWallConfigurationApply();
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()
                                           where rules.protocol == protocoltext && rules.source == sourcetext && rules.destination == destinationtext && rules.coiladdressstart == coiladdressstarttext && rules.coiladdressend == coiladdressendtext && rules.func == functioncodetext && rules.mindata == mindatatext && rules.maxdata == maxdatatext
                                           select rules.protocol).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallRule(new FireWallRuleDataTable() { protocol = protocoltext, source = sourcetext, destination = destinationtext, coiladdressstart = coiladdressstarttext, coiladdressend = coiladdressendtext, mindata = mindatatext, maxdata = maxdatatext, func = functioncodetext, log = logtext });
                            this.Close();
                            fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fwca.ShowDialog();
                        }
                        else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                    }                        
                    else
                    {
                        //被修改的规则信息
                        string editedprotocol = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].protocol;
                        string editedsource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].source;
                        string editeddestination = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].destination;
                        string editedcoiladdressstart = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].coiladdressstart;
                        string editedcoiladdressend = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].coiladdressend;
                        int editedmindata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].mindata;
                        int editedmaxdata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].maxdata;
                        string editedfunctioncode = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].func;
                        bool editedlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].log;
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = "" };
                        List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()
                                           where rules.protocol == protocoltext && rules.source == sourcetext && rules.destination == destinationtext && rules.coiladdressstart == coiladdressstarttext && rules.coiladdressend == coiladdressendtext && rules.func == functioncodetext && rules.mindata == mindatatext && rules.maxdata == maxdatatext
                                           select rules.protocol).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = protocoltext, source = sourcetext, destination = destinationtext, coiladdressstart = coiladdressstarttext, coiladdressend = coiladdressendtext, mindata = mindatatext, maxdata = maxdatatext, func = functioncodetext, log = logtext };
                            this.Close();
                            fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fwca.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = editedprotocol, source = editedsource, destination = editeddestination, coiladdressstart = editedcoiladdressstart, coiladdressend = editedcoiladdressend, mindata = editedmindata, maxdata = editedmaxdata, func = editedfunctioncode, log = editedlog };
                            UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                        }
                    }
                }
                else UserMessageBox.Show("提示", "请输入正确的规则！");
            }
           else
            {
                if (IPAddress.TryParse(sourcetext, out ip) && IPAddress.TryParse(destinationtext, out ip))
                {
                    FireWallConfigurationApply fwca = new FireWallConfigurationApply();
                    if (StaticGlobal.editflag == false)
                    {
                        List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()
                                           where rules.protocol == protocoltext && rules.source == sourcetext && rules.destination == destinationtext
                                           select rules.protocol).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].addFireWallRule(new FireWallRuleDataTable() { protocol = protocoltext, source = sourcetext, destination = destinationtext, coiladdressstart = coiladdressstarttext, coiladdressend = coiladdressendtext, func = functioncodetext, log = logtext });
                            this.Close();
                            fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fwca.ShowDialog();
                        }
                        else UserMessageBox.Show("提示", "添加失败，已经存在此规则！");
                    }
                    else
                    {
                        //被修改的规则信息
                        string editedprotocol = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].protocol;
                        string editedsource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].source;
                        string editeddestination = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].destination;
                        string editedcoiladdressstart = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].coiladdressstart;
                        string editedcoiladdressend = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].coiladdressend;
                        int editedmindata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].mindata;
                        int editedmaxdata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].maxdata;
                        string editedfunctioncode = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].func;
                        bool editedlog = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex].log;
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = "" };
                        List<string> dt = (from rules in StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()
                                           where rules.protocol == protocoltext && rules.source == sourcetext && rules.destination == destinationtext 
                                           select rules.protocol).ToList<string>();
                        if (dt.Count == 0)
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = protocoltext, source = sourcetext, destination = destinationtext, coiladdressstart = coiladdressstarttext, coiladdressend = coiladdressendtext, func = functioncodetext, log = logtext };
                            this.Close();
                            fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
                            fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
                            fwca.ShowDialog();
                        }
                        else
                        {
                            StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[StaticGlobal.selectedindex] = new FireWallRuleDataTable() { protocol = editedprotocol, source = editedsource, destination = editeddestination, coiladdressstart = editedcoiladdressstart, coiladdressend = editedcoiladdressend, func = editedfunctioncode, log = editedlog };
                            UserMessageBox.Show("提示", "编辑失败，已经存在此规则！");
                        }
                    }
                }
                else UserMessageBox.Show("提示", "请输入正确的规则！");
            }
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //FireWallConfigurationApply fwca = new FireWallConfigurationApply();
            //fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
            //fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
            //fwca.ShowDialog();
        }

        private void PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            //string selectcodetext = null;
            
            if(FunctionCodeComboBox.SelectedItem != null)
            {
                string selectcode = FunctionCodeComboBox.SelectedItem.ToString();
                FunctionCodeComboBox.Text = selectcode;
            }
           else
            {
                UserMessageBox.Show("提醒", "请选择功能码！");
            }
            //string[] selectcode = new string[FunctionCodeComboBox.SelectedItems.Count()];
            //if (FunctionCodeComboBox.SelectedItems.Count()!=0)
            //{
            //    int index = 0;
            //    for (int i = 0; i < FunctionCodeComboBox.SelectedItems.Count(); i++)
            //    {
            //        selectcode[index] = FunctionCodeComboBox.SelectedItems[i].ToString();
            //        index++;
            //    }
            //    Array.Sort(selectcode);
            //    selectcodetext += selectcode[0];
            //    for (int j = 1; j < FunctionCodeComboBox.SelectedItems.Count(); j++)
            //    {
            //        selectcodetext += ";" + selectcode[j];
            //    }
            //    FunctionCodeComboBox.Text = selectcodetext;
            //}
            CodeNum.Text = FunctionCodeComboBox.SelectedItems.Count().ToString();
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
                    textBox.Select(offset,0);
                }
            }
            if(textBox.Name == "MinDatatextBox" || textBox.Name == "MaxDatatextBox")
            {
                if (textBox.Text != "")
                {
                    if (Convert.ToInt16(textBox.Text) >= 0)
                    {
                        textBox.Text = Convert.ToString(Convert.ToInt16(textBox.Text));
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    if (Convert.ToInt16(textBox.Text) > 1500)
                    {
                        textBox.Text = Convert.ToString(1500);
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }
            }
            else
            {
                if (textBox.Name != "CoilAddressStarttextBox" && textBox.Name != "CoilAddressEndtextBox")
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
                else
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
            }
              
            CheckCode();
        }

        private void CheckCode()
        {
            if (CoilAddressStarttextBox.Text != "" && CoilAddressEndtextBox.Text != "")
            {
                if (Convert.ToInt32(CoilAddressStarttextBox.Text) > Convert.ToInt32(CoilAddressEndtextBox.Text))
                {
                    WarningLabel.Visibility = Visibility.Visible;
                }
                else WarningLabel.Visibility = Visibility.Collapsed;
            }
            else WarningLabel.Visibility = Visibility.Collapsed;
        }

        //只能输入数字
        private void keydown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if (txt == SourceIPtextBox_1 || txt == DestinationIPtextBox_1)
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

        private void SourceIPCheck_Checked(object sender, RoutedEventArgs e)
        {
            SourceIPtextBox_1.Text = "";
            SourceIPtextBox_2.Text = "";
            SourceIPtextBox_3.Text = "";
            SourceIPtextBox_4.Text = "";
            SourceIPtextBox_1.IsReadOnly = true;
            SourceIPtextBox_2.IsReadOnly = true;
            SourceIPtextBox_3.IsReadOnly = true;
            SourceIPtextBox_4.IsReadOnly = true;
        }

        private void DestinationIPCheck_Checked(object sender, RoutedEventArgs e)
        {
            DestinationIPtextBox_1.Text = "";
            DestinationIPtextBox_2.Text = "";
            DestinationIPtextBox_3.Text = "";
            DestinationIPtextBox_4.Text = "";
            DestinationIPtextBox_1.IsReadOnly = true;
            DestinationIPtextBox_2.IsReadOnly = true;
            DestinationIPtextBox_3.IsReadOnly = true;
            DestinationIPtextBox_4.IsReadOnly = true;
        }

        private void SourceIPCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            SourceIPtextBox_1.IsReadOnly = false;
            SourceIPtextBox_2.IsReadOnly = false;
            SourceIPtextBox_3.IsReadOnly = false;
            SourceIPtextBox_4.IsReadOnly = false;
        }

        private void DestinationIPCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            DestinationIPtextBox_1.IsReadOnly = false;
            DestinationIPtextBox_2.IsReadOnly = false;
            DestinationIPtextBox_3.IsReadOnly = false;
            DestinationIPtextBox_4.IsReadOnly = false;
        }
    }
}
