using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FireWall
{
   public class ProtecDeviceForm : DeviceForm
    {
        private string dev_type;

        public ProtecDeviceForm(string dev_IP, string dev_MAC) : base(dev_IP,dev_MAC)
        {
            if (dev_MAC != "")
            {
                string[] macArray = dev_MAC.Split(':');
                string macQuery = macArray[0] + "-" + macArray[1] + "-" + macArray[2];
                string sqlSearch = "select Manufacturers from macs where Macs=" + "'" + macQuery + "'";
                MySqlConnection con = new MySqlConnection(StaticGlobal.ConnectionString);
                MySqlCommand cmd = new MySqlCommand(sqlSearch, con);
                try
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                        this.dev_type = reader.GetString(0);
                    else
                        this.dev_type = "未知设备";
                    reader.Close();
                }
                catch (Exception e)
                {
                    this.dev_type = "未知设备";
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                this.dev_type = "未知设备";
            }
        }

        public string getDev_type()
        {
            return dev_type;
        }
    }
}
