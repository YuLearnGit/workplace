module.exports = protectDeviceForm;

/**
 * 被保护设备对象
 * function：获取设备IP、MAC及设备类型
 * AUthor：于仁飞
 * @param {*被保护设备的IP地址} devIP 
 * @param {*被保护设备的MAC地址} devMAC 
 */

function protectDeviceForm(devIP, devMAC) {
    this.devMAC = devMAC;
    this.devIP = devIP;

    //返回设备MAC地址
    this.getDevMAC = function () {
        return devMAC;
    };

    //返回设备IP地址
    this.getDevIP = function () {
        return devIP;
    };
    //获取设备类型
    this.getDevType = function () {
        var devType = "";
        var macArray = devMAC.toString().split(":");
        var macQuery = macArray[0] + "-" + macArray[1] + "-" + macArray[2];
        var mysql = require('mysql');

        var connect = mysql.createConnection({
            host: 'localhost',
            user: 'root',
            password: '123456',
            database: 'firewallclient'
        });
        var sqlstr = "select Manufacturers from macs where Macs=" + "'" + macQuery + "'";
        //var sqlstr="select Manufacturers from macs where Macs='C0-98-79'";
        connect.connect();
        connect.query(sqlstr, function (err, results) {
            if (err) {
                console.log(err.message);
                throw err;
            }
            if (results && results.length != 0) {
                for (var i = 0; i < results.length; i++) {
                    devType = results[i].Manufacturers;
                    console.log("设备类型：%s\t", devType);
                }
            } else {
                devType = "受保护设备类型未知!";
                console.log(devType);
            }
        });
        connect.end();

        return devType;

    };
}