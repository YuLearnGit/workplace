using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface ISTDRulesManage
    {
        /// <summary>
        /// 添加状态检测模块规则
        /// </summary>
        /// <param name="devIP">防火墙设备IP</param>
        /// <param name="log_flag">是否记录日志</param>
        /// <param name="protocol">状态检测的协议</param>
        /// <param name="srcIP">状态检测的源IP,若不存在则传入空字符串</param>
        /// <param name="dstIP">状态检测的目的IP，若不存在则传入空字符串</param>
        /// <param name="sport">状态检测的源端口，若不存在则传入空字符串</param>
        /// <param name="dport">状态检测的目的端口,若不存在则传入空字符串</param>
        /// <returns>返回bool</returns>
        bool AddSTDRules(string devIP, bool log_flag, string protocol, string srcIP, string dstIP, string sport, string dport);

        /// <summary>
        ///删除状态检测模块规则
        /// </summary>
        /// <param name="id">该条规则在本地数据库中的主键id</param>
        /// <param name="devIP"></param>
        /// <param name="log_flag"></param>
        /// <param name="protocol"></param>
        /// <param name="srcIP"></param>
        /// <param name="dstIP"></param>
        /// <param name="sport"></param>
        /// <param name="dport"></param>
        /// <returns></returns>
        bool DelSTDRules(string devIP, bool log_flag, string protocol, string srcIP, string dstIP, string sport, string dport);
    }
}
