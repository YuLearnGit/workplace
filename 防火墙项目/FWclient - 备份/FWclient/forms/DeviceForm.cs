using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 一般设备，可代表受保护设备
    /// </summary>
    public class DeviceForm
    {
        private string dev_IP;  //设备ip
        private int dev_port;   //设备端口
        private string dev_MAC;

        public DeviceForm(string dev_IP, int dev_port)
        {
            this.dev_IP = dev_IP;
            this.dev_port = dev_port;
        }

        public DeviceForm(string dev_IP, string dev_MAC)
        {
            this.dev_IP = dev_IP;
            this.dev_MAC = dev_MAC;
        }

        public DeviceForm(string dev_IP, int dev_port, string dev_MAC)
        {
            this.dev_IP = dev_IP;
            this.dev_port = dev_port;
            this.dev_MAC = dev_MAC;
        }

        public string getDev_IP()
        {
            return dev_IP;
        }

        public void setDev_IP(string dev_IP)
        {
            this.dev_IP = dev_IP;
        }

        public int getDev_port()
        {
            return dev_port;
        }

        public void setDev_port(int dev_port)
        {
            this.dev_port = dev_port;
        }

        public string getDev_MAC()
        {
            return dev_MAC;
        }

        public void setDev_MAC(string dev_MAC)
        {
            this.dev_MAC = dev_MAC;
        }
    }
}
