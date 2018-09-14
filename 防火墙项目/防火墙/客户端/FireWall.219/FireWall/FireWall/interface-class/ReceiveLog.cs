using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FireWall
{
    class ReceiveLog : IReceiveLog
    {
        Queue<string> real_time = new Queue<string>();
        private int listenPort = 8000;

        public void Save_DisplayLog(bool start)
        {
            if (start)
            {
                Thread save = new Thread(new ThreadStart(SaveData));
                save.Start();
            }
            
            //real_time.Enqueue("2017-03-2414:27:20 hehe-desktop 172.16.10.205 172.16.10.123 598 TCP 57078 502 DROP");
        }


        /// <summary>
        /// 将日志从消息队列中取出的方法
        /// </summary>
        public string queue_func()
        {
            string Display_log = null;
            while (real_time.Count == 0)
            {
                Thread.Sleep(2000);
               
            }
            Display_log = real_time.Dequeue();
#if debug
            Console.WriteLine(Display_log);
#endif
            return Display_log;
        }

        /// <summary>
        /// 日志存入数据库并将日志消息入队
        /// </summary>
        /// <param name="log">UDP客户端监听到的原始日志</param>
        public void SaveData()
        {
            /* 连接数据库 */
            MySqlConnection con = new MySqlConnection(StaticGlobal.ConnectionString);
            con.Open();
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = null;
            /*建立月份转换字典*/
            Dictionary<string, string> month = new Dictionary<string, string>();
            month.Add("Jan", "01"); month.Add("Feb", "02"); month.Add("Mar", "03"); month.Add("Apr", "04"); month.Add("May", "05"); month.Add("Jun", "06");
            month.Add("Jul", "07"); month.Add("Aug", "08"); month.Add("Sep", "09"); month.Add("Oct", "10"); month.Add("Nov", "11"); month.Add("Dec", "12");
            try
            {
                while (true)
                {
                    /* 监听并接收日志 */
                    byte[] content = listener.Receive(ref groupEP);
                    string log = Encoding.Default.GetString(content);
                    string[] log_array = log.Split(' ');//将日志按空格拆分
                    string mon = null;
                    if (month.ContainsKey(log_array[0]))
                        month.TryGetValue(log_array[0], out mon);
                    /* 日志前缀处理 */
                    string year = DateTime.Now.ToString("yyyy");
                    //string mon = DateTime.Now.ToString("MM");// 需删除
                    string handle_result = null; string handle_reason = null; string time = null; string host_name = null; string PROTO = null;
                    string tab_name = null;
                    if (log_array[1] == "")
                    {
                        string[] prefix_pre1 = log_array[6].Split('&');
                        handle_result = prefix_pre1[0];
                        PROTO = prefix_pre1[1];
                        string handle_reason0 = prefix_pre1[2];
                        handle_reason = handle_reason0.Replace('_', ' ');
                        time = year + "-" + mon + "-" + log_array[2] + '-' + log_array[3];
                        tab_name = year + mon;
                        host_name = log_array[4];
                    }
                    else if (log_array[1] != "")
                    {
                        string[] prefix_pre1;
                        prefix_pre1 = log_array[7].Split('&');
                        if (prefix_pre1.Length <= 1)
                            prefix_pre1 = log_array[6].Split('&');
                            handle_result = prefix_pre1[0];
                            PROTO = prefix_pre1[1];                            
                        string handle_reason0 = prefix_pre1[2];
                        handle_reason = handle_reason0.Replace('_', ' ');
                        time = year + "-" + mon + "-" + log_array[1] + '-' + log_array[2];
                        tab_name = year + mon;
                        host_name = log_array[3];
                    }

                    string MAC = log.Substring(log.IndexOf("MAC") + 4, log.IndexOf("SRC") - log.IndexOf("MAC") - 4);
                    string data_in = log.Substring(log.IndexOf("IN") + 3, log.IndexOf("OUT") - log.IndexOf("IN") - 3);
                    string PHYSIN = log.Substring(log.IndexOf("PHYSIN") + 7, log.IndexOf("PHYSOUT") - log.IndexOf("PHYSIN") - 7);
                    string fw_MAC = log.Substring(log.IndexOf("$") + 1, 17);
                    string data_out = log.Substring(log.IndexOf("OUT") + 4, log.IndexOf("PHYSIN") - log.IndexOf("OUT") - 4);
                    string PHYSOUT = log.Substring(log.IndexOf("PHYSOUT") + 8, log.IndexOf("MAC") - log.IndexOf("PHYSOUT") - 8);
                    string DST_MAC = MAC.Substring(0, 17);
                    string SRC_MAC = MAC.Substring(18, 17);
                    string SRC = log.Substring(log.IndexOf("SRC") + 4, log.IndexOf("DST") - log.IndexOf("SRC") - 4);
                    string DST = log.Substring(log.IndexOf("DST") + 4, log.IndexOf("LEN") - log.IndexOf("DST") - 4);
                    string LEN = log.Substring(log.IndexOf("LEN") + 4, log.IndexOf("TOS") - log.IndexOf("LEN") - 4);
                    string TOS = log.Substring(log.IndexOf("TOS") + 4, log.IndexOf("PREC") - log.IndexOf("TOS") - 4);
                    string PREC = log.Substring(log.IndexOf("PREC") + 5, log.IndexOf("TTL") - log.IndexOf("PREC") - 5);
                    string TTL = log.Substring(log.IndexOf("TTL") + 4, log.IndexOf("ID") - log.IndexOf("TTL") - 4);
                    string ID = log.Substring(log.IndexOf("ID") + 3, log.IndexOf("PROTO") - log.IndexOf("ID") - 6);
                    //string PROTO = log.Substring(log.IndexOf("PROTO") + 6, log.IndexOf("SPT") - log.IndexOf("PROTO") - 6);
                    string SPT = log.Substring(log.IndexOf("SPT") + 4, log.IndexOf("DPT") - log.IndexOf("SPT") - 4);
                    string DPT = log.Substring(log.IndexOf("DPT") + 4, log.IndexOf("WINDOW") - log.IndexOf("DPT") - 4);
                    /*按月份建表并存入数据库*/
                    string sqlCreate = "CREATE TABLE if not EXISTS iptables_log" + tab_name + " (time datetime DEFAULT NULL, host_name varchar(50) DEFAULT NULL, fw_MAC varchar(20) DEFAULT NULL, data_in varchar(10) DEFAULT NULL, PHYSIN varchar(10) DEFAULT NULL, data_out varchar(10) DEFAULT NULL, PHYSOUT varchar(10) DEFAULT NULL, DST_MAC varchar(20) DEFAULT NULL, SRC_MAC varchar(20) DEFAULT NULL, SRC varchar(20) DEFAULT NULL, DST varchar(20) DEFAULT NULL, LEN varchar(10) DEFAULT NULL, TOS varchar(10) DEFAULT NULL, PREC varchar(10) DEFAULT NULL, TTL varchar(10) DEFAULT NULL, ID varchar(10) DEFAULT NULL, PROTO varchar(10) DEFAULT NULL, SPT varchar(10) DEFAULT NULL, DPT varchar(10) DEFAULT NULL, handle_result varchar(30) DEFAULT NULL, handle_reason varchar(255) DEFAULT NULL) ENGINE=InnoDB DEFAULT CHARSET=utf8;insert into iptables_log" + tab_name + "(time,host_name,fw_MAC,data_in,PHYSIN,data_out,PHYSOUT,DST_MAC,SRC_MAC,SRC,DST,LEN,TOS,PREC,TTL,ID,PROTO,SPT,DPT,handle_result,handle_reason) values('" + time + "','" + host_name + "', '" + fw_MAC + "','" + data_in + "','" + PHYSIN + "','" + data_out + "','" + PHYSOUT + "','" + DST_MAC + "','" + SRC_MAC + "','" + SRC + "','" + DST + "','" + LEN + "','" + TOS + "','" + PREC + "','" + TTL + "','" + ID + "','" + PROTO + "','" + SPT + "','" + DPT + "','" + handle_result + "','" + handle_reason + "')";

                    MySqlCommand cmd_create = new MySqlCommand(sqlCreate, con);
                    cmd_create.ExecuteNonQuery();
                    string displayLog = time + " " + host_name + " " + SRC + " " + DST + " " + ID + " " + PROTO + " " + SPT + " " + DPT + " " + handle_result;

                    real_time.Enqueue(displayLog);//日志信息入队
                    

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
                listener.Close();
            }
        }


        //        /// <summary>
        //        /// 日志消息队列
        //        /// </summary>
        //        /// <param name="real_content">UDP客户端监听到的原始日志</param>
        //        public void DisplayLog()
        //        {
        //            UdpClient listener = new UdpClient(listenPort);
        //            IPEndPoint groupEP = null;

        //            try
        //            {
        //                while (true)
        //                {
        //                    /* 监听并接收日志 */
        //                    byte[] content = listener.Receive(ref groupEP);
        //                    string log = Encoding.Default.GetString(content);
        //                    real_time.Enqueue(log);//日志信息入队  

        //#if debug
        //                    Console.WriteLine("{0}", real_time.Dequeue());
        //#endif
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.ToString());
        //            }
        //            finally
        //            {
        //                listener.Close();
        //            }

        //        }
    }
}
