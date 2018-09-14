/*----------------------------------------------------------------
// Copyright (C) 
// 版权所有。
//
// 文件名：GlobalXmlConfig
// 文件功能描述：全局变量读取
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class GlobalXmlConfig
    {
        //*----------------------------------------------------------------
        //函数说明：静态方法读取全局变量
        //输入：无
        //输出：无
        //----------------------------------------------------------------*//   
        public static void ReadXmlConfig()
        {
            //读取全局配置信息
            XmlSerializationHelper configContext = new XmlSerializationHelper("Config");
            GlobalConfig globalconfig = configContext.Get<GlobalConfig>();

            //数据库连接配置               
            StaticGlobal.database = globalconfig.DataBaseConfigs[0].DataBase;
            StaticGlobal.datasource = globalconfig.DataBaseConfigs[0].DataSource;
            StaticGlobal.userid = globalconfig.DataBaseConfigs[0].UserId;
            StaticGlobal.password = globalconfig.DataBaseConfigs[0].Password;
            StaticGlobal.ConnectionString = "Database = " + StaticGlobal.database + ";Data Source = " + StaticGlobal.datasource + "; User Id = " + StaticGlobal.userid + "; Password =" + StaticGlobal.password;

            //参数配置
            StaticGlobal.FunctionCodeNumber = globalconfig.Parameters[0].FunctionCodeNum;

            //第一次使用软件标志
            StaticGlobal.firstloginflag = globalconfig.LoginSettings[0].firstloginflag;
        }

    }
}
