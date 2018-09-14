#define debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using SharpPcap;
using System.Windows;

namespace FireWall
{
    class NoIPConfig : INoIPConfig
    {
        bool INoIPConfig.NoipConfig(FWDeviceForm fw_dev)
        {
            string cmd = "ifconfig br0 down && ifconfig br0 0.0.0.0 up";
            fw_dev.setDev_port(22222);
            /*
             *本身就无IP的防火墙不能配置为无IP模式
             */
            if (fw_dev.getDev_IP() == "0.0.0.0")
                return false;
            SendInfo sendcmd = new SendInfo(fw_dev);
            if (sendcmd.SendConfigInfo(cmd))
            {
                fw_dev.setDev_IP("0.0.0.0");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
