/*----------------------------------------------------------------
// Copyright (C) 
// 版权所有。
//
// 文件名：StaticGlobal
// 文件功能描述：全局变量存放类
//----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    public class StaticGlobal
    {  
        //被选择规则的下标
        public static int selectedindex;
        //被选择规则的下标
        public static int firewallindex;
        //修改数据的标志位
        public static bool editflag = false;
        //设置规则的防火墙MAC
        public static string firewallmac;

        public static List<FWDeviceForm> fwdev_list = new List<FWDeviceForm>();
      
        public static List<FireWallDevices> FireWalldevices = new List<FireWallDevices>();

        public static List<PropertyDataTble> property = new List<PropertyDataTble>();

        public static List<SNATRuleDataTable> SNAToldrules = new List<SNATRuleDataTable>();//SNAT规则

        public static List<DNATRuleDataTable> DNAToldrules = new List<DNATRuleDataTable>();//DNAT规则
        public static List<WHLRuleDataTable> WHLoldrules = new List<WHLRuleDataTable>();//白名单规则

        public static List<FireWallRuleDataTable> oldrules = new List<FireWallRuleDataTable>();//DPI规则

        public static List<APCRuleDataTable> APColdrules = new List<APCRuleDataTable>();//APC规则

        public static List<CNCRuleDataTable> CNColdrules = new List<CNCRuleDataTable>();//CNC规则

        public static List<PRTRuleDataTable> PRToldrules = new List<PRTRuleDataTable>();//PRT规则

        public static List<STDRuleDataTable> STDoldrules = new List<STDRuleDataTable>();//STD规则
        //数据库连接信息
        public static string ConnectionString;

        //数据库信息
        public static string database;
        public static string datasource;
        public static string userid;
        public static string password;

        //功能码数目
        public static int FunctionCodeNumber;

        //用户ID
        public static int UserID;

        //登录用户名
        public static string UserName;
        //登录用户权限
        public static string UserRole = "SUPER";

        //用户未注销标志
        public static Boolean flag = true;

        //第一次使用软件标志
        public static Boolean firstloginflag = true;

        //自定义的PLC设备名称
        public static string newPLC;

        //防火墙设备的MAC地址和IP的对应关系
        public static Dictionary<string, string> FwMACandIP = new Dictionary<string, string>();

        //进入系统的时间，即实时数据的起点
        public static string LoginTime;

        //设备扫描IP范围
        public static string ScanIP;
    }
}