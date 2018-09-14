using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class ResetIP:IResetIP
    {
        bool IResetIP.ResetIP(ProtecDeviceForm fw_dev,string BindIP)
        {
           
            string cmd = "ifconfig br0 down && ifconfig br0 " +BindIP+" up";
            fw_dev.setDev_port(22222);
            SendInfo sendResetcmd = new SendInfo(fw_dev);

            if (sendResetcmd.SendConfigInfo(cmd))
            {
                fw_dev.setDev_IP(BindIP);
                return true;
            }

            else
            {
                return false;
            }
        }

    }
}
