module.exports = fwDeviceForm;
var protectDeviceForm = require('./protectDeviceForm.js');
/**
 * 防火墙设备相关信息包括其下所连接的被保护设备信息（防火墙设备对象）
 * function：获取防火墙设备IP、MAC地址；获取被保护设备map
 * Author:于仁飞
 */

var prodevIPMap = new Map();
var protecDevMap = new Map();

function fwDeviceForm(fwIP, fwMAC) {
    this.fwIP = fwIP;
    this.fwMAC = fwMAC;

    this.getDevIP = function () {
        return fwIP;
    };

    this.getDevMAC = function () {
        return fwMAC;
    };

    this.setFwIP = function () {
        fwIP = "0.0.0.0";
    };

    //protecDev类型为new protectDeviceForm
    this.addProtecDev = function (protecDev) {
        protecDevMap.set(fwMAC, protecDev);
    };

    //返回以防火墙设备MAC地址为键的被保护设备map
    this.getProtecDevMap = function () {
        return protecDevMap;
    };
    //添加被保护设备IP地址
    this.addProdecIP = function (devIP) {
        prodevIPMap.set(fwMAC, devIP);
    };
    //返回以防火墙设备MAC地址为键的被保护设备的IP地址map
    this.getProdecIPMap = function () {
        return prodevIPMap;
    };

}
