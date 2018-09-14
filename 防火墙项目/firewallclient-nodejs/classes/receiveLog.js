var dgram = require('dgram');
var dbOperations = require('../tools/dbOperations.js');

/**
 * 接收日志并存入本机数据库(未提供实时数据接口)
 * Author：于仁飞
 */
function receiveLog() {
    //建立月份转换map
    var monthMap = new Map([
        ["Jan", "01"],
        ["Feb", "02"],
        ["Mar", "03"],
        ["Apr", "04"],
        ["May", "05"],
        ["Jun", "06"],
        ["Jul", "07"],
        ["Aug", "08"],
        ["Sep", "09"],
        ["Oct", "10"],
        ["Nov", "11"],
        ["Dec", "12"]
    ]);

    //监听日志端口
    const server = dgram.createSocket('udp4');
    server.on('error', (err) => {
        console.log(`server error:\n${err.stack}`);
        server.close();
    });
    server.on('message', (msg, rinfo) => {
        console.log(`日志信息：${msg.toString()}`);
        //日志处理操作
        var log = msg.toString();
        var logArray = log.toString().spilt(" ");
        var logPrefix = log.toString().spilt("]");
        var date = new Date();
        var time, hostName;
        if (logArray[1] == "") {
            time = date.getFullYear().toString() + "-" + monthMap.get(logArray[0]) + "-" + logArray[2] + "-" + logArray[3];
            hostName = logArray[4];
        }
        if (logArray[1] != "") {
            time = date.getFullYear().toString() + "-" + monthMap.get(logArray[0]) + "-" + logArray[1] + "-" + logArray[2];
            hostName = logArray[3];
        }
        tabName = date.getFullYear().toString() + monthMap.get(logArray[0]);
        var prefixPre1 = logPrefix[1].spilt('&');
        var handleResult = prefixPre1[0];
        var protocol = prefixPre1[1];
        var handleReason = prefixPre1[2].split(' ')[0].toString();
        var MAC = log.substring(log.indexOf("MAC") + 4, log.indexOf("SRC") - log.indexOf("MAC") - 4);
        var data_in = log.substring(log.indexOf("IN") + 3, log.indexOf("OUT") - log.indexOf("IN") - 3);
        var PHYSIN = log.substring(log.indexOf("PHYSIN") + 7, log.indexOf("PHYSOUT") - log.indexOf("PHYSIN") - 7);
        var fw_MAC = log.substring(log.indexOf("$") + 1, 17);
        var data_out = log.substring(log.indexOf("OUT") + 4, log.indexOf("PHYSIN") - log.indexOf("OUT") - 4);
        var PHYSOUT = log.substring(log.indexOf("PHYSOUT") + 8, log.indexOf("MAC") - log.indexOf("PHYSOUT") - 8);
        var DST_MAC = MAC.substring(0, 17);
        var SRC_MAC = MAC.substring(18, 17);
        var SRC = log.substring(log.indexOf("SRC") + 4, log.indexOf("DST") - log.indexOf("SRC") - 4);
        var DST = log.substring(log.indexOf("DST") + 4, log.indexOf("LEN") - log.indexOf("DST") - 4);
        var LEN = log.substring(log.indexOf("LEN") + 4, log.indexOf("TOS") - log.indexOf("LEN") - 4);
        var TOS = log.substring(log.indexOf("TOS") + 4, log.indexOf("PREC") - log.indexOf("TOS") - 4);
        var PREC = log.substring(log.indexOf("PREC") + 5, log.indexOf("TTL") - log.indexOf("PREC") - 5);
        var TTL = log.substring(log.indexOf("TTL") + 4, log.indexOf("ID") - log.indexOf("TTL") - 4);
        var ID = log.substring(log.indexOf("ID") + 3, log.indexOf("PROTO") - log.indexOf("ID") - 6);
        var SPT = log.substring(log.indexOf("SPT") + 4, log.indexOf("DPT") - log.indexOf("SPT") - 4);
        var DPT = log.substring(log.indexOf("DPT") + 4, log.indexOf("WINDOW") - log.indexOf("DPT") - 4);

        //数据库操作
        var sqlRule = "CREATE TABLE if not EXISTS log" + tabName + " (time datetime DEFAULT NULL," +
            "host_name varchar(50) DEFAULT NULL, fw_MAC varchar(20) DEFAULT NULL, data_in varchar(10) DEFAULT NULL," +
            "PHYSIN varchar(10) DEFAULT NULL, data_out varchar(10) DEFAULT NULL, PHYSOUT varchar(10) DEFAULT NULL," +
            "DST_MAC varchar(20) DEFAULT NULL, SRC_MAC varchar(20) DEFAULT NULL, SRC varchar(20) DEFAULT NULL," +
            "DST varchar(20) DEFAULT NULL, LEN varchar(10) DEFAULT NULL, TOS varchar(10) DEFAULT NULL," +
            "PREC varchar(10) DEFAULT NULL, TTL varchar(10) DEFAULT NULL, ID varchar(10) DEFAULT NULL," +
            "PROTO varchar(10) DEFAULT NULL, SPT varchar(10) DEFAULT NULL, DPT varchar(10) DEFAULT NULL," +
            "handle_result varchar(30) DEFAULT NULL, handle_reason varchar(255) DEFAULT NULL) ENGINE=InnoDB" +
            "DEFAULT CHARSET=utf8;insert into log" + tabName + "(time,host_name,fw_MAC,data_in,PHYSIN,data_out," +
            "PHYSOUT,DST_MAC,SRC_MAC,SRC,DST,LEN,TOS,PREC,TTL,ID,PROTO,SPT,DPT,handle_result,handle_reason)" +
            "values('" + time + "','" + hostName + "', '" + fw_MAC + "','" + data_in + "','" + PHYSIN + "','" +
            data_out + "','" + PHYSOUT + "','" + DST_MAC + "','" + SRC_MAC + "','" + SRC + "','" + DST + "','" +
            LEN + "','" + TOS + "','" + PREC + "','" + TTL + "','" + ID + "','" + protocol + "','" + SPT + "','" +
            DPT + "','" + handleResult + "','" + handleReason + "')";
        dbOperations.dbOperation(sqlRule);
    });
    server.bind(8000);
}
exports.receiveLog = receiveLog;