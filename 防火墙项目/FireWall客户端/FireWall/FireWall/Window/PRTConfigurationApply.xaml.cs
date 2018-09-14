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
using System.Threading;
namespace FireWall
{
    /// <summary>
    /// PRTConfigurationApply.xaml 的交互逻辑
    /// </summary>
    public partial class PRTConfigurationApply : Window
    {
        Thread PRTApplyThread;
        public PRTConfigurationApply()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PRTdataGrid.ItemsSource = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list();
            PRTApplyEnabled();
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

        private void NewPRT_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.editflag = false;
            this.Close();
            PRTConfigurationDetail fw = new PRTConfigurationDetail();
            fw.ShowDialog();
        }

        private void PRTApplybutton_Click(object sender, RoutedEventArgs e)
        {
            PRTApplyThread = new Thread(new ThreadStart(PRTApplying));
            PRTApplyThread.IsBackground = true;
            PRTApplyThread.Start();
        }
        public void PRTApplyEnabled()
        {
            if (StaticGlobal.PRToldrules.Count() == StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Count())
            {
                int total = 0;
                for (int i = 0; i < StaticGlobal.PRToldrules.Count; i++)
                {
                    string route_type = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].route_type;
                    string host_IP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].ToString();
                    string dstIP = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].ToString();
                    string netmask = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].netmask;
                    string ETH = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].ETH;
                    string Gateway = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].Gateway;
                    bool log = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list()[i].log;
                    int num = (from rules in StaticGlobal.PRToldrules
                               where rules.route_type == route_type && rules.host_IP == host_IP && rules.dstIP == dstIP
                               && rules.netmask == netmask && rules.ETH == ETH && rules.Gateway == Gateway && rules.log == log
                               select rules.route_type).ToList<string>().Count;
                    total += num;
                }
                if (total == StaticGlobal.PRToldrules.Count)
                    PRTApplybutton.Visibility = Visibility.Collapsed;           
                else PRTApplybutton.Visibility = Visibility.Visible;
            }
            else PRTApplybutton.Visibility = Visibility.Visible;
        }   

        public void PRTApplying()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                NewPRT.IsEnabled = false;
                PRTApplybutton.IsEnabled = false;
                PRTdataGrid.IsEnabled = false;
                Closebutton.IsEnabled = false;
            }));
            //找出需要删除的规则
            var deleteRules = StaticGlobal.PRToldrules.Where(deleteRule => !StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Contains(deleteRule)).ToList();

            //找出需要增加的规则
            var addRules = StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Where(addRule => !StaticGlobal.PRToldrules.Contains(addRule)).ToList();
            bool PRTApplyFlag = true;
            string dev_ip = (from devices in StaticGlobal.fwdev_list
                             where devices.getDev_MAC() == StaticGlobal.firewallmac
                             select devices).ToList<FWDeviceForm>()[0].getProtecDevIP_list()[0];
            IPRTRulesManage prtrule = new PRTRulesManage();
            //增加规则
            for (int i = 0; i < addRules.Count(); i++)
            {
                string route_type = addRules[i].route_type;              
                string ETH = addRules[i].ETH;
                string Gateway = addRules[i].Gateway;
                bool log = addRules[i].log;
                switch (route_type)
                {
                    case "默认路由":
                        if (prtrule.DefaultRouteConfig(dev_ip, true, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                    case "主机路由":
                        string host_IP = addRules[i].host_IP.ToString();
                        if (prtrule.HostRouteConfig(dev_ip, true, host_IP, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                    case "网络路由":
                        string dstIP = addRules[i].dstIP.ToString();
                        string netmask = addRules[i].netmask;
                        if (prtrule.NetRouteConfig(dev_ip, true, dstIP, netmask, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                }
            }
            //删除规则
            for (int j = 0; j < deleteRules.Count(); j++)
            {
                string route_type = deleteRules[j].route_type;                       
                string ETH = deleteRules[j].ETH;
                string Gateway = deleteRules[j].Gateway;
                bool log = deleteRules[j].log;
                switch (route_type)
                {
                    case "默认路由":
                        if (prtrule.DefaultRouteConfig(dev_ip, false, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                    case "主机路由":
                        string host_IP = deleteRules[j].host_IP.ToString();
                        if (prtrule.HostRouteConfig(dev_ip, false, host_IP, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                    case "网络路由":
                        string dstIP = deleteRules[j].dstIP.ToString();
                        string netmask = deleteRules[j].netmask;
                        if (prtrule.NetRouteConfig(dev_ip, false, dstIP, netmask, ETH, Gateway))
                            PRTApplyFlag = true;
                        else PRTApplyFlag = false;
                        break;
                }
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (PRTApplyFlag)
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
            PRTApplyThread.Abort();
        }
        private void PRTeditclick(object sender, MouseButtonEventArgs e)
        {
            string[] gate = (PRTdataGrid.SelectedItem as PRTRuleDataTable).Gateway.ToString().Split('.');
            PRTConfigurationDetail fw = new PRTConfigurationDetail();
            switch ((PRTdataGrid.SelectedItem as PRTRuleDataTable).route_type.ToString())
            {
                case "默认路由":
                    fw.NetInterface.Visibility = Visibility.Visible;
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

                    if (host[0] != "")
                    {
                        fw.destIPStarttextBox_1.Text = host[0];
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
                    if (mask[0] != "")
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
            fw.ETHComboBox.Text = (PRTdataGrid.SelectedItem as PRTRuleDataTable).ETH.ToString();
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
            string ETH=(PRTdataGrid.SelectedItem as PRTRuleDataTable).ETH.ToString();
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
                    if (PRTrule.NetRouteConfig(devIP, false, dstIP,netmask, ETH,Gateway))
                    {
                        StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Remove((PRTdataGrid.SelectedItem as PRTRuleDataTable));
                        UserMessageBox.Show("提示", "规则删除成功！");
                    }
                    else UserMessageBox.Show("提示", "规则删除失败，请检查设备连接！");
                    break;
            }

        }

        private void Closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
