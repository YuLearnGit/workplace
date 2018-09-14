using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireWall;
using MySql.Data.MySqlClient;

namespace FireWall
{
    class DBOperation
    {
        private string con_str = StaticGlobal.ConnectionString;
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
    }
}
