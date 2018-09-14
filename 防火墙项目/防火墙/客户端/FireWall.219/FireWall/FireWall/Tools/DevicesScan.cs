#define debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class DevicesScan : IDevicesScan
    {
        public int ScanDevice(string start_IP, string end_IP)
        {
            string[] sArray_startIP = start_IP.Split('.');
            string[] sArray_endIP = end_IP.Split('.');
            string unchange_part = sArray_startIP[0] + "." + sArray_startIP[1] + "." + sArray_startIP[2] + ".";

            int start = Int32.Parse(sArray_startIP[3]);
            int end = Int32.Parse(sArray_endIP[3]);
            int IP_num = end - start;

            List<string> dev_IP_list = new List<string>();

            for (int count = 0; count + start <= end ; count++)
            {
                dev_IP_list.Add(unchange_part + Convert.ToString(count + start));
            }

            foreach (string dev_IP in dev_IP_list)
            {
#if debug
                Console.WriteLine(dev_IP);
#endif
                DeviceForm devform = new DeviceForm(dev_IP, 33333);
                SendInfo sendcheckInfo = new SendInfo(devform);
                sendcheckInfo.SendCheckInfo();
            }

            return IP_num;
        }
    }
}
