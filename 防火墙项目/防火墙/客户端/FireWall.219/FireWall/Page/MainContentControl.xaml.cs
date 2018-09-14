using DragDrop;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace FireWall
{
    /// <summary>
    /// MainContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainContent : UserControl
    {
        TreeViewItem currentselectedItem = new TreeViewItem();
        DBOperation db = new DBOperation();
        DateTime mStartHoverTime = DateTime.MinValue;
        TreeViewItem mHoveredItem = null;
        AdornerLayer mAdornerLayer = null;
        string scanstarttext = "";
        string scanendtext = "";
        Thread ScanThread;

        public MainContent()
        {
            InitializeComponent();
            //ScanlistBox.ItemContainerStyle = this.FindResource("SimpleListBoxItemFireWall") as Style;
        }

        //扫描按键事件
        private void Scanbutton_Click(object sender, RoutedEventArgs e)
        {
            scanstarttext = ScanIPStarttextBox_1.Text + "." + ScanIPStarttextBox_2.Text + "." + ScanIPStarttextBox_3.Text + "." + ScanIPStarttextBox_4.Text;
            scanendtext = ScanIPEndtextBox_1.Text + "." + ScanIPEndtextBox_2.Text + "." + ScanIPEndtextBox_3.Text + "." + ScanIPEndtextBox_4.Text;
            IPAddress ip;
            if (IPAddress.TryParse(scanstarttext, out ip) && IPAddress.TryParse(scanendtext, out ip))
            {
                if ((Convert.ToInt16(ScanIPStarttextBox_1.Text) == Convert.ToInt16(ScanIPEndtextBox_1.Text)) && (Convert.ToInt16(ScanIPStarttextBox_2.Text) == Convert.ToInt16(ScanIPEndtextBox_2.Text)) && (Convert.ToInt16(ScanIPStarttextBox_3.Text) == Convert.ToInt16(ScanIPEndtextBox_3.Text)) && (Convert.ToInt16(ScanIPStarttextBox_4.Text) <= Convert.ToInt16(ScanIPEndtextBox_4.Text)))
                {
                    //MainWindow MW = (MainWindow)Application.Current.Windows[0];
                    //MW.MainBox.Background = new SolidColorBrush(Colors.Gray);
                    //扫描线程开启
                    ScanThread = new Thread(new ThreadStart(Scaning));
                    ScanThread.IsBackground = true;
                    ScanThread.Start();
                    //MW.MainBox.Background = new SolidColorBrush(Color.FromArgb(0xFF,0x2D,0x2D,0x2D));
                }
                else UserMessageBox.Show("提示", "请输入正确的范围！");
            }
            else UserMessageBox.Show("提示", "请输入正确的IP！");
        }

        //扫描线程
        private void Scaning()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Scanbutton.IsEnabled = false;
                LayoutRoot.Visibility = Visibility.Visible;
                ScanlistBox.Items.Clear();
                //treeView.Items.Clear();
            }));
            StaticGlobal.FireWalldevices.Clear();
            StaticGlobal.FwMACandIP.Clear();
            StaticGlobal.fwdev_list.Clear();
            string inserttext = "";
            IDevicesCheck devConfirm = new DevicesCheck();
            StaticGlobal.fwdev_list = devConfirm.CheckDevices(scanstarttext, scanendtext);
            string propertySql = "";

            for (int i = 0; i < StaticGlobal.fwdev_list.Count(); i++)
            {
                for (int j = 0; j < StaticGlobal.fwdev_list[i].getProtecDev_list().Count(); j++)
                {
                    string fw_ip = StaticGlobal.fwdev_list[i].getDev_IP();
                    string fw_mac = StaticGlobal.fwdev_list[i].getDev_MAC();
                    string dev_ip = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_IP();
                    string dev_mac = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_MAC();
                    string dev_type = StaticGlobal.fwdev_list[i].getProtecDev_list()[j].getDev_type();
                    inserttext += "INSERT INTO firewallip VALUES ('" + fw_ip + "','" + fw_mac + "','" + dev_ip + "','" + dev_mac + "','" + dev_type + "');";
                    propertySql += "INSERT INTO fwproperty values('"+ fw_mac +"','" + fw_mac + "','" + fw_ip + "',NULL);";
                }
            }
            string propertySql1 = "truncate table fwproperty;" + propertySql;
            db.dboperate(propertySql1);
            MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
            conn.Open();
            string sqltext = "truncate table firewallip;" + inserttext + "select fw_ip,fw_mac,dev_ip,dev_mac,dev_type from firewallip order by 1;";
            MySqlCommand cm = new MySqlCommand(sqltext, conn);
            MySqlDataReader dr = cm.ExecuteReader();
            List<string> firewallmac = new List<string>();
            int index = 0;
            //绑定
            while (dr.Read())
            {
                if (!firewallmac.Contains(dr[1]))
                {
                    firewallmac.Add(dr[1].ToString());
                    StaticGlobal.FwMACandIP.Add(dr[1].ToString(), dr[0].ToString());

                    Dispatcher.Invoke(new Action(() =>
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = "防火墙  MAC: " + dr[1];
                        item.Style = this.FindResource("SimpleListBoxItemFireWall") as Style;
                        ScanlistBox.Items.Add(item);

                        ListBoxItem item2 = new ListBoxItem();
                        item2.Content = dr[4] + "  IP: " + dr[2];
                        item2.Style = this.FindResource("SimpleListBoxItemPLC") as Style;
                        ScanlistBox.Items.Add(item2);
                    }));
                    FireWallDevices firewalldevices = new FireWallDevices(index, dr[0].ToString(), dr[1].ToString());
                    StaticGlobal.FireWalldevices.Add(firewalldevices);
                    index++;
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Content = dr[4] + "  IP: " + dr[2];
                        item.Style = this.FindResource("SimpleListBoxItemPLC") as Style;
                        ScanlistBox.Items.Add(item);
                    }));
                }
            }
            index = 0;
            dr.Close();
            conn.Close();
            Thread.Sleep(500);
            Dispatcher.Invoke(new Action(() =>
            {
                Scanbutton.IsEnabled = true;
                LayoutRoot.Visibility = Visibility.Collapsed;
            }));
            ScanThread.Abort();
        }

        private void listBoxPreviewMouseMove(object sender, MouseEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            Point pos = e.GetPosition(listBox);
            HitTestResult result = VisualTreeHelper.HitTest(listBox, pos);
            if (result == null)
                return;

            ListBoxItem listBoxItem = Utils.FindVisualParent<ListBoxItem>(result.VisualHit); // Find your actual visual you want to drag
                                                                                             //   if (listBoxItem == null || listBoxItem.Content != listBox.SelectedItem || !(listBox.SelectedItem is DataItem))
            if (listBoxItem == null)
                //if (listBoxItem == null || listBoxItem.Content != listBox.SelectedItem)
                return;

            DragDropAdorner adorner = new DragDropAdorner(listBoxItem);
            mAdornerLayer = AdornerLayer.GetAdornerLayer(DragableGrid); // Window class do not have AdornerLayer
            mAdornerLayer.Add(adorner);

            //DataItem dataItem = listBoxItem.Content as DataItem;
            DataObject dataObject = new DataObject(listBoxItem);
            // Here, we should notice that dragsource param will specify on which 
            // control the drag&drop event will be fired
            System.Windows.DragDrop.DoDragDrop(listBox, dataObject, DragDropEffects.Copy);

            mStartHoverTime = DateTime.MinValue;
            mHoveredItem = null;
            mAdornerLayer.Remove(adorner);
            mAdornerLayer = null;
        }

        private void listBoxQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            mAdornerLayer.Update();
            UpdateTreeViewExpandingState();
        }

        private void UpdateTreeViewExpandingState()
        {
            Win32.POINT point = new Win32.POINT();
            if (Win32.GetCursorPos(ref point))
            {
                Point pos = new Point(point.X, point.Y);
                pos = treeView.PointFromScreen(pos);
                HitTestResult result = VisualTreeHelper.HitTest(treeView, pos);
                if (result != null)
                {
                    TreeViewItem selectedItem = Utils.FindVisualParent<TreeViewItem>(result.VisualHit);
                    if (selectedItem != null)
                    {
                        if (mHoveredItem != selectedItem)
                        {
                            mHoveredItem = selectedItem;
                            mStartHoverTime = DateTime.Now;
                        }
                        else
                        {
                            if (mHoveredItem.Items.Count > 0 && !mHoveredItem.IsExpanded &&
                                DateTime.Now - mStartHoverTime > TimeSpan.FromSeconds(2))
                                mHoveredItem.IsExpanded = true;
                        }
                    }
                }
            }
        }

        private void treeViewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            Point pos = e.GetPosition(treeView);
            HitTestResult result = VisualTreeHelper.HitTest(treeView, pos);
            if (result == null)
                return;

            TreeViewItem selectedItem = Utils.FindVisualParent<TreeViewItem>(result.VisualHit);
            if (selectedItem != null)
                selectedItem.IsSelected = true;

            e.Effects = DragDropEffects.Copy;
           

        }

        private void treeViewDrop(object sender, DragEventArgs e)
        {
            Point pos = e.GetPosition(treeView);
            HitTestResult result = VisualTreeHelper.HitTest(treeView, pos);
            if (result == null)
                return;

            ListBoxItem newChild = new ListBoxItem();
            newChild = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            TreeViewItem addChild = new TreeViewItem();

            TreeViewItem selectedItem = Utils.FindVisualParent<TreeViewItem>(result.VisualHit);
            if (selectedItem == null)
            {
                treeView.Items.Add(addChild);
                if (newChild.Content.ToString().Contains("防火墙"))
                {
                    addChild.Header = newChild.Content.ToString();
                    addChild.Style = this.FindResource("SimpleTreeViewItemFireWall") as Style;
                }
                else if (newChild.Content.ToString().Contains("电脑"))
                {
                    addChild.Header = "电脑";
                    addChild.Style = this.FindResource("SimpleTreeViewItemComputer") as Style;
                }
                else if (newChild.Content.ToString().Contains("PLC"))
                {
                    RenameWindow Rename = new RenameWindow();
                    Rename.ShowDialog();
                    addChild.Header = StaticGlobal.newPLC;
                    addChild.Style = this.FindResource("SimpleTreeViewItemPLC") as Style;
                }
                else
                {
                    addChild.Header = newChild.Content.ToString();
                    addChild.Style = this.FindResource("SimpleTreeViewItemPLC") as Style;
                }
            }
            else
            {
                TreeViewItem parent = selectedItem as TreeViewItem;
                parent.Items.Add(addChild);
                if (newChild.Content.ToString().Contains("防火墙"))
                {
                    addChild.Header = newChild.Content.ToString();
                    addChild.Style = this.FindResource("SimpleTreeViewItemFireWall") as Style;
                }
                else if (newChild.Content.ToString().Contains("电脑"))
                {
                    addChild.Header = "电脑";
                    addChild.Style = this.FindResource("SimpleTreeViewItemComputer") as Style;
                }
                else if (newChild.Content.ToString().Contains("PLC"))
                {
                    RenameWindow Rename = new RenameWindow();
                    Rename.ShowDialog();
                    addChild.Header = StaticGlobal.newPLC;
                    addChild.Style = this.FindResource("SimpleTreeViewItemPLC") as Style;
                }
                else
                {
                    addChild.Header = newChild.Content.ToString();
                    addChild.Style = this.FindResource("SimpleTreeViewItemPLC") as Style;
                }
                parent.IsExpanded = true;
            }
        }

        private void treeViewPreviewMouseRightDown(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (selectedItem != null)
            {
                selectedItem.Focus();
                e.Handled = true;
                currentselectedItem = selectedItem;
                //右键菜单显示项目设置
                if (selectedItem.Header.ToString().Contains("防火墙"))
                {
                    //Configure.Visibility = Visibility.Visible;
                    NoIPConfigure.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    //Configure.Visibility = Visibility.Collapsed;
                    NoIPConfigure.Visibility = Visibility.Collapsed;
                    Delete.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //Configure.Visibility = Visibility.Collapsed;
                NoIPConfigure.Visibility = Visibility.Collapsed;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        private DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);
            return source;
        }
        //双击事件
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
           bool add_flag =true;
            var selectedItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (selectedItem != null && selectedItem.Header.ToString().Contains("防火墙"))
            {
                e.Handled = true;
                currentselectedItem = selectedItem;
                StaticGlobal.firewallmac = currentselectedItem.Header.ToString().Replace("防火墙  MAC: ", "");                           
                TabItem FWMAC = new TabItem();
                foreach (TabItem item in FirsttabControl.Items)
                {
                    if (FirsttabControl.Items.Count > 1)
                    {
                        if (item.Header.ToString() != "网络拓扑")
                            item.Visibility = Visibility.Collapsed;
                    }
                    if (item.Header.ToString() == StaticGlobal.firewallmac)
                    {
                        add_flag = false;
                        if (item.Visibility == Visibility.Collapsed)
                        {
                            item.Visibility = Visibility.Visible;
                        }
                    }
                }
                    if (add_flag)
                    {
                        FWMAC.Header = StaticGlobal.firewallmac;
                        ConfigFW fw = new ConfigFW();
                        FWMAC.Content = fw;
                        FirsttabControl.Items.Add(FWMAC);
                        //FirsttabControl.SelectedItem = FWMAC;
                        DB_DataGridbinding fwbind = new DB_DataGridbinding();
                        fwbind.SNATDB_Gridbinding(); fwbind.DNATDB_Gridbinding();
                        fwbind.WHLDB_Gridbinding(); fwbind.PropertyBind();
                        fwbind.DPIDB_Gridbinding();
                        fwbind.APCDB_Gridbinding();
                        fwbind.CNCDB_Gridbinding();
                        fwbind.PRTDB_Gridbinding();
                        fwbind.STDDB_Gridbinding();
                        selectedItem.Focus();
                    }                                 
            }

        }
        private void Configure_Click(object sender, RoutedEventArgs e)
        {
            StaticGlobal.firewallmac = currentselectedItem.Header.ToString().Replace("防火墙  MAC: ", "");
            FireWallConfigurationApply fwca = new FireWallConfigurationApply();
            fwca.MAClabel.Content = "MAC: " + StaticGlobal.firewallmac;
            fwca.IPlabel.Content = "IP: " + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac];
            DB_DataGridbinding fw = new DB_DataGridbinding();
            fw.DPIDB_Gridbinding();
            fwca.ShowDialog();
        }

        private void NoIPConfigure_Click(object sender, RoutedEventArgs e)
        {
            if (UserMessageBox.Show("无IP配置", "确定要将此防火墙配置成无IP吗？") == true)
            {
                INoIPConfig noip = new NoIPConfig();
                FWDeviceForm fwdev = (from devices in StaticGlobal.fwdev_list
                                      where devices.getDev_MAC() == currentselectedItem.Header.ToString().Replace("防火墙  MAC: ", "")
                                      select devices).ToList<FWDeviceForm>()[0];
                if (noip.NoipConfig(fwdev))
                {
                    UserMessageBox.Show("提示", "无IP配置成功！");
                }
                else
                {
                    UserMessageBox.Show("提示", "无IP配置失败！");
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = currentselectedItem as TreeViewItem;
            DependencyObject target = VisualTreeHelper.GetParent(item);
            while (target != null)
            {
                if (target is TreeViewItem)
                {
                    break;
                }
                target = VisualTreeHelper.GetParent(target);
            }
            TreeViewItem parent = target as TreeViewItem;
            if (parent != null)
            {
                parent.Items.Remove(item);
            }
            else
            {
                treeView.Items.Remove(item);
            }
            if (item != null && item.Header.ToString().Contains("防火墙"))
            {
                foreach (TabItem tabitem in FirsttabControl.Items)
                {
                    if (tabitem.Header.ToString() == item.Header.ToString().Replace("防火墙  MAC: ", ""))
                    {
                        
                        tabitem.Visibility = Visibility.Collapsed;                 
                    }
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (UserMessageBox.Show("清空", "确定要清空所有的拓扑结构吗？") == true)
            {
                treeView.Items.Clear();
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


        //显示规则grid控件    

        private void FirsttabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        internal static class Utils
        {
            public static T FindVisualParent<T>(DependencyObject obj) where T : class
            {
                while (obj != null)
                {
                    if (obj is T)
                        return obj as T;
                    obj = VisualTreeHelper.GetParent(obj);
                }

                return null;
            }
        }


    }
}
