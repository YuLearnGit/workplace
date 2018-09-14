using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{   
    interface IDPIRulesManage
    {
        /// <summary>
        /// 添加modbusTcp规则
        /// </summary>
        /// <param name="dst_IP">modbusTcp规则中的目的地址,即从机IP</param>
        /// <param name="src_startIP">modbusTcp规则中源地址范围的起始地址</param>
        /// <param name="src_endIP">modbusTcp规则中源地址范围的结束地址</param>
        /// <param name="min_addr">modbusTcp规则中的最小线圈地址</param>
        /// <param name="max_addr">modbusTcp规则中的最大线圈地址</param>
        /// <param name="function_code_select">modbusTcp规则中允许通过的功能码，该字符串数组元素只能表示为十进制形式，如“01”“33”</param>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        /// <param name="log_record">是否记录日志</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ChangeModbusTcpRules(string dst_IP, string src_IP, string min_addr, string max_addr, string func, int min_data, int max_data, string dev_IP, bool log_record, bool add_delete);

        /// <summary>
        /// 添加OPC规则
        /// </summary>
        /// <param name="dst_IP">OPC规则中的目的地址</param>
        /// <param name="src_IP">OPC规则中的源地址</param>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        /// <param name="log_record">是否记录日志</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ChangeOPCRules(string dst_IP, string src_IP, string dev_IP, bool log_record,bool add_delete);

        /// <summary>
        /// 添加DNP3规则
        /// </summary>
        /// <param name="dst_IP">DNP3规则中的目的地址</param>
        /// <param name="src_IP">DNP3规则中的源地址</param>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        /// <param name="log_record">是否记录日志</param>
        /// <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
        bool ChangeDNP3Rules(string dst_IP, string src_IP, string dev_IP, bool log_record, bool add_delete);

        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="dst_IP">白名单中的目的地址</param>
        /// <param name="src_IP">白名单中的源地址</param>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        /// <param name="dst_port">白名单中的目的端口</param>
        /// <param name="src_port">白名单中的源端口</param>
        /// <param name="log_record">是否记录日志</param>
        /// <param name="add_delete">添加或者删除(值为true时添加)</param>
        // bool ChangeWhiteLists(string dst_IP,string src_IP,string dev_IP,string dst_port,string src_port,bool log_record,bool add_delete);

        /// <summary>
        /// 清除所有规则
        /// </summary>
        /// <param name="dev_IP">防火墙设备IP地址</param>
        bool ClearAllRules(string dev_IP);

    }
}
