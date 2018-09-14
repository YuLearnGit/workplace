
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ConfigureNAT : IConfigureNAT
    {
       
        private DBOperation NATdb_operate;

        public ConfigureNAT()
        {
            this.NATdb_operate = new DBOperation();
        }

        public bool ConfigSNAT(FWDeviceForm fw_dev, string EthName, string devIP, string EthIP, bool add_delete)
        {
            string flag = ""; string configEth_bridge = ""; string configInfo = ""; string configEth_IP = "";string sql_rule = "";
            string rule = "iptables -t nat -A POSTROUTING -s " + devIP + " -o br0 -j SNAT --to-source " + fw_dev.getDev_IP();
            if (add_delete)
            {
                flag = "NAT1";
                configEth_bridge = "brctl delif br0 " + EthName;//先将网口从网桥上删除
                configEth_IP = "ifconfig " + EthName + " " + EthIP + " netmask 255.255.255.0" + " up";
                configInfo = flag + configEth_bridge + " && " + configEth_IP + " && " + rule;
                sql_rule = "INSERT INTO SNAT values "+"('"+fw_dev.getDev_IP()+ "','"+devIP+"','" + EthName + "','" + EthIP+"','"+ fw_dev.getDev_IP() + "')";
            }
           else if (!add_delete)
            {
                flag = "NAT0";
                configEth_bridge = "brctl addif br0 " + EthName;
                configEth_IP = "ifconfig " + EthName + " " + "0.0.0.0 up";
                configInfo = flag + configEth_IP + " && " + configEth_bridge + " && " + rule;
                sql_rule = "DELETE FROM SNAT where (fwIP='" + fw_dev.getDev_IP() + "' and origin_devIP='" + devIP + "'"+
                    " and EthName='" + EthName + "' and EthIP='" + EthIP + "' and NATIP='" + fw_dev.getDev_IP() + "')";
            }

            fw_dev.setDev_port(22222);
            NATdb_operate.dboperate(sql_rule);

            SendInfo sendcmd = new SendInfo(fw_dev);    
            return sendcmd.SendConfigInfo(configInfo);


           
        }


      public  bool ConfigDNAT(FWDeviceForm fw_dev, string Original_DIP, string Original_dport, string Map_IP, string Map_port, bool add_delete)
        {
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
                sql_rule = "INSERT INTO dnat values "+"('"+fw_dev.getDev_IP()+"','"+ Original_DIP + "','"+ Original_dport + "','"+ Map_IP + "','"+ Map_port + "')";
            }
            else if (!add_delete)
            {
                flag = "NAT0";
                sql_rule ="DELETE FROM DNAT where "+"(fwIP='"+fw_dev.getDev_IP()+"' and origin_dstIP='"+ Original_DIP + "'"+
                    " and origin_dport='"+ Original_dport + "' and map_IP='"+ Map_IP + "' and map_port='" + Map_port + "')";
            }
            string configrule = flag + pre_rule;
          
            fw_dev.setDev_port(22222);
            SendInfo sendcmd = new SendInfo(fw_dev);
            NATdb_operate.dboperate(sql_rule);
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
