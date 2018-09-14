using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class GetLocalMacAddr
    {
        public static string GetMacAddr()
        {
            try
            {
                string mac = null;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {

                    if ((bool)mo["IPEnabled"] == true)
                    {
                        string serverName = mo["ServiceName"].ToString();
                        if (serverName.ToLower().Contains("vmnetadapter") || serverName.ToLower().Contains("ppoe") ||
                            serverName.ToLower().Contains("nic"))
                        { continue; }
                        else
                            mac = mo["MacAddress"].ToString();
                        mo.Dispose();
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
