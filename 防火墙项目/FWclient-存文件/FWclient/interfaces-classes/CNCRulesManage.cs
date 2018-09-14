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
        FileOperation fo = new FileOperation("C:\\CMP\\Config");

        public CNCRulesManage()
        {
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
            string frule = devIP + " " + log_flag.ToString() + " " + connlimit + " " + srcIP + " " + dstIP + " " + sport + " " + dport;
            fo.SaveRules(frule,"cnc");
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
            string frule = devIP + " " + log_flag.ToString() + " " + connlimit + " " + srcIP + " " + dstIP + " " + sport + " " + dport;
            fo.DeleteRules(frule, "cnc");
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
