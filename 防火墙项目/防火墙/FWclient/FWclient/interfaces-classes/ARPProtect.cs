using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ARPProtect:IARPProtect
    {
        private DeviceForm devform;
        private DBOperation dboperate;
        public ARPProtect()
        {
            this.devform = new DeviceForm("0.0.0.0", 22222);
            this.dboperate = new DBOperation();
        }
     public  bool configARPRules(string dev_IP, string gateIP, string gateMAC, bool addDelete)
        {

            this.devform.setDev_IP(dev_IP);
            string fwmac = dboperate.getFwMAC(dev_IP);
            bool result = false;
            string arpRules = null;string sqlRule = null;
            string rule0 = "arptables -A INPUT -i br0 --src-mac ! " + gateMAC + " -j DROP";
            string rule1 = "arptables -A INPUT -i br0 --src-ip " + gateIP + " --src-mac ! " + gateMAC + " -j DROP";
            string rule2 = "arptables -A OUTPUT --destination-mac ff:ff:ff:ff:ff:ff -j ACCEPT";
            if (addDelete)
            {
                arpRules = "ARP1" + rule0 + " && " + rule1 + " && " + rule2;               
               sqlRule = "INSERT INTO arp VALUES('"+fwmac+"','"+gateIP+"','"+gateMAC + "')";
            }
                
            else if (!addDelete)
            {
                arpRules = "ARP0" + rule0 + " && " + rule1 + " && " + rule2;
                sqlRule = "DELETE FROM arp where fwMAC='" + fwmac + "' and gateIP='" + gateIP + "' and gateMAC='" + gateMAC+"'";
            }               
          
            SendInfo send = new SendInfo(devform);
            send.SendConfigInfo(arpRules);
            dboperate.dboperate(sqlRule);
            return result;
        }
    }
}
