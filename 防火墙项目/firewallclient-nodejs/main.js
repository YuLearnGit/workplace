/**
 * 用于测试，调试入口
 * Author:于仁飞
 */
var sendInfo = require('./tools/sendInfo.js');
var db = require('./tools/dbOperations.js');
//测试扫描设备及配置设备


/*
sendInfo.sendCheckInfo('172.16.10.165');
//sendInfo.sendConfigInfo('172.16.10.76','ifconfig');
*/

//测试数据库
/*
var db=require('./tools/dbOperations.js');
db.dbOperation("select Manufacturers from macs where Macs='C0-98-79'");
//db.dbOperation("select * from apc");
*/

//测试form
/*
var protectForm=require('./forms/protectDeviceForm');
var DeviceForm=require('./forms/DeviceForm.js');
var dev=new DeviceForm("172.16.10.1",22222);
//process.stdout.write("getDev_IP: ");dev.getDev_IP();
//process.stdout.write("setDev_IP: ");dev.setDev_IP("192.16.10.1");
//process.stdout.write("getDev_info: ");dev.getDev_info();
//process.stdout.write("setDev_info: ");dev.setDev_info("33333");

var protectForm=require('./forms/protectDeviceForm.js');
var prodev=new protectForm("21312","C0:98:79:c6:5f:c5");
prodev.getDevType();
*/

//测试扫描设备
//var devMac = require('./tools/getLocalMAC.js');
//devMac.getLocalMAC();

var devScan = require('./tools/deviceScan.js');
var scan = new devScan("192.16.10.7", "192.16.10.10");

var devCheck = require('./classes/deviceCheck.js');
var check = new devCheck();


//测试无IP配置
var noIp = require('./classes/noIPConfig.js');
var fwdev = require('./forms/fwDeviceForm.js');
var configInfo = require('./tools/listenConfigInfo.js');
var resetIp = require('./classes/resetIp.js');
var protectDeviceForm = require('./forms/protectDeviceForm.js');

//noIp.noIPConfig(new fwdev("172.16.10.19","MAC"));
//configInfo.listenConfigInfo();
//resetIp.resetIp(new protectDeviceForm("172.16.10.76","MAC"),"172.16.10.19");

//测试规则配置
var dpi = require('./classes/configDPIRules.js');
var modbusForm = require('./forms/modbusTcpRulesForm.js');
var OPCForm = require('./forms/OPCRulesForm.js');
var DNP3Form = require('./forms/DNP3RulesForm.js');
var whlForm = require('./forms/whiteListForm.js');
//dpi.configModbusTcpRules(new protectDeviceForm("172.16.10.200", "28:63:36:95:d1:8d"), new modbusForm("any", "any", 12292, 12292, 6, 10, 700), true, true);
//dpi.configOPCRules(new protectDeviceForm("172.16.10.76", "MAC"), new OPCForm("any", "any"), true, false);
//dpi.configDNP3Rules(new protectDeviceForm("172.16.10.76", "MAC"), new DNP3Form("any", "any"), true, true);

var APC = require('./classes/configAPCRules.js');
//APC.configAPCRules("172.16.10.76", "ftp", true);
var CNC = require('./classes/configCNCRules.js');
//CNC.configCNCRules("172.16.10.76", "50", "172.16.10.1", "172.16.10.10", 12, 21, false);
var PRT = require('./classes/configPRTRules.js');
//PRT.configDefaultRoute("172.16.10.76", "172.16.10.254", false);
//PRT.configHostRoute("172.16.10.76", "172.16.10.231", "172.16.10.254", false);
//PRT.configNetRoute("172.16.10.76", "10.20.30.40", "255.255.255.248", "172.16.10.254", false);
var STD = require('./classes/configSTDRules.js');
//STD.configSTDRules("172.16.10.76", "tcp", "10.20.30.40", "20.30.40.50", "12", "21", false);
var WHL = require('./classes/configWHLRules.js');
//WHL.configWHLRules("172.16.10.76", new whlForm("172.16.10.11", "172.16.10.1", "12", "21"), false);

var NAT = require('./classes/configNATRules.js');
//NAT.configSNATRules(new fwdev("172.16.10.76", "MAC"), "eth3", "192.16.10.1", "172.16.10.1", "255.255.255.0", false);
//NAT.configDNATRules(new fwdev("172.16.10.19", "MAC"), "192.16.10.1", "23", "172.16.10.254", "any", false);
var log = require('./classes/receiveLog.js');
//log.receiveLog();

//测试mongodb
// var mongo = require('./tools/dbOperations-mongo.js');
// var data = {
//     "name": "菜鸟教程",
//     "url": "www.runoob.com"
// };
// mongo.deleteData(data, 'site');