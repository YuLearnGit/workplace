var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');

/**
 * 状态检测规则配置
 * Author:于仁飞
 * @param {*设备IP地址} devIp 
 * @param {*协议类型} protocol 
 * @param {*源IP地址} srcIp 
 * @param {*目的IP地址} dstIp 
 * @param {*源端口} sport 
 * @param {*目的端口} dport 
 * @param {*true为添加,false为删除} addOrDelete 
 */

function configSTDRules(devIp, protocol, srcIp, dstIp, sport, dport, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var STDRules, sqlRule, logRule, rule;
    if (addOrDelete) {
        rule = "iptables -A FORWARD -p " + protocol + " -s " + srcIp + " --sport " + sport + " -d " + dstIp + " --dport " + dport + " -m state --state NEW -j ACCEPT";
        logRule = "STD1iptables -A FORWARD -p " + protocol + " -s " + srcIp + " --sport " + sport + " -d " + dstIp + " --dport " + dport + " -m state --state NEW -j LOG";
        sqlRule = "INSERT INTO STD VALUES ('" + devIp + "','" + protocol + "','" + srcIp + "','" + dstIp + "','" + sport + "','" + dport + "')";
    }
    if (!addOrDelete) {
        rule = "iptables -A FORWARD -p " + protocol + " -s " + srcIp + " --sport " + sport + " -d " + dstIp + " --dport " + dport + " -m state --state NEW -j ACCEPT";
        logRule = "STD0iptables -A FORWARD -p " + protocol + " -s " + srcIp + " --sport " + sport + " -d " + dstIp + " --dport " + dport + " -m state --state NEW -j LOG";
        sqlRule = "DELETE FROM STD where(devIP='" + devIp + "' and protocol='" + protocol + "' and srcIP='" + srcIp + "' and dstIP='" + dstIp + "' and sport='" + sport + "' and dport='" + dport + "')";
    }
    STDRules = logRule + " && " + rule;
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(devIp, STDRules);
    return configInfo.listenConfigInfo();
}
exports.configSTDRules = configSTDRules;