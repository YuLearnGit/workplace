
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ConfigWhiteLists:IConfigWhiteLists
    {
        private DeviceForm devform;
        private DBOperation LISTdb_operate;

        public ConfigWhiteLists()
        {
            this.devform = new DeviceForm("0.0.0.0",22222);
            this.LISTdb_operate = new DBOperation();
        }

        public bool ChangeWhiteLists(string dev_IP, string dst_IP, string src_IP,  string dst_port, string src_port, bool log_record, bool add_delete)
        {
            this.devform.setDev_IP(dev_IP);
            WhiteLists lists = new WhiteLists();
            lists.setIPAndPort(dst_IP, src_IP, dst_port, src_port);

            string flag = null; string sql_rule = "";
    
            string whiteList_from_client_to_server0 = "iptables -A FORWARD -p tcp -s " + lists.getsrc_IP() + " -d " + lists.getdst_IP() + " --sport " + lists.getsrc_port()
                + " --dport " + lists.getdst_port() + " -j ACCEPT ";
            // string whiteList_from_client_to_server1 = "iptables -A FORWARD -p tcp -d" + wl.getSrc_IP() + "--sport" + wl.getPort();

            if (add_delete)
            {
                flag = "DPI1";
                sql_rule = "INSERT INTO whitelist values "+"('"+dev_IP+"','"+ dst_IP + "','"+ src_IP + "','"+ dst_port + "','"+ src_port + "')";
            }

            else
            {
                flag = "DPI0";
                sql_rule = "DELETE FROM whitelist where (dev_IP='" + dev_IP + "' and dst_IP='" + dst_IP + "' and src_IP='" + src_IP + "' and dst_port='" + dst_port + "' and src_port='" + src_port + "')";
            }

            string changewl = flag + whiteList_from_client_to_server0;
            LISTdb_operate.dboperate(sql_rule);

            //DeviceForm devform = new DeviceForm(dev_IP, 22222);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(changewl);                           

        }
    }
}
