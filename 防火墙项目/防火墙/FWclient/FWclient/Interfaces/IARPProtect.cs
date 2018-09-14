using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IARPProtect
    {
        /// <summary>
        /// 配置ARP防护
        /// </summary>
        /// <param name="dev_IP">被保护设备IP</param>
        /// <param name="gateIP">网关IP</param>
        /// <param name="gateMAC">网关MAC地址</param>
        /// <param name="addDelete">true为添加，false为删除</param>
        /// <returns></returns>
        bool configARPRules(string dev_IP,string gateIP,string gateMAC,bool addDelete);
    }
}
