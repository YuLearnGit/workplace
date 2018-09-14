module.exports=deviceScan;
//var DeviceForm=require('./forms/DeviceForm.js');
var sendInfo=require('./sendInfo.js');
var getMAC=require('./getLocalMAC.js');

/**
 * 用于扫描设备，无返回值
 * Author：于仁飞
 * @param {*string扫描设备起始IP} startIP 
 * @param {*string扫描设备结束IP} endIP 
 */

function deviceScan( startIP, endIP){
var startIP_array=startIP.toString().split(".");
var endIP_array=endIP.toString().split(".");

var staticPart=startIP_array[0]+"."+startIP_array[1]+"."+startIP_array[2]+".";
var start=parseInt(startIP_array[3]);
var end=parseInt(endIP_array[3]);
var IP_num=endIP_array[3]-startIP_array[3];
process.stdout.write(`IP_num: ${IP_num} \n`);

var devIP_list=new Array();
for(var count=0;count+start<=end;count++){
    devIP_list.push(staticPart+(count+start).toString());
}
console.log(`本机MAC地址： ${getMAC.getLocalMAC()}`);
devIP_list.forEach(function(devIP) {
    //var devform=new DeviceForm(devIP,33333);
    console.log(devIP);
    sendInfo.sendCheckInfo(devIP);

}, this);
}