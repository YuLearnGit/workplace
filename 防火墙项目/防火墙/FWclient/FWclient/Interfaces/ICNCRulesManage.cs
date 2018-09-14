using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface ICNCRulesManage
    {
        /// <summary>
        /// 添加控制IP连接数的规则
        /// </summary>
        /// <param name="devIP">设备或防火墙IP</param>
        /// <param name="log_flag">是否记录日志</param>
        /// <param name="connlimit">最大并发会话数</param>
        /// <param name="srcIP"></param>
        /// <param name="dstIP"></param>
        /// <param name="sport"></param>
        /// <param name="dport"></param>
        /// <returns></returns>
        bool AddCNCRules(string devIP, bool log_flag, int connlimit, string srcIP, string dstIP, string sport, string dport);

        /// <summary>
        /// 删除控制IP连接数的规则
        /// </summary>
        /// <param name="devIP">设备或防火墙IP</param>
        /// <param name="log_flag">是否记录日志</param>
        /// <param name="connlimit">最大并发会话数</param>
        /// <param name="srcIP"></param>
        /// <param name="dstIP"></param>
        /// <param name="sport"></param>
        /// <param name="dport"></param>
        /// <returns></returns>
        bool DelCNCRules(string devIP, bool log_flag, int connlimit, string srcIP, string dstIP, string sport, string dport);
    }
}
