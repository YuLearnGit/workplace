/**
 * 监测防火墙配置返回信息
 * Author：于仁飞
 */


//var events = require('events');  
var noIP = require('../classes/noIPConfig.js');
var resetIp = require('../classes/resetIp.js');
const dgram = require('dgram');
//var eventEmitter = new events.EventEmitter(); 

function listenConfigInfo() {
    const server = dgram.createSocket('udp4');

    server.on('error', (err) => {
        console.log(`server error:\n${err.stack}`);
        server.close();
    });
    server.on('message', (msg, rinfo) => {
        console.log(`配置确认信息：${msg.toString()}`);
        if (msg.toString() == 'success') {
            //noIP.confirm('true');
            //resetIp.confirm('true');
            console.log("配置成功");
            return true;
        }
        if (msg.toString() == 'fail') {
            //noIP.confirm('');
            console.log("配置失败");
            return false;
        }
    });
    server.bind(30333);
}
exports.listenConfigInfo = listenConfigInfo;