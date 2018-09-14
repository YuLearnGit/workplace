//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FireWall
//{
//    /// <summary>
//    /// 该类用来处理modbusTcp规则中被禁止的功能码
//    /// </summary>
//    class RulesDataProcess
//    {
//        public static void ModbusTcpRulesDataProcess(ModbusTcpRulesForm mtrf)
//        {
//            int[] function_code_selected_int = new int[32];
//            string[] function_code = mtrf.getFunction_code_select();
//            long lfc_flag = 0;
//            long hfc_flag = 0;

//            if (mtrf.getValue_select())
//            {
//                int code_number = function_code.Length;
//                for (int trs_count = 0; trs_count < code_number; trs_count++)
//                {
//                    function_code_selected_int[trs_count] = Int32.Parse(function_code[trs_count]);
//                    //Console.WriteLine("int funcode is {0}", function_code_selected_int[trs_count]);
//                }

//                if (code_number > 32)
//                {
//                    code_number = 32;
//                }

//                for (int count = 0; count < code_number; count++)
//                {
//                    if (function_code_selected_int[count] <= 64)
//                    {
//                        lfc_flag = lfc_flag | ((long)1 << (function_code_selected_int[count] - 1));
//                    }
//                    else
//                    {
//                        hfc_flag = hfc_flag | ((long)1 << (function_code_selected_int[count] - 1 - 64));
//                    }
//                }
//            }

//            mtrf.setHfc_flag(hfc_flag);
//            mtrf.setLfc_flag(lfc_flag);
//        }
//    }
//}
