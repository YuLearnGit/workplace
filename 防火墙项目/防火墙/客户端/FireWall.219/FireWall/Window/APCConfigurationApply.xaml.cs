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
    /// APCConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class APCConfigurationApply : Window
    {
        DB_DataGridbinding APC = new DB_DataGridbinding();
        public APCConfigurationApply()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //APC.APCDB_Gridbinding();
            APCdataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getAPCRule_list();
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

        private void APCeditclick(object sender, MouseButtonEventArgs e)
        {
            DB_DataGridbinding APCbind = new DB_DataGridbinding();
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            string proto = (APCdataGrid.SelectedItem as APCRuleDataTable).protocol.ToString();
            string status = (APCdataGrid.SelectedItem as APCRuleDataTable).status.ToString();
            IAPCRulesManage APC = new APCRulesManage();
            if (status == "allow")
            {
                if (APC.ApplicationProtocolControl(dev_ip, proto, false))
                {
                    UserMessageBox.Show("提示", "状态修改成功！");
                    APCbind.APCDB_Gridbinding();
                }
                else UserMessageBox.Show("提示", "状态修改失败，请检查设备连接！");
            }
            if (status == "forbid")
            {
                if (APC.ApplicationProtocolControl(dev_ip, proto, true))
                {
                    UserMessageBox.Show("提示", "状态修改成功！");
                    APCbind.APCDB_Gridbinding();
                }
                else UserMessageBox.Show("提示", "状态修改失败，请检查设备连接！");
            }
            
        }

        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
