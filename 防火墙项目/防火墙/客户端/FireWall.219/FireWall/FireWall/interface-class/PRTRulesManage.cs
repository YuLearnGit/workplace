using FireWall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class PRTRulesManage : IPRTRulesManage
    {
        private DeviceForm devform;
        private DBOperation db_operate;

        public PRTRulesManage()
        {
            this.db_operate = new DBOperation();
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }

        public bool DefaultRouteConfig(string devIP, bool add_del_flag, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule;
            string sql_str;
            if (add_del_flag)
            {
                rule = "PRT1route add default ";
                sql_str = "INSERT INTO prt VALUES " + "('" + StaticGlobal.firewallmac + "'," + "'默认路由','" + "" + "','" + "" + "','" + "" + "','" + Iface + "','" + "" + "','"  + gateway + "')";
            }
            else
            {
                rule = "PRT0route del default ";
                sql_str = "DELETE FROM prt WHERE (fwmac='" + StaticGlobal.firewallmac + "' and route_type='默认路由'" + " and Iface='" + Iface + "' and gateway='" + gateway  + "')";
            }

            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;

            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool HostRouteConfig(string devIP, bool add_del_flag, string host, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule;
            string sql_str;
            if (add_del_flag)
            {
                rule = "PRT1route add -host " + host;
                sql_str = "INSERT INTO prt VALUES " + "('" + StaticGlobal.firewallmac + "'," + "'主机路由','" + host + "','" + "" + "','" + "" + "','" + Iface + "','" + "" + "','" + gateway + "')";
            }
            else
            {
                rule = "PRT0route del -host " + host;
                sql_str = "DELETE FROM prt WHERE (fwmac='" + StaticGlobal.firewallmac + "' and route_type='主机路由'"+" and host='" + host + "' and Iface='" + Iface + "' and gateway='" + gateway + "')";
            }

            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;

            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool NetRouteConfig(string devIP, bool add_del_flag, string net, string netmask, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule;
            string sql_str;
            if (add_del_flag)
            {
                rule = "PRT1route add -net " + net;
                sql_str = "INSERT INTO prt VALUES " + "('" + StaticGlobal.firewallmac + "'," + "'网络路由','" + "" + "','" + net + "','" + netmask + "','" + Iface + "','" + "" + "','" + gateway + "')";
            }
            else
            {
                rule = "PRT0route del -net " + net;
                sql_str = "DELETE FROM prt WHERE (fwmac='" + StaticGlobal.firewallmac + "' and route_type='网络路由'"+" and net='" + net + "' and netmask='" + netmask + "' and Iface='" + Iface + "' and gateway='" + gateway + "')";
            }

            if (netmask != "")
                rule = rule + " netmask " + netmask;
            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;

            db_operate.dboperate(sql_str);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
