#define debug

using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FireWall
{
    class DevicesCheck : IDevicesCheck
    {
        private static List<FWDeviceForm> fw_list = new List<FWDeviceForm>(); //单次扫描到的防火墙设备
        private static List<string> fwMAC_list = new List<string>(); //单次扫描到的防火墙设备的MAC
                                                                     // private static int  fw_num = 1; //防火墙标识计数      
        static int listenPort = 30331;//监听的端口
        string confirm = null;
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = null;
        public List<FWDeviceForm> CheckDevices(string start_IP, string end_IP)
        {
            fw_list.Clear();
            fwMAC_list.Clear();

            Thread listen = new Thread(new ThreadStart(listenCheckResult));
            listen.Start();
            IDevicesScan devScan = new DevicesScan();
            int ip_num = devScan.ScanDevice(start_IP, end_IP);
            Thread.Sleep(ip_num * 5000);


            listen.Abort();
            listener.Close();
            return fw_list;
        }

        /* 监听扫描返回数据包端口 */
        public void listenCheckResult()
        {
            while (true)
            {
                byte[] content = listener.Receive(ref groupEP);
                confirm = Encoding.Default.GetString(content);
                if ((confirm.IndexOf("firedeviceConfirm")) != -1)
                {
#if debug
                    Console.WriteLine("捕获到返回信息！！！");
#endif
                    string[] sArray_IP_MAC = confirm.Split('&');
                    string fw_IP = sArray_IP_MAC[0];    //防火墙IP
                    string dev_mac = sArray_IP_MAC[1];  //受保护设备MAC
                    string fw_mac = sArray_IP_MAC[2];   //防火墙MAC
                    string dev_IP = sArray_IP_MAC[4];   //受保护设备IP
                    //if (dev_mac != "")
                    //{
                        if (fw_IP != "0.0.0.0" && (fwMAC_list.Contains(fw_mac)))    //如果已经存在
                        {
                            foreach (FWDeviceForm fwdev in fw_list)
                            {
                                if (fwdev.getDev_MAC() == fw_mac)
                                {
                                    if (!fwdev.getProtecDevIP_list().Contains(dev_IP))
                                    {
                                        ProtecDeviceForm protecDev = new ProtecDeviceForm(dev_IP, dev_mac);
                                        fwdev.addProtecDev(protecDev);
                                        fwdev.addProtecDevIP(dev_IP);
                                    }
                                }
                            }
                        }
                        else
                        {
                            FWDeviceForm fw_dev = new FWDeviceForm(fw_IP, 22222, fw_mac);
                            fw_dev.addProtecDev(new ProtecDeviceForm(dev_IP, dev_mac));
                            fw_dev.addProtecDevIP(dev_IP);
                            fwMAC_list.Add(fw_mac);
                            fw_list.Add(fw_dev);
                        }
                    //}

#if debug
                    Console.WriteLine("保存设备信息！！！");
#endif
                }
                else Console.WriteLine("未扫描到设备");
            }
        }
    }
}