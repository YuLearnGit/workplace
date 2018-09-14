//#define debug

using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharpPcap;
using PacketDotNet;
using FWclient;
using System.Security.Cryptography;
using System.IO;



namespace FWclient
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
            UdpClient client =null;
            IPAddress remoteIP = IPAddress.Parse(devform.getDev_IP());
            int remotePort = devform.getDev_port();
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            //CaptureDeviceList devices = CaptureDeviceList.Instance;
            //// If no devices were found print an error
            //if (devices.Count < 1)
            //{
            //    // Console.WriteLine("No devices were found on this machine");
            //    return false;
            //}
            //ICaptureDevice device = devices[0];
            //device.OnPacketArrival += new PacketArrivalEventHandler(configDev_OnPacketArrival);
            //int readTimeoutMilliseconds = 1000;
            //device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            //string filter = "ip and udp";
            //device.Filter = filter;
            //device.StartCapture();

#if debug
            Console.WriteLine("start sending:");
#endif
            client = new UdpClient();
            client.Send(data, data.Length, remotePoint);


            UdpClient listener = new UdpClient(30333);
            listener.Client.ReceiveTimeout =10000;
            IPEndPoint groupEP = null;
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
        /// 异或的加密和解密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string BCutEncrypt(string str)
        {
            char emblem = '\n';//密钥
            StringBuilder buffer = new StringBuilder();
            char[] chars = str.ToCharArray();
            foreach (char ch in chars)
            {
                char temp = (char)(ch ^ emblem);
                buffer.Append(temp);
            }
            return buffer.ToString();
        }

        //private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//密钥
        string Keys = "1234567890ABCDEF";
        ///<summary>  
        ///DES加密字符串  
        ///</summary>  
        ///<param name="str">待加密的字符串</param>  
        ///<param name="key">加密密钥,要求为8位</param>  
        ///<returns>加密成功返回加密后的字符串，失败返回源字符串</returns>  
        public string EncryptDES(string str, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(Keys);
                //byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
                DESCryptoServiceProvider myDES = new DESCryptoServiceProvider();
                myDES.Mode = CipherMode.CBC;
               // myDES.Padding = PaddingMode.Zeros;          
                MemoryStream MStream = new MemoryStream();
                CryptoStream CStream = new CryptoStream(MStream, myDES.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                CStream.Write(inputByteArray, 0, inputByteArray.Length);
                CStream.FlushFinalBlock();
                return Convert.ToBase64String(MStream.ToArray());
            }
            catch
            {
                return str;
            }
        }

        ///<summary>  
        ///DES解密字符串  
        ///</summary>  
        ///<param name="str">待解密的字符串</param>  
        ///<param name="key">解密密钥,要求为8位,和加密密钥相同</param>  
        ///<returns>解密成功返回解密后的字符串，失败返源字符串</returns>  
        public string DecryptDES(string str, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Encoding.UTF8.GetBytes(Keys);
                //byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(str);
                DESCryptoServiceProvider myDES = new DESCryptoServiceProvider();
                MemoryStream MStream = new MemoryStream();
                CryptoStream CStream = new CryptoStream(MStream, myDES.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                CStream.Write(inputByteArray, 0, inputByteArray.Length);
                CStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(MStream.ToArray());
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] text, string password, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            //rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] keyBytes = new byte[32];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = ivBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = text;
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return cipherBytes;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] text, string password, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            //rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = text;
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] keyBytes = new byte[32];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = ivBytes;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return plainText;
        }



        /// <summary>
        /// 发送扫描设备的数据
        /// </summary>
        public void SendCheckInfo()
        {
            //string mac = "40-8D-5C-08-53-A7";
            string mac = GetLocalMacAddr.GetMacAddr();  //获取本机MAC地址
            //Console.WriteLine("local mac is {0}", mac);
            string str = mac + "*";
            //string key = "0123456789ABCDEF0123456789ABCDEF";
            //string iv = "1234567890ABCDEF";
            //// string secret = EncryptDES(str,key);//数据加密
            //byte[] text = Encoding.UTF8.GetBytes(str);
            //byte[] secret = AESEncrypt(text, key, iv);
            //byte[] reverse = AESDecrypt(secret, key, iv);
            byte[] head = { 0x0f, 0x0e, 0x0d, 0x0c, 0x0b, 0x0a };   //自定义数据包包头
            byte[] body = Encoding.ASCII.GetBytes(str);
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

      /*  private static void configDev_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            //var time = e.Packet.Timeval.Date;
            //var len = e.Packet.Data.Length;

            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            UdpPacket udpPacket = UdpPacket.GetEncapsulated(packet);

            if (udpPacket != null)
            {
                var ipPacket = (PacketDotNet.IpPacket)udpPacket.ParentPacket;
                //System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                int srcPort = udpPacket.SourcePort;
                int dstPort = udpPacket.DestinationPort;
#if debug
                Console.WriteLine("端口检查");
#endif             
                    if (srcPort == 30332 && dstPort == 30333)
                    {
                        byte[] payload = udpPacket.PayloadData;
                        string content = System.Text.Encoding.Default.GetString(payload); //确认包的内容
                        if (content == "success")
                        {
                            config_info_confirm = true;
                        Console.WriteLine("{0}",config_info_confirm.ToString());
                        }
                        if (content == "fail")
                        {
                            config_info_confirm = false;
                        }
                    }

                    if(srcPort==30334 & dstPort==30335)
                {
                    byte[] payload = udpPacket.PayloadData;
                    string content = System.Text.Encoding.Default.GetString(payload); //确认包的内容
                    FWIPinfo = content;
                    Console.WriteLine("{0}",FWIPinfo);
                }
                
            }
        }*/
    }
}
