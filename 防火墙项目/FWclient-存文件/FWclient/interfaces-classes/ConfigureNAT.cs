
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ConfigureNAT : IConfigureNAT
    {
        FileOperation of = new FileOperation("C:\\CMP\\Config");
        public bool ConfigSNAT(FWDeviceForm fw_dev, string EthName, string devIP, string EthIP, bool add_delete)
        {
            string flag = ""; string configEth_bridge = ""; string configInfo = ""; string configEth_IP = "";
            string rule = "iptables -t nat -A POSTROUTING -s " + devIP + " -o br0 -j SNAT --to-source " + fw_dev.getDev_IP();
            if (add_delete)
            {
                flag = "NAT1";
                configEth_bridge = "brctl delif br0 " + EthName;//先将网口从网桥上删除
                configEth_IP = "ifconfig " + EthName + " " + EthIP + " netmask 255.255.255.0" + " up";
                configInfo = flag + configEth_bridge + " && " + configEth_IP + " && " + rule;
                string frule = fw_dev.getDev_IP()+ " "+devIP+" " + EthName + " " + EthIP+" "+ fw_dev.getDev_IP();
                of.SaveRules(frule, "snat");
            }
           else if (!add_delete)
            {
                flag = "NAT0";
                configEth_bridge = "brctl addif br0 " + EthName;
                configEth_IP = "ifconfig " + EthName + " " + "0.0.0.0 up";
                configInfo = flag + configEth_IP + " && " + configEth_bridge + " && " + rule;
                string frule = fw_dev.getDev_IP() + " " + devIP + " " + EthName + " " + EthIP + " " + fw_dev.getDev_IP();
                of.DeleteRules(frule, "snat");
            }

            fw_dev.setDev_port(22222);
            SendInfo sendcmd = new SendInfo(fw_dev);    
            return sendcmd.SendConfigInfo(configInfo);        
        }


      public  bool ConfigDNAT(FWDeviceForm fw_dev, string Original_DIP, string Original_dport, string Map_IP, string Map_port, bool add_delete)
        {
            string flag = "";string pre_rule = "";//string post_rule = "";
            if (Original_dport != "any" & Map_port != "any")
            {
                pre_rule = "iptables -t nat -A PREROUTING -d" + " " + Original_DIP + " " + "-p tcp --dport " + Original_dport
                         + " -j DNAT --to-destination " + Map_IP + ":" + Map_port;
                //post_rule = "iptables -t nat -A POSTROUTING -d"+" "+Map_IP+" "+"-p tcp --dport "+Map_port
                //    +" -j SNAT --to "+Original_DIP+":"+Original_dport;
            }
            if (Original_dport != "any" & Map_port == "any")
            {
                pre_rule ="iptables -t nat -A PREROUTING -d" + " " + Original_DIP + " " + "-p tcp --dport " + Original_dport
                           + " -j DNAT --to-destination " + Map_IP;
               
            }
            if (Original_dport == "any" & Map_port != "any")
            {
                pre_rule = "iptables -t nat -A PREROUTING -d" + " " + Original_DIP + " " + "-p tcp " + " -j DNAT --to-destination " + Map_IP + ":" + Map_port;
            }
            if (Original_dport == "any" & Map_port == "any")
            {
                pre_rule ="iptables -t nat -A PREROUTING -d" + " " + Original_DIP + " " + "-p tcp " + " -j DNAT --to-destination " + Map_IP;
            }

            if (add_delete)
            {
                flag = "NAT1";
                string rfule = fw_dev.getDev_IP()+" "+ Original_DIP + " "+ Original_dport + " "+ Map_IP + " "+ Map_port;
                of.SaveRules(rfule, "dnat");
            }
            else if (!add_delete)
            {
                flag = "NAT0";
                string rfule = fw_dev.getDev_IP() + " " + Original_DIP + " " + Original_dport + " " + Map_IP + " " + Map_port;
                of.DeleteRules(rfule, "dnat");
            }
            string configrule = flag + pre_rule;
          
            fw_dev.setDev_port(22222);
            SendInfo sendcmd = new SendInfo(fw_dev);
            return sendcmd.SendConfigInfo(configrule);
                                  
        }

        public bool ClearNATRules(FWDeviceForm fw_dev)
        {
            string rule = "iptables -t nat -F";
            fw_dev.setDev_port(22222);
            SendInfo sendcmd = new SendInfo(fw_dev);
            return sendcmd.SendConfigInfo(rule);

        }
    }
}
