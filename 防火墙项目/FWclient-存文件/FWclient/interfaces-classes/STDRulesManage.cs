using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class STDRulesManage : ISTDRulesManage
    {
        private DeviceForm devform;
        FileOperation of = new FileOperation("C:\\CMP\\Config");

        public STDRulesManage()
        {
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

            string frule = devIP  + " " + protocol + " " + srcIP + " " + dstIP + " " + sport + " " + dport + " " + log_flag.ToString();
            of.SaveRules(frule, "std");
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
            string frule = devIP + " " + protocol + " " + srcIP + " " + dstIP + " " + sport + " " + dport + " " + log_flag.ToString();
            of.DeleteRules(frule, "std");
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
