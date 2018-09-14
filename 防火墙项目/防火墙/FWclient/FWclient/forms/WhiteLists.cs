using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 白名单功能
    /// </summary>
    class WhiteLists
    {
        private string dst_IP;
        private string src_IP;
        private string dst_port;
        private string src_port;

        public void setIPAndPort(string dst_IP,string src_IP,string dst_port, string src_port)
        {
            this.dst_IP = dst_IP;
            this.src_IP = src_IP;
            this.dst_port = dst_port;
            this.src_port = src_port;
        }

        public string getdst_IP()
        {
            return dst_IP;
        }

        public string getsrc_IP()
        {
            return src_IP;
        }

        public string getdst_port()
        {
            return dst_port;
        }

        public string getsrc_port()
        {
            return src_port;
        }

    }
}
