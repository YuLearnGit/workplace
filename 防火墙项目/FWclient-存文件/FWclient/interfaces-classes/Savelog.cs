using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace FWclient
{
    class Savelog:IReceiveLog
    {
        FileOperation of = new FileOperation("C:\\CMP\\log");
        Queue<string> real_time = new Queue<string>();
        static int listenPort = 8000;

        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = null;
     public void Save_DisplayLog(bool start)
        {
            if (start)
            {
                Thread save = new Thread(new ThreadStart(SaveData));
                save.Start();
            }
        }
      public string queue_func()
        {
            if (real_time.Count == 0)
                Thread.Sleep(2000);
            string Display_log = real_time.Dequeue();
            return Display_log;
        }

        void SaveData()
        {
            string Defaultpath = "C:\\CMP\\log";

            /*建立月份转换字典*/
            Dictionary<string, string> month = new Dictionary<string, string>();
            month.Add("Jan", "01"); month.Add("Feb", "02"); month.Add("Mar", "03"); month.Add("Apr", "04"); month.Add("May", "05"); month.Add("Jun", "06");
            month.Add("Jul", "07"); month.Add("Aug", "08"); month.Add("Sep", "09"); month.Add("Oct", "10"); month.Add("Nov", "11"); month.Add("Dec", "12");
            string year = DateTime.Now.ToString("yyyy");
            try
            {
                while (true)
                {
                    /* 监听并接收日志 */
                    byte[] content = listener.Receive(ref groupEP);
                    string log = Encoding.Default.GetString(content);
                    FileStream fs = new FileStream(@);
                }
            }catch(Exception e)
            {
                Console.WriteLine("error:" + e.ToString());
            }
            finally
            {
                listener.Close();
            }
         }
    }
}
