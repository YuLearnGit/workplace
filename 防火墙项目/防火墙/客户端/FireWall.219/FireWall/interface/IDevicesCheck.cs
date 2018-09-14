using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    interface IDevicesCheck
    {
        /// <summary>
        /// 扫描并确认网络中存在的防火墙设备，返回其IP地址列表
        /// </summary>
        /// <param name="start_IP">起始IP地址</param>
        /// <param name="end_IP">结束IP地址</param>
        /// <returns>返回List<string>，防火墙设备的IP地址列表</returns>
        List<FWDeviceForm> CheckDevices(string start_IP, string end_IP);
    }
}
