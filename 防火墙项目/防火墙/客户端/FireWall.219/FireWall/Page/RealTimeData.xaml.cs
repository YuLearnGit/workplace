using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// RealTimeData.xaml 的交互逻辑
    /// </summary>
    public partial class RealTimeData : UserControl
    {   
        //实时数据线程
        public Thread DataRandomThread;
        string hostname = null;
        string Statetext = null;
        //DataTable dt = new DataTable();       
        public class Log
        {
            public string time { get; set; }
            public string hostname { get; set; }
            public string SRC { get; set; }
            public string DST { get; set; }
            public string ID { get; set; }
            public string PROTO { get; set; }
            public string SPT { get; set; }
            public string DPT { get; set; }
            public string handle_result { get; set; }
            public Log(string a, string b, string c, string d, string e, string f, string g, string h, string i)
            {
                time = a;
                hostname = b;
                SRC = c;
                DST = d;
                ID = e;
                PROTO = f;
                SPT = g;
                DPT = h;
                handle_result = i;
            
            }
        }
        public ObservableCollection<Log> realtimedata = new ObservableCollection<Log>();

        public RealTimeData()
        {
            InitializeComponent();
            RealtimeData.ItemsSource = realtimedata;
        }

        /*----------------------------------------------------------------
        //函数说明：加载窗口//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;
            StateBinding();
                //刷新数据线程
                DataRandomThread = new Thread(new ThreadStart(GetRealData));
                DataRandomThread.IsBackground = true;
                DataRandomThread.Start();


            //dt.Columns.Add("主机名称",typeof(string));
            //dt.Rows.Add(DateTime.Now.ToShortTimeString());
        }

        private void GetRealData()
        {
            ReceiveLog receivelog = new ReceiveLog();             
            receivelog.Save_DisplayLog(true);  
            while (true)
            {               
                string log = receivelog.queue_func();
                string[] reallog = log.Split(' ');
                if (realtimedata.Count < 50)
                {
                    realtimedata.Insert(0, new Log(reallog[0],reallog[1],reallog[2], reallog[4], reallog[6], reallog[8], reallog[9], reallog[11], reallog[14]));
                }
                else
                {
                    realtimedata.RemoveAt(49);
                    realtimedata.Insert(0, new Log(reallog[0], reallog[1], reallog[2], reallog[4], reallog[6], reallog[8], reallog[9], reallog[11], reallog[14]));
                }


                //if (realtimedata.Count<50)
                //realtimedata.Insert(0,new Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                //else
                //{
                //    realtimedata.RemoveAt(49);
                //    realtimedata.Insert(0, new Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                //}
                //dt.Rows.Add("aaa");
                try
                {
                    //string mainCommandText = "select time as'时间',host_name as'主机名称',SRC as'源IP地址',DST as'目标IP地址'," +
                    //                                     "LEN as'承载数据的总长度',TOS as'IP包头内的服务类型',PREC as'服务类型的优先级',ID as'IP数据包标示'," +
                    //                                     "PROTO as'传输层协议类型',SPT as'源端口号', DPT as'目标端口号',handle_result as '处理状态' from iptables_log " +
                    //                                     "where host_name ='" + hostname + "'" + Statetext + "AND time > '" + StaticGlobal.LoginTime + "' order by time DESC limit 100; ";
                    //DataSet result = new DataSet();
                    //MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                    //conn.Open();
                    //MySqlDataAdapter s = new MySqlDataAdapter(mainCommandText, conn);
                    //s.Fill(result);
                    //conn.Close();
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //RealtimeData.ItemsSource = result.Tables[0];
                        //if (RealtimeData.SelectedItem != null)
                        //{
                        //    RealtimeData.View.ScrollIntoView(RealtimeData.View.FocusedRowHandle);
                        //}

                    }));
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
            }
        }

        public void HostNameBinding()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = DateTime.Now;
            //绑定
            try
            {
                string name = null;
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                string sqltext = "select DISTINCT host_name from iptables_log"+ currentTime.Year.ToString()+currentTime.Month.ToString().PadLeft(2,'0');
                MySqlCommand cmd = new MySqlCommand(sqltext, conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (name == null)
                    {
                        name += dr[0].ToString();
                    }
                    else
                    {
                        name += "," + dr[0].ToString();
                    }
                }
                dr.Close();
                conn.Close();
                HostName.ItemsSource = name.Split(',');
                HostName.SelectedIndex = 0;
            }
            catch
            {
                HostName.ItemsSource = null;
            }
        }
        public void StateBinding()
        {
            string[] state = { "ALL", "ACCEPT", "DROP" };
            StateName.ItemsSource = state;
            StateName.SelectedIndex = 0;
        }
        private void namechange(object sender, RoutedEventArgs e)
        {
            hostname = HostName.SelectedItem.ToString();
        }

        private void statechange(object sender, RoutedEventArgs e)
        {
            if (StateName.SelectedItem.ToString() == "ALL")
            {
                Statetext = "";
            }
            else
            {
                Statetext = "AND handle_result = '" + StateName.SelectedItem.ToString() + "'";
            }
        }
    }
}