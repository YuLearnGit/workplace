using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// modbusTcp规则
    /// </summary>
    class ModbusTcpRulesForm
    {
        private String dst_IP;
        private String src_IP;
        private String Min_addr;
        private String Max_addr;
        private int Min_data;
        private int Max_data;
        private int func;
       // private string[] function_code_select;
        private bool value_select = false;

        public bool getValue_select()
        {
            return value_select;
        }

        public string getDst_IP()
        {
            return dst_IP;
        }

        public string getSrc_IP()
        {
            return src_IP;
        }

        public String getMin_addr()
        {
            return Min_addr;
        }

        public String getMax_addr()
        {
            return Max_addr;
        }

        public int getfunc()
        {
            return func;
        }

        public int getMin_data()
        {
            return Min_data;

        }
        public int getMax_data()
        {
            return Max_data;

        }
        //public string [] getFunction_code_select()
        //{
        //    return function_code_select;
        //}

        public void setIP_Addr_Funcode(string dst_IP, string src_IP, String Min_addr, String Max_addr,int func,int Min_data,int Max_data)
        {
            this.dst_IP = dst_IP;
            this.src_IP = src_IP;
            this.Min_addr = Min_addr;
            this.Max_addr = Max_addr;
            this.func = func;
            this.Min_data = Min_data;
            this.Max_data = Max_data;
    
            //if (function_code_select[0] != null)
            //{
            //    this.function_code_select = function_code_select;
            //    value_select = true;
            //}
            //else
            //{
            //    value_select = false;
            //}
        }
    }
}
