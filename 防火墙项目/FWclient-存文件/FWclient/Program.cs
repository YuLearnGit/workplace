#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.SqlClient;
namespace FWclient
{
    /// <summary>
    /// 接口测试
    /// </summary>
    class Test
    {
        static void Main(string[] args)
        {
            //IAPCRulesManage apc = new APCRulesManage();
            //apc.ApplicationProtocolControl("172.16.10.1", "http", true);
            //IARPProtect arp = new ARPProtect();
            //arp.configARPRules("172.16.10.1", "172.16.10.2", "172.16.10.254", false);
            //ICNCRulesManage cnc = new CNCRulesManage();
            //cnc.DelCNCRules("172.16.10.1", true, 52, "172.16.10.2", "172.16.10.3", "502", "205");
            DeviceForm dev = new DeviceForm("172.16.10.1", 502);
            FWDeviceForm fw = new FWDeviceForm("172.16.10.2",22222,"ff:ff:ff:ff:ff:ff");
            //IConfigRules dpi = new ConfigDPIRules(dev);
            //ModbusTcpRulesForm modbus = new ModbusTcpRulesForm();
            //OPCRulesForm opc = new OPCRulesForm();
            //DNP3RulesForm dnp = new DNP3RulesForm();
            ////dpi.ConfigModbusTcpRules(modbus,true,false);
            ////dpi.ConfigOPCRules(opc,true,false);
            //dpi.ConfigDNP3Rules(dnp,true,false);
            IConfigureNAT nat = new ConfigureNAT();
            //nat.ConfigSNAT(fw,"eth0","192.168.10.3","172.16.10.3",false);
            //nat.ConfigDNAT(fw,"192.16.10.1","55","172.16.10.1","55",false);
            IConfigWhiteLists whl = new ConfigWhiteLists();
            //whl.ChangeWhiteLists("172.16.10.1", "172.16.10.2", "172.16.10.254","205","502",true,false);
            IPRTRulesManage prt = new PRTRulesManage();
            //prt.DefaultRouteConfig("172.16.10.1",false,"eth0","172.16.10.254");
            //prt.HostRouteConfig("172.16.10.1",false,"172.16.10.2","eth0","172.16.10.254");
            //prt.NetRouteConfig("172.16.10.1", false, "172.16.10.2", "255.255.255.0","eth0", "172.16.10.254");
            ISTDRulesManage std = new STDRulesManage();
            std.DelSTDRules("172.16.10.1",true,"tcp", "172.16.10.2", "172.16.10.3","22222","33333");
            //            IDevicesCheck devConfirm = new DevicesCheck();
            //            List<FWDeviceForm> fws = devConfirm.CheckDevices("172.16.10.9", "172.16.10.10");

            //            Console.WriteLine("打印扫描结果 :");
            //            IReceiveLog log = new ReceiveLog();
            //            log.Save_DisplayLog(true);
            //            foreach (FWDeviceForm fw in fws)
            //            {
            //                string fwip = fw.getDev_IP();
            //                string fwmac = fw.getDev_MAC();
            //                List<ProtecDeviceForm> protecDev_list = fw.getProtecDev_list();

            //                Console.WriteLine("防火墙设备IP : {0} 防火墙设备MAC : {1}", fwip, fwmac);

            //                Console.WriteLine("关联的受保护设备 :");

            //                if (fwip != "0.0.0.0")
            //                {

            //                    foreach (var item in protecDev_list)
            //                    {
            //                        string dev_type = item.getDev_type();
            //                        Console.WriteLine("IP {0}   MAC {1} 设备制造商 {2}", item.getDev_IP(), item.getDev_MAC(), dev_type);

            //                    }
            //                }
            //                else Console.WriteLine("防火墙为无IP模式");
            //            }

            //            //IARPProtect tes = new ARPProtect();
            //            //tes.configARPRules("172.16.10.9", "172.16.10.254", "08:57:00:e5:87:a7", true);

#if debug
            Console.ReadLine();
#endif
        }     
    }
}
