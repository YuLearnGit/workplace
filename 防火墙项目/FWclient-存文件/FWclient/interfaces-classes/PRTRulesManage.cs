using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class PRTRulesManage : IPRTRulesManage
    {
        private DeviceForm devform;
        FileOperation of = new FileOperation("C:\\CMP\\Config");

        public PRTRulesManage()
        {
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }

        public bool DefaultRouteConfig(string devIP, bool add_del_flag, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule=string.Empty;
            if (add_del_flag)
            {
                rule = "PRT1route add default ";
                string frule = devIP +  " default-route " + Iface + " " + gateway;
                of.SaveRules(frule, "prt");
            }
            else
            {
                rule = "PRT0route del default ";
                string frule = devIP + " default-route " + Iface + " " + gateway;
                of.DeleteRules(frule, "prt");
            }

            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool HostRouteConfig(string devIP, bool add_del_flag, string host, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule;
            if (add_del_flag)
            {
                rule = "PRT1route add -host " + host;
                string frule =devIP + " host-route " + host +" "+ Iface + " " + gateway;
                of.SaveRules(frule, "prt");
            }
            else
            {
                rule = "PRT0route del -host " + host;
                string frule = devIP + " host-route " + host + " " + Iface + " " + gateway;
                of.DeleteRules(frule, "prt");
            }

            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;

            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool NetRouteConfig(string devIP, bool add_del_flag, string net, string netmask, string Iface, string gateway)
        {
            this.devform.setDev_IP(devIP);
            if (devform.getDev_IP() == "0.0.0.0")
                return false;

            string rule;
            if (add_del_flag)
            {
                rule = "PRT1route add -net " + net;
                string frule = devIP  + " net-route " + " " + net + " " + netmask + " " + Iface + " " + gateway;
                of.SaveRules(frule, "prt");
            }
            else
            {
                rule = "PRT0route del -net " + net;
                string frule = devIP + " net-route " + " " + net + " " + netmask + " " + Iface + " " + gateway;
                of.DeleteRules(frule, "prt");
            }

            if (netmask != "")
                rule = rule + " netmask " + netmask;
            if (Iface != "")
                rule = rule + " dev " + Iface;
            if (gateway != "")
                rule = rule + " gw " + gateway;

            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }
    }
}
