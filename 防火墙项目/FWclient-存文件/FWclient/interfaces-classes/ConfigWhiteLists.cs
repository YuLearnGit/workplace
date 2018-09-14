
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
        FileOperation of = new FileOperation("C:\\CMP\\Config");
        public ConfigWhiteLists()
        {
            this.devform = new DeviceForm("0.0.0.0",22222);
        }

        public bool ChangeWhiteLists(string dev_IP, string dst_IP, string src_IP,  string dst_port, string src_port, bool log_record, bool add_delete)
        {
            this.devform.setDev_IP(dev_IP);
            WhiteLists lists = new WhiteLists();
            lists.setIPAndPort(dst_IP, src_IP, dst_port, src_port);

            string flag = null;
    
            string whiteList_from_client_to_server0 = "iptables -A FORWARD -p tcp -s " + lists.getsrc_IP() + " -d " + lists.getdst_IP() + " --sport " + lists.getsrc_port()
                + " --dport " + lists.getdst_port() + " -j ACCEPT ";
            // string whiteList_from_client_to_server1 = "iptables -A FORWARD -p tcp -d" + wl.getSrc_IP() + "--sport" + wl.getPort();

            if (add_delete)
            {
                flag = "WHL1";
                string frule = dev_IP+" "+ dst_IP + " "+ src_IP + " "+ dst_port + " "+ src_port;
                of.SaveRules(frule, "whl");
            }

            else
            {
                flag = "WHL0";
                string frule = dev_IP + " " + dst_IP + " " + src_IP + " " + dst_port + " " + src_port;
                of.DeleteRules(frule, "whl");
            }

            string changewl = flag + whiteList_from_client_to_server0;
            //DeviceForm devform = new DeviceForm(dev_IP, 22222);
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(changewl);                           

        }
    }
}
