using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FireWall
{
   public class PRTRuleDataTable: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
   
        public string route_type { get; set; }//路由类型
        public string host_IP { get; set; }//主机IP
        public string dstIP { get; set; }//添加到路由目的网络IP，不能为空字符串
        public string netmask { get; set; }//网络掩码
        public string ETH { get; set; }//为路由指定的网络接口，如eth0等等，如不配置该项则传入空字符串
        public string Gateway { get; set; }//路由数据包通过的网关，如不配置该项则传入空字符串
        public bool log { get; set; }
    }
}
