//#define debug

using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharpPcap;
using PacketDotNet;


namespace FireWall
{
    /// <summary>
    /// 向防火墙设备发送信息
    /// </summary>
    class SendInfo
    {
        private  DeviceForm devform;
        private static bool config_info_confirm;
        /*初始化设备*/
        public SendInfo(DeviceForm devform)
        {
            this.devform = devform;
        }

        /// <summary>
        /// 发送配置信息
        /// </summary>
        /// <param name="cmd">需要配置的规则</param>
        public bool  SendConfigInfo(string cmd)
        {
            config_info_confirm = false;

            byte[] head = { 0x0f, 0x0e, 0x0d, 0x0c, 0x0b, 0x0a};   //自定义数据包包头
            byte[] body = Encoding.ASCII.GetBytes(cmd + "*");
            byte[] data = head.Concat(body).ToArray();


            ASCIIEncoding encoding = new ASCIIEncoding();
            string yucon = encoding.GetString(data, 0, data.Length);
            Console.WriteLine("{0}",yucon);

            UdpClient client =null;
            IPAddress remoteIP = IPAddress.Parse(devform.getDev_IP());
            int remotePort = devform.getDev_port();
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

#if debug
            Console.WriteLine("start sending:");
#endif
            client = new UdpClient();
            client.Send(data, data.Length, remotePoint);

            /* 监听无IP配置是否成功返回消息  */
            UdpClient listener = new UdpClient(30333);
            listener.Client.ReceiveTimeout=10000;
            IPEndPoint groupEP = null;
            //DateTime beforDT = System.DateTime.Now;
            try
            {
                while (!config_info_confirm)
                {
                    byte[] content = listener.Receive(ref groupEP);
                    string con = Encoding.Default.GetString(content);
                    if (con == "success")
                    {                  
                        config_info_confirm = true;
                    }
                    if (con == "fail")
                        config_info_confirm = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                config_info_confirm = false;
            }
            finally
            {
                listener.Close();
            }

            client.Close();
#if debug
            Console.WriteLine("send successfully!");
#endif
           
            return config_info_confirm;
        }

        /// <summary>
        /// 发送扫描设备的数据
        /// </summary>
        public void SendCheckInfo()
        {

            string mac = GetLocalMacAddr.GetMacAddr();  //获取本机MAC地址    
            //string mac = "40-8D-5C-08-53-A7";
            byte[] head = { 0x0f, 0x0e, 0x0d, 0x0c, 0x0b, 0x0a };   //自定义数据包包头
            byte[] body = Encoding.ASCII.GetBytes(mac + "*");
            byte[] data = head.Concat(body).ToArray();

            UdpClient client = null;
            IPAddress remoteIP = IPAddress.Parse(devform.getDev_IP());
            int remotePort = devform.getDev_port();
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            client = new UdpClient(); 
#if debug
            Console.WriteLine("start sending:");
#endif
            client.Send(data, data.Length, remotePoint);
            client.Close();
#if debug
            Console.WriteLine("send successfully!");
#endif     
        }

   }
}
