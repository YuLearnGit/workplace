using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace FireWall
{
    /// <summary>
    /// FireWallConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class FireWallConfigurationApply : Window
    {
        Thread ApplyThread;

        public FireWallConfigurationApply()
        {
            InitializeComponent();          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list();
            ApplyEnabled();
        }

        private void ApplyEnabled()
        {
            if (StaticGlobal.oldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Count() )
            {
                int total = 0;
                for (int i = 0; i < StaticGlobal.oldrules.Count; i++)
                {
                    string protocol = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].protocol;
                    string source = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].source;
                    string destination = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].destination;
                    string coiladdressstart = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].coiladdressstart;
                    string coiladdressend = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].coiladdressend;
                    string func = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].func;
                    int mindata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].mindata;
                    int maxdata = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].maxdata;
                    bool log = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list()[i].log;
                    int num = (from rules in StaticGlobal.oldrules
                               where rules.protocol == protocol && rules.source == source && rules.destination == destination && rules.coiladdressstart == coiladdressstart && rules.coiladdressend == coiladdressend && rules.func == func && rules.log == log
                               select rules.protocol).ToList<string>().Count;
                    total += num;
                }
                if (total == StaticGlobal.oldrules.Count)
                    Applybutton.Visibility = Visibility.Collapsed;
                else Applybutton.Visibility = Visibility.Visible;
            }
            else Applybutton.Visibility = Visibility.Visible;
        }

        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void NewRulebutton_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            this.Close();
            FireWallConfigurationDetail fwcd = new FireWallConfigurationDetail();
            fwcd.ShowDialog();
        }

        private void editclick(object sender, MouseButtonEventArgs e)
        {
            string[] source = (dataGrid.SelectedItem as FireWallRuleDataTable).source.ToString().Split('.');
            string[] destination = (dataGrid.SelectedItem as FireWallRuleDataTable).destination.ToString().Split('.');
            FireWallConfigurationDetail fwcd = new FireWallConfigurationDetail();
            fwcd.ProtocolComboBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).protocol.ToString();
            if (fwcd.ProtocolComboBox.Text == "ModbusTCP")
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
            if(destination[0] != "any")
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
           
            fwcd.CoilAddressStarttextBox.Text= (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressstart.ToString();
            fwcd.CoilAddressEndtextBox.Text= (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressend.ToString();
            fwcd.MinDatatextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).mindata.ToString();
            fwcd.MaxDatatextBox.Text = (dataGrid.SelectedItem as FireWallRuleDataTable).maxdata.ToString(); 
            fwcd.FunctionCodeComboBox.Text= (dataGrid.SelectedItem as FireWallRuleDataTable).func.ToString();
            fwcd.logcheckBox.IsChecked = (dataGrid.SelectedItem as FireWallRuleDataTable).log;
            StaticGlobal.selectedindex = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().IndexOf(dataGrid.SelectedItem as FireWallRuleDataTable);
            StaticGlobal.editflag = true;
            this.Close();
            fwcd.ShowDialog();
        }
        //规则删除按钮
        private void deleteclick(object sender, MouseButtonEventArgs e)
        {
            IDPIRulesManage rulesmg = new DPIRulesManage();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string source = (dataGrid.SelectedItem as FireWallRuleDataTable).source.ToString();
            string destination= (dataGrid.SelectedItem as FireWallRuleDataTable).destination.ToString();
            string min_addr = (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressstart.ToString();
            string max_addr= (dataGrid.SelectedItem as FireWallRuleDataTable).coiladdressend.ToString();
            string func= (dataGrid.SelectedItem as FireWallRuleDataTable).func.ToString();
            int min_data = (dataGrid.SelectedItem as FireWallRuleDataTable).mindata;
            int max_data= (dataGrid.SelectedItem as FireWallRuleDataTable).maxdata;
            bool log = (dataGrid.SelectedItem as FireWallRuleDataTable).log;
            if (rulesmg.ChangeModbusTcpRules(destination, source, min_addr, max_addr, func, min_data, max_data, dev_ip, log, false))
            {
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Remove((dataGrid.SelectedItem as FireWallRuleDataTable));
                UserMessageBox.Show("提示", "规则删除成功！");
            }
            else UserMessageBox.Show("提示","规则删除失败，请检查设备连接！");

        }

        //编写规则按钮
        private void Applybutton_Click(object sender, RoutedEventArgs e)
        {
            ApplyThread = new Thread(new ThreadStart(Applying));
            ApplyThread.IsBackground = true;
            ApplyThread.Start();
        }

        private void Applying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewRulebutton.IsEnabled = false;
                Applybutton.IsEnabled = false;
                dataGrid.IsEnabled = false;
                Closebutton.IsEnabled = false;
            }));
            //找出需要删除的规则
            var deleteRules = StaticGlobal.oldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var addRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Where(addRule => !StaticGlobal.oldrules.Contains(addRule)).ToList();
            bool allApplyFlag = true;
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];

            IDPIRulesManage rulesmg = new DPIRulesManage();
            //增加规则
            for (int i = 0; i < addRules.Count(); i++)
            {
                string protocol = addRules[i].protocol;
                string source = addRules[i].source;
                string destination = addRules[i].destination;
                string coiladdressstart = addRules[i].coiladdressstart;
                string coiladdressend = addRules[i].coiladdressend;
                int mindata = addRules[i].mindata;
                int maxdata = addRules[i].maxdata;
                string functioncode = addRules[i].func;
                bool log = addRules[i].log;
                //string[] functioncode16 = fuctioncode.Split(';');
                //string[] functioncode10 = new string[functioncode16.Length];
                //if (fuctioncode == "")
                //{
                //    functioncode10 = null;
                //}
                //else
                //{
                //    for (int j = 0; j < functioncode16.Length; j++)
                //    {
                //        functioncode10[j] = Convert.ToInt16(functioncode16[j], 16).ToString();
                //    }
                //}

                //IRulesManage rulesmg = new RulesManage();
                switch (protocol)
                {
                    case "ModbusTCP":
                        if (rulesmg.ChangeModbusTcpRules(destination, source, coiladdressstart, coiladdressend, functioncode, mindata, maxdata, dev_ip, log, true))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                    case "OPC":
                        if (rulesmg.ChangeOPCRules(destination, source, dev_ip, log, true))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                    case "DNP3":
                        if (rulesmg.ChangeDNP3Rules(destination, source, dev_ip, log, true))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                }
            }
            //删除规则
            for (int i = 0; i < deleteRules.Count(); i++)
            {
                string protocol = deleteRules[i].protocol;
                string source = deleteRules[i].source;
                string destination = deleteRules[i].destination;
                string coiladdressstart = deleteRules[i].coiladdressstart;
                string coiladdressend = deleteRules[i].coiladdressend;
                int mindata = deleteRules[i].mindata;
                int maxdata = deleteRules[i].maxdata;
                string functioncode = deleteRules[i].func;
                bool log = deleteRules[i].log;
                //string[] functioncode16 = fuctioncode.Split(';');
                //string[] functioncode10 = new string[functioncode16.Length];
                //if (fuctioncode == "")
                //{
                //    functioncode10 = null;
                //}
                //else
                //{
                //    for (int j = 0; j < functioncode16.Length; j++)
                //    {
                //        functioncode10[j] = Convert.ToInt16(functioncode16[j], 16).ToString();
                //    }
                //}

                //IRulesManage rulesmg = new RulesManage();
                switch (protocol)
                {
                    case "ModbusTCP":
                        if (rulesmg.ChangeModbusTcpRules(destination, source, coiladdressstart, coiladdressend, functioncode, mindata, maxdata, dev_ip, log, false))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                    case "OPC":
                        if (rulesmg.ChangeOPCRules(destination, source, dev_ip, log, false))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                    case "DNP3":
                        if (rulesmg.ChangeDNP3Rules(destination, source, dev_ip, log, false))
                            allApplyFlag = true;
                        else
                        {
                            allApplyFlag = false;
                        }
                        break;
                }           }
            Dispatcher.Invoke(new Action(() =>
            {
                if(allApplyFlag)
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
            ApplyThread.Abort();
        }
    }
}
