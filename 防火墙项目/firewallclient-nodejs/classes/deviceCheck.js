module.exports = deviceCheck;
var protectForm = require('../forms/protectDeviceForm.js');
var DeviceForm = require('../forms/DeviceForm.js');
var fwDeviceForm = require('../forms/fwDeviceForm.js');

/**
 * 用于接收并处理扫描设备返回消息
 * Author：于仁飞
 * @return 返回防火墙设备Map（fwDeviceForm对象）
 */

var fwMap = new Map();
var fwMACSet = new Set();

function deviceCheck() {
    const dgram = require('dgram');
    const server = dgram.createSocket('udp4');
    server.on('error', (err) => {
        console.log(`server error:\n${err.stack}`);
        server.close();
    });
    server.on('message', (msg, rinfo) => {
        console.log(`扫描设备返回信息：${msg.toString()}`);
        var messageArray = msg.toString().split("&");
        var fwIP = messageArray[0];
        var devMAC = messageArray[1];
        var fwMAC = messageArray[2];
        var devIP = messageArray[4];

        console.log(`防火墙IP: ${fwIP}  防火墙MAC: ${fwMAC}`);
        console.log(`保护设备IP：${devIP}  保护设备MAC: ${devMAC}`);
        //如果被保护设备的MAC地址不空
        if (devMAC != null && devMAC != undefined) {
            var prodev = new protectForm(devIP, devMAC);
            prodev.getDevType();
        }

        //暂存扫描设备信息
        var fwdev = new fwDeviceForm(fwIP, fwMAC);
        fwMap.set(fwMAC, fwdev);
        fwdev.addProtecDev(new protectForm(devIP, devMAC));
        fwdev.addProdecIP(devIP);
        fwMACSet.add(fwMAC);
        console.log("扫描完成！");
        return fwMap;
        /*
         //测试使用map
         var fw=new Map([[fwIP,new DeviceForm(fwIP,fwMAC)]]);
        fw.set(fwIP,new DeviceForm(fwIP,fwMAC));
        fw.forEach(function(value,key,map){
            //if(value.getDev_IP()==fwIP)
                console.log(value);
        })
        console.log(fw.size);
        */
    });
    server.bind(30331);
}