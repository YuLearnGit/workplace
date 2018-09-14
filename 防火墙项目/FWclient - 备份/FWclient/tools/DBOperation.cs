using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FWclient
{
    class DBOperation
    {
        private string con_str = "Server=localhost;Database=firewallclient;Uid=root;Pwd=123456;CharSet=utf8";
        private MySqlConnection conn;
        private int i;

        public DBOperation()
        {
            try
            {
                this.conn = new MySqlConnection(con_str);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int dboperate(string sql_str)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql_str, conn);
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
            return i;
        }

        public string getFwMAC(string dev_IP)
        {
            string fwmac = null;
            try
            {
                conn.Open();
                MySqlDataReader data = null;
                string sqlRule = "select fw_mac from firewallip where dev_ip='" + dev_IP + "'";
                MySqlCommand cmd = new MySqlCommand(sqlRule,conn);
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    fwmac = data[0].ToString();
                }
                data.Close();             
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {                  
                conn.Close();
            }
            if (fwmac != null)
                return fwmac;
            else
                return "error";
        }
    }
}
