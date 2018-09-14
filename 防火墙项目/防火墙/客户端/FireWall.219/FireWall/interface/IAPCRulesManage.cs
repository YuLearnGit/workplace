using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    interface IAPCRulesManage
    {
       /// <summary>
       /// 控制应用层协议
       /// </summary>
       /// <param name="devIP">防火墙或设备IP</param>
       /// <param name="protocol">需要进行控制的应用层协议，必须小写</param>
       /// <param name="pro_status">true表示允许该协议，false表示禁止该协议</param>
       /// <returns>你知道的</returns>
        bool ApplicationProtocolControl(string devIP, string protocol, bool pro_status);
    }
}
