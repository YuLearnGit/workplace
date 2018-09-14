
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 将防火墙设备恢复成有IP模式
    /// </summary>
    /// <param name="fw_dev">连接在防火墙上的被保护设备</param>
    /// <param name="BindIP">设置防火墙IP</param>
    interface IResetIP
    {
        bool ResetIP(ProtecDeviceForm fw_dev,string BvindIP);
    }
}
