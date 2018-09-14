//module.exports=noIPConfig;
var fwDeviceForm = require('../forms/fwDeviceForm.js');
var configInfo = require('../tools/listenConfigInfo.js');
var sendInfo = require('../tools/sendInfo.js');

/**
 * 用于将防火墙设备设置成无IP
 * Author：于仁飞
 * @param {*防火墙设备对象} fwdev 
 */


var noIPConfig = function (fwdev) {
    var cmd = "ifconfig br0 down && ifconfig br0 0.0.0.0 up";
    if (fwdev.getFwIP() == "0.0.0.0")
        return false;
    sendInfo.sendConfigInfo(fwdev.getFwIP(), cmd);
    configInfo.listenConfigInfo();
};
exports.noIPConfig = noIPConfig;

var confirm = function (result) {
    if (result) {
        console.log("配置成功");
        return true;
    }
    else {
        console.log("配置失败");
        return false;
    }
};
exports.confirm = confirm;