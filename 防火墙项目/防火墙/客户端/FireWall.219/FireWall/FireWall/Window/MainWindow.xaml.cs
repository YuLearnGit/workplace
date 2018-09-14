using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FireWall
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainContent MCC = new MainContent();
        public LogControl LC = new LogControl();
        public RealTimeData RTD = new RealTimeData();
        public StatisticsControl SC = new StatisticsControl();
        public UserManagementControl UMC = new UserManagementControl();
        public SystemSettingControl SSC = new SystemSettingControl();
        public MainWindow()
        {
            InitializeComponent();
            GlobalXmlConfig.ReadXmlConfig();
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //显示时间
            GetDateAndTime();
            ContentBox.Children.Add(MCC);
            ContentBox.Children.Add(LC);
            ContentBox.Children.Add(RTD);
            ContentBox.Children.Add(SC);
            ContentBox.Children.Add(UMC);
            ContentBox.Children.Add(SSC);

            MCC.Visibility = Visibility.Visible;
            LC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Collapsed;
        }
        void GetDateAndTime()
        {
            DispatcherTimer ShowTime = new DispatcherTimer();
            ShowTime.Tick += new EventHandler(ShowCurrentTime);//一直获取当前时间
            ShowTime.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTime.Start();

        }

        //获取时间函数
        public void ShowCurrentTime(object sender, EventArgs e)
        {
            //获得年月日
            Date.Text = DateTime.Now.ToString("yyyy年MM月dd日");   //yyyy年MM月dd日
            //Date.Text += " ";
            //获得星期几
            //Date.Text += DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));         
            //获得时分秒
            Time.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        //关闭按钮
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        // 最大化按钮
        private void MaximizedButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizedButton.SetResourceReference(BackgroundProperty, "MaximizeButtonBrush");
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizedButton.SetResourceReference(BackgroundProperty, "RestoreButtonBrush");
            }
        }

        //最小化按钮
        private void MinimizedButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //主页按钮
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MCC.Visibility = Visibility.Visible;
            RTD.Visibility = Visibility.Collapsed;
            LC.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Collapsed;

            homeimage.Visibility = Visibility.Visible;
            realtimedataimage.Visibility = Visibility.Collapsed;
            logimage.Visibility = Visibility.Collapsed;
            statisticsimage.Visibility = Visibility.Collapsed;
            usermanagementimage.Visibility = Visibility.Collapsed;
            systemsettingimage.Visibility = Visibility.Collapsed;
        }

        //实时数据按钮
        private void RealTimeDataButton_Click(object sender, RoutedEventArgs e)
        {
                     
            MCC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Visible;
            LC.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Collapsed;

            homeimage.Visibility = Visibility.Collapsed;
            realtimedataimage.Visibility = Visibility.Visible;
            logimage.Visibility = Visibility.Collapsed;
            statisticsimage.Visibility = Visibility.Collapsed;
            usermanagementimage.Visibility = Visibility.Collapsed;
            systemsettingimage.Visibility = Visibility.Collapsed;

            RTD.HostNameBinding();
            //RTD.DataRandomThread.Start();
        }

        //日志按钮
        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            MCC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Collapsed;
            LC.Visibility = Visibility.Visible;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Collapsed;

            homeimage.Visibility = Visibility.Collapsed;
            realtimedataimage.Visibility = Visibility.Collapsed;
            logimage.Visibility = Visibility.Visible;
            statisticsimage.Visibility = Visibility.Collapsed;
            usermanagementimage.Visibility = Visibility.Collapsed;
            systemsettingimage.Visibility = Visibility.Collapsed;

            LC.SelectionBinding();
            LC.LogData.ItemsSource = null;
            LC.InitGridControl();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            MCC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Collapsed;
            LC.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Visible;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Collapsed;

            homeimage.Visibility = Visibility.Collapsed;
            realtimedataimage.Visibility = Visibility.Collapsed;
            logimage.Visibility = Visibility.Collapsed;
            statisticsimage.Visibility = Visibility.Visible;
            usermanagementimage.Visibility = Visibility.Collapsed;
            systemsettingimage.Visibility = Visibility.Collapsed;

            SC.SelectionBinding();
            SC.IPinitGridControl();
            SC.IPchartControl.Visibility = Visibility.Collapsed;
            SC.DatechartControl.Visibility = Visibility.Visible;
            SC.Returnbutton.Visibility = Visibility.Collapsed;
            SC.ShowBar.Visibility = Visibility.Collapsed;
            SC.ShowLine.Visibility = Visibility.Collapsed;
            SC.EnabledDateQueryFlag = false;
            SC.StatisData.ItemsSource = null;
            SC.barSeries.DataSource = null;
            SC.line.DataSource = null;
            SC.pie.Points.Clear();
        }

        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MCC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Collapsed;
            LC.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Visible;
            SSC.Visibility = Visibility.Collapsed;

            homeimage.Visibility = Visibility.Collapsed;
            realtimedataimage.Visibility = Visibility.Collapsed;
            logimage.Visibility = Visibility.Collapsed;
            statisticsimage.Visibility = Visibility.Collapsed;
            usermanagementimage.Visibility = Visibility.Visible;
            systemsettingimage.Visibility = Visibility.Collapsed;

            UMC.InitDateGrid();
        }

        private void SystemSettingButton_Click(object sender, RoutedEventArgs e)
        {
            MCC.Visibility = Visibility.Collapsed;
            RTD.Visibility = Visibility.Collapsed;
            LC.Visibility = Visibility.Collapsed;
            SC.Visibility = Visibility.Collapsed;
            UMC.Visibility = Visibility.Collapsed;
            SSC.Visibility = Visibility.Visible;

            homeimage.Visibility = Visibility.Collapsed;
            realtimedataimage.Visibility = Visibility.Collapsed;
            logimage.Visibility = Visibility.Collapsed;
            statisticsimage.Visibility = Visibility.Collapsed;
            usermanagementimage.Visibility = Visibility.Collapsed;
            systemsettingimage.Visibility = Visibility.Visible;

            SSC.DataBaseBox.Text = StaticGlobal.database;
            SSC.DataSourceBox.Text = StaticGlobal.datasource;
            SSC.UserIDBox.Text = StaticGlobal.userid;
            SSC.PassWordBox.Text = StaticGlobal.password;
            SSC.SaveXmlButton.IsEnabled = false;
            SSC.ResetXmlButton.IsEnabled = false;
        }
        /*----------------------------------------------------------------
        //函数说明：拖动窗口//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //WindowState = WindowState.Normal;                
                DragMove();
                //WindowState = WindowState.Maximized;
            }
            catch
            {

            }   
        }

        /*----------------------------------------------------------------
        //函数说明：鼠标左键双击界面事件//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        int i = 0;
        private void MouseLeftClick(object sender, MouseEventArgs e)
        {
            i += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;
            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    MaximizedButton.SetResourceReference(BackgroundProperty, "MaximizeButtonBrush");
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    MaximizedButton.SetResourceReference(BackgroundProperty, "RestoreButtonBrush");
                }
            }
        }
    }
}
