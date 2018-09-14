#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.SqlClient;
namespace FWclient
{
    /// <summary>
    /// 接口测试
    /// </summary>
    class Test
    {
        static void Main(string[] args)
        {
            string str = "C:\\CMP\\Config";
            FileOperation op = new FileOperation(str);
            //op.SearchRules("modbustcp.config");
            //string rules = "DPI test Rule";
            op.SaveRules("3333", "modbustcp.config");

            //            IDevicesCheck devConfirm = new DevicesCheck();
            //            List<FWDeviceForm> fws = devConfirm.CheckDevices("172.16.10.9", "172.16.10.10");

            //            Console.WriteLine("打印扫描结果 :");
            //            IReceiveLog log = new ReceiveLog();
            //            log.Save_DisplayLog(true);
            //            foreach (FWDeviceForm fw in fws)
            //            {
            //                string fwip = fw.getDev_IP();
            //                string fwmac = fw.getDev_MAC();
            //                List<ProtecDeviceForm> protecDev_list = fw.getProtecDev_list();

            //                Console.WriteLine("防火墙设备IP : {0} 防火墙设备MAC : {1}", fwip, fwmac);

            //                Console.WriteLine("关联的受保护设备 :");

            //                if (fwip != "0.0.0.0")
            //                {

            //                    foreach (var item in protecDev_list)
            //                    {
            //                        string dev_type = item.getDev_type();
            //                        Console.WriteLine("IP {0}   MAC {1} 设备制造商 {2}", item.getDev_IP(), item.getDev_MAC(), dev_type);

            //                    }
            //                }
            //                else Console.WriteLine("防火墙为无IP模式");
            //            }

            //            //IARPProtect tes = new ARPProtect();
            //            //tes.configARPRules("172.16.10.9", "172.16.10.254", "08:57:00:e5:87:a7", true);

#if debug
            Console.ReadLine();
#endif
        }
        //static void Main(string[] args)
        //        {
        //            string connstr = "server=.;database=firewallclient;integrated security=SSPI";
        //            try
        //            {
        //                using (SqlConnection conn = new SqlConnection())
        //                {
        //                    conn.ConnectionString = connstr;
        //                    conn.Open(); // 打开数据库连接
        //                    Console.WriteLine("连接成功");
        //                    string queryStr = "select * from apc";
        //                    SqlCommand command = new SqlCommand(queryStr,conn);
        //                    SqlDataReader reader = command.ExecuteReader();
        //                    while(reader.Read())
        //                    {
        //                        Console.WriteLine(String.Format("{0}, {1}",
        //                        reader[0], reader[1]));
        //                    }
        //                    reader.Close();
        //                    conn.Close(); // 关闭数据库连接
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("错误信息："+ex.ToString());
        //            }
        //            Console.ReadLine();
        //        }
    }
}
