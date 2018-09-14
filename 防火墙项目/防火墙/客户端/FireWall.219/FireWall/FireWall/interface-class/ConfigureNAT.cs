
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class ConfigureNAT : IConfigureNAT
    {    
        private DBOperation NATdb_operate;
        private DeviceForm devform;
        public ConfigureNAT()
        {
            this.NATdb_operate = new DBOperation();
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }

        public bool ConfigSNAT(string dev_IP, string EthName, string devIP, string EthIP, bool add_delete)
        {
            this.devform.setDev_IP(dev_IP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;
            string flag = ""; string configEth_bridge = ""; string configInfo = ""; string configEth_IP = "";string sql_rule = "";
            string rule = "iptables -t nat -A POSTROUTING -s " + devIP + " -o br0 -j SNAT --to-source " + dev_IP;
            if (add_delete)
            {
                flag = "NAT1";
                configEth_bridge = "brctl delif br0 " + EthName;//先将网口从网桥上删除
                configEth_IP = "ifconfig " + EthName + " " + EthIP + " netmask 255.255.255.0" + " up";
                configInfo = flag + configEth_bridge + " && " + configEth_IP + " && " + rule;
                sql_rule = "INSERT INTO snat VALUES('" + StaticGlobal.firewallmac + "', '" + devIP + "', '" + EthName + "', '" + EthIP + "', '" + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] + "'); ";
            }
           else if (!add_delete)
            {
                flag = "NAT0";
                configEth_bridge = "brctl addif br0 " + EthName;
                configEth_IP = "ifconfig " + EthName + " " + "0.0.0.0 up";
                configInfo = flag + configEth_IP + " && " + configEth_bridge + " && " + rule;
                sql_rule = "DELETE FROM snat WHERE fwmac= '" + StaticGlobal.firewallmac + "' and origin_devIP='" + devIP + "'"
                        + " and EthName= '" + EthName + "' and NATIP='" + EthIP + "' and EthIP='" + StaticGlobal.FwMACandIP[StaticGlobal.firewallmac] + "');";
            }

            NATdb_operate.dboperate(sql_rule);

            SendInfo sendcmd = new SendInfo(devform);    
            return sendcmd.SendConfigInfo(configInfo);


           
        }

      public  bool ConfigDNAT(string dev_IP, string Original_DIP, string Original_dport, string Map_IP, string Map_port, bool add_delete)
        {
            this.devform.setDev_IP(dev_IP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string flag = "";string pre_rule = "";//string post_rule = "";
            string sql_rule = "";
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
                sql_rule = "INSERT INTO dnat VALUES ('" + StaticGlobal.firewallmac + "','" + Original_DIP + "','" + Original_dport + "','" + Map_IP + "','" + Map_port + "');";
            }
            else if (!add_delete)
            {
                flag = "NAT0";
                sql_rule = "DELETE FROM dnat WHERE fwmac= '" + StaticGlobal.firewallmac + "' and origin_dstIP='" + Original_DIP + "'"
                        + " and origin_dport= '" + Original_dport + "' and map_IP='" + Map_IP + "' and map_port='" + Map_port + "');";
            }
            string configrule = flag + pre_rule;                 
            SendInfo sendcmd = new SendInfo(devform);
            NATdb_operate.dboperate(sql_rule);
            return sendcmd.SendConfigInfo(configrule);                                 
        }

        public bool ClearNATRules(string dev_IP)
        {
            this.devform.setDev_IP(dev_IP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule = "iptables -t nat -F";
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);

        }
    }
}
