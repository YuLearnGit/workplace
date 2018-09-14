var getMAC = require('./getLocalMAC.js');
var q = require('q');
var confirmInfo; //返回确认信息,true为配置成功

/**
 * 用于扫描设备
 * Author: 于仁飞
 * @param {*扫描设备的IP地址} devIP 
 */

function sendCheckInfo(devIP) {
    var mac = getMAC.getLocalMAC();
    //构造数据,转化成Buffer
    var head = ['0x0f', '0x0e', '0x0d', '0x0c', '0x0b', '0x0a'];
    head_buf = new Buffer(head);
    var body = mac + '！';
    body_buf = new Buffer(body);
    var data = head.concat(body);
    var buf = Buffer.concat([head_buf, body_buf]);
    //创建UDP Client
    const dgram = require('dgram');
    const UDPClient = dgram.createSocket('udp4');
    UDPClient.on('close', () => {
        console.log('socket已关闭');
    });
    UDPClient.on('error', (err) => {
        console.log(err);
    });
    UDPClient.on('message', (msg, rinfo) => {
        if (msg == 'exit') UDPClient.close();
        console.log(`receive message from ${rinfo.address}:${rinfo.port}`);
    });
    UDPClient.send(buf, 33333, devIP);
}
exports.sendCheckInfo = sendCheckInfo;

/**
 * 用于发送配置设备信息
 * Author：于仁飞
 * @param {*配置设备IP地址} devIP 
 * @param {*配置命令} cmd 
 * @return {*返回bool值，true为成功，false为失败}
 */

function sendConfigInfo(devIP, cmd) {
    //构造数据,转化成Buffer
    var head = ['0x0f', '0x0e', '0x0d', '0x0c', '0x0b', '0x0a'];
    head_buf = new Buffer(head);
    var body = cmd + '!';
    body_buf = new Buffer(body);
    buf = Buffer.concat([head_buf, body_buf]);
    const dgram = require('dgram');

    const UDPClient = dgram.createSocket('udp4');
    UDPClient.on('close', () => {
        console.log('socket已关闭');
    });
    UDPClient.on('error', (err) => {
        console.log(err);
    });
    UDPClient.on('message', (msg, rinfo) => {
        if (msg == 'exit') UDPClient.close();
        console.log(`receive message from ${rinfo.address}:${rinfo.port}`);
    });
    UDPClient.send(buf, 22222, devIP);
}
exports.sendConfigInfo = sendConfigInfo;