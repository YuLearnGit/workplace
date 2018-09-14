using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    interface IReceiveLog
    {
        /// <summary>
        /// 日志存储
        /// </summary>
        /// /// <param name="start">值为true时开始接收并存储日志到数据库</param>
        void Save_DisplayLog(bool start);

        /// <summary>
        /// 实时日志(调用之前必须先实例化Save_DisplayLog)
        /// </summary>
        /// <returns>返回string, 当前调用时接收到的日志</returns>
        string queue_func();
    }
}
