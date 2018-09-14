var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');

/**
 * 配置连接数控制规则
 * @param {*设备IP地址string} devIp 
 * @param {*连接数string} conlimit 
 * @param {*源IP地址string(any表示任意)} srcIp 
 * @param {*目的IP地址string} dstIp 
 * @param {*源端口string} sport 
 * @param {*目的端口string} dport 
 * @param {*true为添加，false为删除bool} addOrDelete 
 */

function configCNCRules(devIp, conlimit, srcIp, dstIp, sport, dport, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var rule, CNCRules, sqlRule;
    if (srcIp != "any") {
        rule = "iptables -A FORWARD -p tcp --syn -s " + srcIp + " --sport " + sport + " -d " + dstIp + " --dport " + dport +
            " -m connlimit --connlimit-above " + conlimit;
    }
    if (srcIp == "any") {
        rule = "iptables -A FORWARD -p tcp --syn --sport " + sport + " -d " + dstIp + " --dport " + dport +
            " -m connlimit --connlimit-above " + conlimit;
    }

    var CNCLogRule = rule + " -j LOG";

    if (addOrDelete) {
        CNCRules = "CNC1" + CNCLogRule + " && " + rule;
        sqlRule = "INSERT INTO cnc VALUES " + "('" + devIp + "','" + conlimit + "','" + srcIp + "','" + dstIp + "','" + sport + "','" + dport + "')";
        dbOperations.dbOperation(sqlRule);
        sendInfo.sendConfigInfo(devIp, CNCRules);
        return (configInfo.listenConfigInfo());
    }
    if (!addOrDelete) {
        CNCRules = "CNC0" + CNCLogRule + " && " + rule;
        sqlRule = "DELETE FROM cnc WHERE (devIP='" + devIp + "' and connlimit=" + conlimit + " and srcIP='" + srcIp + "' and dstIP='" + dstIp + "' and sport='" + sport + "' and dport='" + dport + "')";
        dbOperations.dbOperation(sqlRule);
        sendInfo.sendConfigInfo(devIp, CNCRules);
        return (configInfo.listenConfigInfo());
    }
}
exports.configCNCRules = configCNCRules;