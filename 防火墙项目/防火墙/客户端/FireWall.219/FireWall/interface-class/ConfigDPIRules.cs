

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace FireWall
{
    /// <summary>
    /// 定义防火墙规则
    /// </summary>
    class ConfigDPIRules:IConfigRules
    {
       // private String default_rules = "iptables -P FORWARD  DROP";
        private DeviceForm devform;
        private DBOperation DPIdb_operate;

        public ConfigDPIRules(DeviceForm devform)
        {
            this.devform = devform;
            this.DPIdb_operate = new DBOperation();
        }      

        public bool ConfigModbusTcpRules(ModbusTcpRulesForm mtrf,bool log_flag, bool add_delete)
        {
           // RulesDataProcess.ModbusTcpRulesDataProcess(mtrf);

            String dpi_pro = "modbusTcp";
            string flag=null; string dpi_rules_from_master_to_slave0 = null;string sql_rule = null; ;
      
            if (mtrf.getSrc_IP() == "any" & mtrf.getDst_IP() == "any")
            {
                dpi_rules_from_master_to_slave0 = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() + ":" + mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " " + "--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + "DROP";
            }
            else if (mtrf.getSrc_IP() == "any" & mtrf.getDst_IP() != "any")
            {
                dpi_rules_from_master_to_slave0 = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-d" + " " + mtrf.getDst_IP() + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() + ":" + mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " " + "--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + "DROP";

            }
            else if (mtrf.getSrc_IP() != "any" & mtrf.getDst_IP() == "any")
            {
                dpi_rules_from_master_to_slave0 = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-s " + mtrf.getSrc_IP() + " "  + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() + ":" + mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " " + "--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + "DROP";
            }
            else
            {

                dpi_rules_from_master_to_slave0 = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-s " + mtrf.getSrc_IP() + " " + "-d" + " " + mtrf.getDst_IP() + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() + ":" + mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " " + "--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + " DROP";
            }
            //string dpi_rules_from_master_to_slave1 = "iptables -A FORWARD -m state --state ESTABLISHED,RELATED -j ACCEPT";
            string dpi_rules_from_master_to_slave_log= "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-s " + mtrf.getSrc_IP() + " " + "-d" + " " + mtrf.getDst_IP() + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() +":"+ mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " "+"--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "DROP&modbusTCP&data_illegal " + "\"";
           
            if (add_delete == true)
            {
                flag = "DPI1";
                sql_rule = "insert into modbustcp values ('" + StaticGlobal.firewallmac + "','" + dpi_pro + "','" + mtrf.getSrc_IP() + "','" + mtrf.getDst_IP() + "','" 
                    + mtrf.getMin_addr() + "','" + mtrf.getMax_addr() + "','" + mtrf.getMin_data() + "','" + mtrf.getMax_data() + "','" + mtrf.getfunc() + "','" + log_flag + "');";
            }

            else if (add_delete == false)
            {
                flag = "DPI0";
                sql_rule = "delete from modbustcp where fw_mac= '" + StaticGlobal.firewallmac + "' and protocol = '" + dpi_pro + "' and source = '"
                    + mtrf.getSrc_IP() + "' and destination = '" + mtrf.getDst_IP() + "' and coiladdressstart = '" + mtrf.getMin_addr() + "' and coiladdressstart = '" 
                    + mtrf.getMax_addr() + "'and minspeed ='" + mtrf.getMin_data() + "' and maxspeed ='" + mtrf.getMax_addr() + "' and functioncode = '" + mtrf.getfunc() + "' and log = '" + log_flag + "';";
            }
            string rule = flag + dpi_rules_from_master_to_slave_log + " && " + dpi_rules_from_master_to_slave0;
            DPIdb_operate.dboperate(sql_rule);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ConfigOPCRules(OPCRulesForm orf,bool log_flag, bool add_delete)
        {
            string flag = null;string sql_rule = null;

            String opc_rules_from_client_to_server0 = "iptables -A FORWARD -p tcp -s " + orf.getSrc_IP() + " -d " + orf.getDst_IP() + " --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
            String opc_rules_from_client_to_server1 = "iptables -A FORWARD -p tcp -s "+ orf.getDst_IP() +" -d "+ orf.getSrc_IP();
            String opc_rules_from_client_to_server_log= "iptables -A FORWARD -p tcp -s " + orf.getSrc_IP() + " -d " + orf.getDst_IP() + " --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix "+ "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
            //String opc_rules_from_server_to_client = "iptables -A FORWARD -p tcp -s " + orf.getDst_IP() + " -d " + orf.getSrc_IP() + " --sport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
            if (add_delete == true)
            {
                flag = "DPI1";
                sql_rule = "INSERT INTO OPC values "+"("+devform.getDev_IP()+"','"+orf.getDst_IP()+"','"+orf.getSrc_IP()+"','ACCEPT','"+log_flag+"')";
            }

            else if (add_delete == false)
            {
                flag = "DPI0";
                sql_rule ="DELETE FROM OPC where (dev_IP='"+devform.getDev_IP()+ "' and dst_IP='" + orf.getDst_IP() + "' and src_IP='" + orf.getSrc_IP() + "' and method='ACCEPT')";
            }

            string rule = flag + opc_rules_from_client_to_server_log + " && " + opc_rules_from_client_to_server0 + " && " + opc_rules_from_client_to_server1;
            DPIdb_operate.dboperate(sql_rule);
            SendInfo sendcmd = new SendInfo(devform);                  
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ConfigDNP3Rules(DNP3RulesForm dnp3rf,bool log_flag, bool add_delete)
        {
            string flag = null;string sql_rule = null;

            String dnp3_rules_from_client_to_server_new = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state NEW -j ACCEPT";
           // String dnp3_rules_from_server_to_client_new = "iptables -A FORWARD -p tcp -s " + dnp3rf.getDst_IP() + " -d " + dnp3rf.getSrc_IP() + " --sport 20000 -m state --state NEW -j ACCEPT";
            String dnp3_rules_from_client_to_server_established = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state ESTABLISHED -j ACCEPT";
            String dnp3_rules_from_client_to_server_back = "iptables -A FORWARD -p tcp -d " + dnp3rf.getSrc_IP() + " -s " + dnp3rf.getDst_IP();
            string dnp3_rules_from_client_to_server_log = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&DNP3&ESTABLISHED " + "\"";
            // String dnp3_rules_from_server_to_client_established = "iptables -A FORWARD -p tcp -s " + dnp3rf.getDst_IP() + " -d " + dnp3rf.getSrc_IP() + " --sport 20000 -m state --state ESTABLISHED -j ACCEPT";
            if (add_delete == true)
            {
                flag = "DPI1";
                sql_rule ="INSERT INTO DNP3 values "+"(NULL,'"+devform.getDev_IP()+"','"+ dnp3rf.getDst_IP() + "','"+ dnp3rf.getSrc_IP() + "','ACCEPT','"+log_flag+"')";
            }

            else if (add_delete == false)
            {
                flag = "DPI0";
                sql_rule ="DELETE FROM DNP3 where (dev_IP='"+ devform.getDev_IP() + "' and dst_IP='" + dnp3rf.getDst_IP() + "' and src_IP='" + dnp3rf.getSrc_IP() + "' and method='ACCEPT')";
            }
            string rule = flag + dnp3_rules_from_client_to_server_log + " && " + dnp3_rules_from_client_to_server_new + " && " + dnp3_rules_from_client_to_server_established;
            DPIdb_operate.dboperate(sql_rule);
            SendInfo sendcmd = new SendInfo(devform);      
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ClearAllRules()
        {
            string rule = "iptables -P FORWARD ACCEPT && iptables -F && iptables -X && iptables -Z && iptables-restore</etc/iptables.up.rules";

            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
            //sendcmd.SendConfigInfo("kill 'ps -e | grep snort | awk '{print $1}' |head -1"+"!");
        }

    }
}
