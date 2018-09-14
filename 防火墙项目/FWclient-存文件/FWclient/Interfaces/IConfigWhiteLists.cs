using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IConfigWhiteLists
    {
        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        /// <param name="dst_IP">白名单中的目的地址</param>
        /// <param name="src_IP">白名单中的源地址</param>

        /// <param name="dst_port">白名单中的目的端口</param>
        /// <param name="src_port">白名单中的源端口</param>
        /// <param name="log_record">是否记录日志</param>
        /// <param name="add_delete">添加或者删除(值为true时添加)</param>
        bool ChangeWhiteLists(string dev_IP, string dst_IP, string src_IP, string dst_port, string src_port, bool log_record, bool add_delete);
    }
}
