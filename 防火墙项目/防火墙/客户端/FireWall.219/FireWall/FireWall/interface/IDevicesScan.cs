using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    interface IDevicesScan
    {
        /// <summary>
        /// 扫描一个IP范围内的防火墙设备
        /// </summary>
        /// <param name="start_IP">起始IP地址</param>
        /// <param name="end_IP">结束IP地址</param>
        int ScanDevice(string start_IP,string end_IP);
    }
}
