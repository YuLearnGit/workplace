
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    /// <summary>
    /// 防火墙规则管理，包括添加和删除规则
    /// </summary>
    class DPIRulesManage : IDPIRulesManage
    {
        public bool ClearAllRules(string dev_IP)
        {
            DeviceForm devform = new DeviceForm(dev_IP, 22222);

            ConfigDPIRules configDevice = new ConfigDPIRules(devform);
            return  configDevice.ClearAllRules();
        }
        
        public bool ChangeDNP3Rules(string dst_IP, string src_IP, string dev_IP, bool log_record,bool add_delete)
        {
            DNP3RulesForm dnp3rf = new DNP3RulesForm();
            dnp3rf.setDst_IPAndSrc_IP(dst_IP, src_IP);

            DeviceForm devform = new DeviceForm(dev_IP, 22222);
           
            ConfigDPIRules configDevice = new ConfigDPIRules(devform);
            return  configDevice.ConfigDNP3Rules(dnp3rf,log_record,add_delete);
        }

        public bool ChangeModbusTcpRules(string dst_IP, string src_IP, string min_addr, string max_addr, string func, int Min_data, int Max_data, string dev_IP, bool log_record, bool add_delete)
        {
            ModbusTcpRulesForm mtrf = new ModbusTcpRulesForm();
            mtrf.setIP_Addr_Funcode(dst_IP, src_IP, min_addr, max_addr, func, Min_data, Max_data);

            DeviceForm devform = new DeviceForm(dev_IP, 22222);

            IConfigRules configDevice = new ConfigDPIRules(devform);
            return configDevice.ConfigModbusTcpRules(mtrf, log_record,add_delete);
        }

        public bool ChangeOPCRules(string dst_IP, string src_IP, string dev_IP, bool log_record,bool add_delete)
        {
            OPCRulesForm orf = new OPCRulesForm();
            orf.setDst_IPAndSrc_IP(dst_IP, src_IP);

            DeviceForm devform = new DeviceForm(dev_IP, 22222);

            ConfigDPIRules configDevice = new ConfigDPIRules(devform);
            return  configDevice.ConfigOPCRules(orf,log_record,add_delete);
        }

        //public bool ChangeWhiteLists(string dst_IP, string src_IP, string dev_IP,string dst_port, string src_port, bool log_record, bool add_delete)
        //{
        //    WhiteLists lists = new WhiteLists();
        //    lists.setIPAndPort(dst_IP,src_IP,dst_port,src_port);

        //    DeviceForm devform = new DeviceForm(dev_IP, 22222);
        //    ConfigDPIRules configDevice = new ConfigDPIRules(devform);
        //    return configDevice.ConfigWhiteLists(lists,add_delete);

        //}

    }
}
