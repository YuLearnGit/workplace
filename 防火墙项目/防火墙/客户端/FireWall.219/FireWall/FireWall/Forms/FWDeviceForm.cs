using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    /// <summary>
    /// 防火墙设备
    /// </summary>
    public class FWDeviceForm : DeviceForm
    {
        private List<string> ProdevIP_list = new List<string>();
        private List<ProtecDeviceForm> ProtecDev_list = new List<ProtecDeviceForm>();

        public FWDeviceForm(string fw_IP, int fw_port, string fw_mac) : base(fw_IP, fw_port, fw_mac)
        {

        }

        public void addProtecDev(ProtecDeviceForm protecDev)
        {
            ProtecDev_list.Add(protecDev);
        }

        public List<ProtecDeviceForm> getProtecDev_list()
        {
            return ProtecDev_list;
        }

        public void addProtecDevIP(string dev_IP)
        {
            ProdevIP_list.Add(dev_IP);
        }

        public List<string> getProtecDevIP_list()
        {
            return ProdevIP_list;
        }
    }
}