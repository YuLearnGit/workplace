/*----------------------------------------------------------------
// Copyright (C) 
// 版权所有。
//
// 文件名：Log
// 文件功能描述：操作日志记录
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Windows;

namespace FireWall
{
    class Log
    {
        //*----------------------------------------------------------------
        //函数说明：操作日志
        //输入：int型 用户id，string型 操作方法
        //输出：无
        //----------------------------------------------------------------*//   
        public static void addlog(int userid,string action,int turbinenum)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string ip=string.Empty;
            //获取操作ip
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = _IPAddress.ToString();
                }
            }
            string CommandText = "insert into operlog values('" + userid + "','" + time + "','" + action + "','" + turbinenum + "','" + ip + "');";
            try
            {
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                MySqlCommand comm = new MySqlCommand(CommandText, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}
