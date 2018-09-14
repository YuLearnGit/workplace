using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class APCRulesManage : IAPCRulesManage
    {
        private DeviceForm devform;
        private DBOperation db_operate;
        private Dictionary<string ,string> protocol_port;

        public APCRulesManage()
        {
            this.db_operate = new DBOperation();
            this.devform = new DeviceForm("0.0.0.0", 22222);
            this .protocol_port = new Dictionary<string ,string >()
            {
                { "ftp", "21"},
                { "http", "80"},
                { "telnet", "23"},
                { "smtp", "25"},
                { "pop3","110"},
                { "ssh" ,"22"}
            };
        }

        public bool ApplicationProtocolControl(string devIP, string protocol, bool pro_status)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string port = protocol_port[protocol];
            string rule1 = "iptables -A INPUT -p tcp --dport " + port + " -j ACCEPT && " +
                           "iptables -A OUTPUT -p tcp --dport " + port + " -j ACCEPT && " +
                           "iptables -A FORWARD -p tcp --sport " + port + " -j ACCEPT && " +
                           "iptables -A FORWARD -p tcp --dport " + port + " -j ACCEPT";
            string rule2 = "iptables -A INPUT -p tcp --dport " + port + " -j DROP && " +
                           "iptables -A OUTPUT -p tcp --dport " + port + " -j DROP && " +
                           "iptables -A FORWARD -p tcp --sport " + port + " -j DROP && " +
                           "iptables -A FORWARD -p tcp --dport " + port + " -j DROP";

            string rule=string.Empty;
            string sql_str=string.Empty;
            FileOperation of = new FileOperation("C:\\CMP\\Config");
            if (pro_status)
            {
                rule = "APC2" + rule1 + "#" + protocol;
                of.ChangeRules(protocol+" "+"forbid",protocol+" "+"allow","apc");
            }
            else
            {
                rule = "APC2" + rule2 + "#" + protocol;
                of.ChangeRules(protocol + " " + "allow", protocol + " " + "forbid", "apc");
            }
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
