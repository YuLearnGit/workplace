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
        public ARPProtect()
        {
            this.devform = new DeviceForm("0.0.0.0", 22222);
        }
     public  bool configARPRules(string dev_IP, string gateIP, string gateMAC, bool addDelete)
        {

            this.devform.setDev_IP(dev_IP);
            string arpRules = null;
            string rule0 = "arptables -A INPUT -i br0 --src-mac ! " + gateMAC + " -j DROP";
            string rule1 = "arptables -A INPUT -i br0 --src-ip " + gateIP + " --src-mac ! " + gateMAC + " -j DROP";
            string rule2 = "arptables -A OUTPUT --destination-mac ff:ff:ff:ff:ff:ff -j ACCEPT";
            FileOperation of = new FileOperation("C:\\CMP\\Config");
            if (addDelete)
            {
                arpRules = "ARP1" + rule0 + " && " + rule1 + " && " + rule2;
                string frule = dev_IP + " " + gateIP + " " + gateMAC;
                of.SaveRules(frule, "arp");               
            }
                
            else if (!addDelete)
            {
                arpRules = "ARP0" + rule0 + " && " + rule1 + " && " + rule2;
                string frule = dev_IP + " " + gateIP + " " + gateMAC;
                of.DeleteRules(frule, "arp");
            }               
          
            SendInfo send = new SendInfo(devform);
            return send.SendConfigInfo(arpRules);           
        }
    }
}
