using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IPRTRulesManage
    {
        /// <summary>
        /// 配置到主机的路由
        /// </summary>
        /// <param name="devIP">防火墙或设备IP</param>
        /// <param name="add_del_flag">true表示添加路由，false表示删除路由</param>
        /// <param name="host">添加到路由的主机IP，不能为空字符串</param>
        /// <param name="Iface">为路由指定的网络接口，如eth0等等，如不配置该项则传入空字符串</param>
        /// <param name="gateway">路由数据包通过的网关，如不配置该项则传入空字符串</param>
        /// <returns>你懂的</returns>
        bool HostRouteConfig(string devIP, bool add_del_flag, string host, string Iface, string gateway);

        /// <summary>
        /// 配置到网络的路由
        /// </summary>
        /// <param name="devIP">防火墙或设备IP</param>
        /// <param name="add_del_flag">true表示添加路由，false表示删除路由</param>
        /// <param name="net">添加到路由的目的网络IP，不能为空字符串</param>
        /// <param name="netmask">目的地址的网络掩码，不能为空字符串</param>
        /// <param name="Iface">为路由指定的网络接口，如eth0等等，如不配置该项则传入空字符串</param>
        /// <param name="gateway">路由数据包通过的网关，如不配置该项则传入空字符串</param>
        /// <returns>你懂的</returns>
        bool NetRouteConfig(string devIP, bool add_del_flag, string net, string netmask, string Iface, string gateway);

        /// <summary>
        /// 配置默认路由
        /// </summary>
        /// <param name="devIP">防火墙或设备IP</param>
        /// <param name="add_del_flag">true表示添加路由，false表示删除路由</param>
        /// <param name="Iface">为路由指定的网络接口，如eth0等等，如不配置该项则传入空字符串</param>
        /// <param name="gateway">路由数据包通过的网关，如不配置该项则传入空字符串</param>
        /// <returns>你懂的</returns>
        bool DefaultRouteConfig(string devIP, bool add_del_flag, string Iface, string gateway);
    }
}
