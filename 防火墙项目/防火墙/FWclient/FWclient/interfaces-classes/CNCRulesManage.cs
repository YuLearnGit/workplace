using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class CNCRulesManage : ICNCRulesManage
    {
        private DeviceForm devform;
        private DBOperation db_operate;

        public CNCRulesManage()
        {
            this.db_operate = new DBOperation();
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }

        public bool AddCNCRules(string devIP, bool log_flag, int connlimit, string srcIP, string dstIP, string sport, string dport)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule1 = "iptables -A FORWARD -p tcp --syn";
            if (srcIP != "")
                rule1 = rule1 + " -s " + srcIP;
            if (sport != "")
                rule1 = rule1 + " --sport " + sport;
            if (dstIP != "")
                rule1 = rule1 + " -d " + dstIP;
            if (dport != "")
                rule1 = rule1 + " --dport " + dport;

            rule1 = rule1 + " -m connlimit --connlimit-above " + Convert.ToString(connlimit);
            string rule = "CNC1" + rule1 + " -j DROP";
            if (log_flag)
                rule = rule + " && " + rule1 + " -j LOG";

            string sql_str = "INSERT INTO CNC VALUES " + "('" + devIP + "'," + log_flag.ToString() + ",'" + connlimit.ToString() + "','" + srcIP + "','" + dstIP + "','" + sport + "','" + dport + "')";
            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool DelCNCRules(string devIP, bool log_flag, int connlimit, string srcIP, string dstIP, string sport, string dport)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule1 = "iptables -A FORWARD -p tcp --syn";
            if (srcIP != "")
                rule1 = rule1 + " -s " + srcIP;
            if (sport != "")
                rule1 = rule1 + " --sport " + sport;
            if (dstIP != "")
                rule1 = rule1 + " -d " + dstIP;
            if (dport != "")
                rule1 = rule1 + " --dport " + dport;

            rule1 = rule1 + " -m connlimit --connlimit-above " + Convert.ToString(connlimit);
            string rule = "CNC0" + rule1 + " -j DROP";
            if (log_flag)
                rule = rule + " && " + rule1 + " -j LOG";

            string sql_str = "DELETE FROM CNC WHERE (devIP='" + devIP + "' and connlimit=" + connlimit + " and srcIP='" + srcIP + "' and dstIP='" + dstIP + "' and sport='" + sport + "' and dport='" + dport + "')";
            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
