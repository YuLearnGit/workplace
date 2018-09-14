using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class STDRulesManage : ISTDRulesManage
    {
        private DeviceForm devform;
        private DBOperation db_operate;

        public STDRulesManage()
        {
            this.db_operate = new DBOperation();
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }

        public bool AddSTDRules(string devIP, bool log_flag, string protocol, string srcIP, string dstIP, string sport, string dport)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule1 = "iptables -A FORWARD -p " + protocol;
            if(srcIP != "")
                rule1 = rule1 + " -s " + srcIP;
            if (sport != "")
                rule1 = rule1 + " --sport " + sport;
            if (dstIP != "")
                rule1 = rule1 + " -d " + dstIP;
            if (dport != "")
                rule1 = rule1 + " --dport " + dport;
            
            string rule = "STD1" + rule1 + " -m state --state NEW -j ACCEPT";
            if (log_flag)
                rule = rule + " && " + rule1 + " -m state --state NEW -j LOG";

            string sql_str = "INSERT INTO STD VALUES " + "('" + StaticGlobal.firewallmac + "'," + log_flag.ToString() + ",'" + protocol + "','" + srcIP + "','" + dstIP + "','" + sport + "','" + dport + "')";
            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool DelSTDRules(string devIP, bool log_flag, string protocol, string srcIP, string dstIP, string sport, string dport)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule1 = "iptables -A FORWARD -p " + protocol;
            if (srcIP != "")
                rule1 = rule1 + " -s " + srcIP;
            if (sport != "")
                rule1 = rule1 + " --sport " + sport;
            if (dstIP != "")
                rule1 = rule1 + " -d " + dstIP;
            if (dport != "")
                rule1 = rule1 + " --dport " + dport;

            string rule = "STD0" + rule1 + " -m state --state NEW -j ACCEPT";
            if (log_flag)
                rule = rule + " && " + rule1 + " -m state --state NEW -j LOG";
            string sql_str = "DELETE FROM STD WHERE (fwmac='" + StaticGlobal.firewallmac + "' and protocol='" + protocol + "' and srcIP='" + srcIP + "' and dstIP='" + dstIP + "' and sport='" + sport + "' and dport='" + dport + "')"; 
            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
