#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FWclient
{
    /// <summary>
    /// 读写文件操作
    /// </summary>
    class FileOperation
    {
        //规则存储文件路径
        string DefaultPath = "";
        public FileOperation(string DefaultPath)
        {
            this.DefaultPath = DefaultPath;
        }
        /// <summary>
        /// 查询规则
        /// </summary>
        /// <param name="filename">查询文件名</param>
        /// <returns>返回查询结果List</returns>
        public List<string> SearchRules(string filename)
        {
            string path = DefaultPath + "\\"+filename;
            FileStream fs = new FileStream(@path,FileMode.Open,FileAccess.Read,FileShare.None);
            StreamReader rf = new StreamReader(fs);
            List<string> rl = new List<string>();
            string nextLine="";
            try
            {
                while ((nextLine = rf.ReadLine()) != null)
                {
                    rl.Add(nextLine);
#if debug
                    Console.WriteLine(nextLine);
#endif
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error:" + e.ToString());
            }
            finally
            {
                rf.Close();
                fs.Close();
            }
            return rl;
        }

        /// <summary>
        /// 保存规则
        /// </summary>
        /// <param name="rules">要保存的规则内容</param>
        /// <param name="filename">文件名</param>
        /// <returns>true为保存成功，false为保存失败</returns>
        public bool SaveRules(string rules,string filename)
        {
            string path = DefaultPath + "\\" + filename;
            FileStream fs = new FileStream(@path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            StreamReader rf = new StreamReader(fs);
            StreamWriter wf = new StreamWriter(fs);
            bool flag = true;
            string nextLine = "";
            try
            {
                //判断是否已经存在此规则,不存在再追加到文件末尾
                bool existFlag = false;
                while((nextLine=rf.ReadLine())!=null)
                {
                    if(nextLine==rules)
                    {
                        existFlag = true;
                    }
                }
                if (!existFlag)
                {
                    wf.Write("");
                    wf.WriteLine(rules);
                }
                else
                {
#if debug 
                    Console.WriteLine("rule existed!");
#endif
                    flag = false;
                }
            }
            catch(Exception e)
            {
                flag = false;
                Console.WriteLine("error:"+e.ToString());
            }
            finally
            {
                wf.Close();
                fs.Close();
            }
            return flag;              
        }
    }
}
