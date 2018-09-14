var protectDeviceForm=require('../forms/protectDeviceForm.js');
var configInfo=require('../tools/listenConfigInfo.js');
var sendInfo=require('../tools/sendInfo.js');

/**
 * 用于恢复防火墙IP地址
 * Author：于仁飞
 * @param {*被保护设备对象protectDeviceForm} protecDev 
 * @param {*防火墙IP地址} bindIp 
 */

function resetIp(protecDev,bindIp){
var cmd="ifconfig br0 down && ifconfig br0 " +bindIp+" up";
sendInfo.sendConfigInfo(protecDev.getDevIP(),cmd);
configInfo.listenConfigInfo();
};exports.resetIp=resetIp;

/**
 * 返回配置是否成功
 * Author：于仁飞
 * @param {*配置确认结果：true为成功，false为失败} result 
 */
var confirm=function(result){
    if(result){
        console.log("配置成功");
        return true;
    }
    else{
        console.log("配置失败");
        return false;
    }
};
exports.confirm=confirm;
