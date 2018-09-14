using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// opc规则
    /// </summary>
    class OPCRulesForm
    {
        private string dst_IP;
        private string src_IP;

        public void setDst_IPAndSrc_IP(string dst_IP, string src_IP)
        {
            this.dst_IP = dst_IP;
            this.src_IP = src_IP;
        }

        public string getDst_IP()
        {
            return dst_IP;
        }

        public string getSrc_IP()
        {
            return src_IP;
        }
    }
}
