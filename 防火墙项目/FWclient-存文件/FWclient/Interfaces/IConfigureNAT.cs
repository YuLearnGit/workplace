
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IConfigureNAT
    {
        ///<summary>
        ///配置源地址映射
        ///<summary>
        ///<param name="FWDeviceForm fw_dev">防火墙设备</param>
        ///<param name="EthName">对应设备的防火墙网口</param>
        ///<param name="devIP">要进行源地址映射的设备IP地址</param>
        ///<param name="EthIP">对应设备的防火墙网口IP</param>
        ///<param name="add_delete">添加或者删除映射规则(true 为添加)</param>
        bool ConfigSNAT(FWDeviceForm fw_dev,string EthName, string devIP,string EthIP,bool add_delete);

        ///<summary>
        ///配置目的地址映射
        ///<summary>
        ///<param name="FWDeviceForm fw_dev">防火墙设备</param>
        ///<param name="Original_DIP">原目的地址</param>     
        ///<param name="Original_dport">原目的端口,any表示任意端口</param> 
        ///<param name="Map_IP">映射IP</param>
        ///<param name="Map_port">映射端口，any表示任意端口</param>
        ///<param name="add_delete">添加或者删除映射规则(true 为添加)</param>
        bool ConfigDNAT(FWDeviceForm fw_dev,string Original_DIP,string Original_dport,string Map_IP,string Map_port,bool add_delete);
       
        
        /// <summary>
        /// 清除NAT规则
        /// <param name="FWDeviceForm fw_dev">防火墙设备</param>
        /// </summary>
        bool ClearNATRules(FWDeviceForm fw_dev);

    }
}
