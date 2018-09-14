using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class GetLocalMacAddr
    {
        public static string GetMacAddr()
        {
            try
            {
                //string mac = "40-8D-5C-08-53-A7";
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }

                moc = null;
                mc = null;
                return mac;
            }
            catch
            {

                return "unknow";
            }
            finally
            {

            }
        }
    }
}
