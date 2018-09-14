var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');
var whlForm = require('../forms/whiteListForm.js');

/**
 * 白名单规则配置
 * Author：于仁飞
 * @param {*设备IP地址} devIp 
 * @param {*白名单对象} whlForm 
 * @param {*true为添加,false为删除} addOrDelete 
 */
function configWHLRules(devIp, whlForm, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var WHLRules, sqlRule, rule, logRule;
    if (addOrDelete) {
        rule = "iptables -A FORWARD -p tcp -s " + whlForm.getSrcIp() + " --sport " + whlForm.getSrcPort() + " -d " +
            whlForm.getDstIp() + " --dport " + whlForm.getDstPort() + " -j ACCEPT";
        logRule = "WHL1iptables -A FORWARD -p tcp -s " + whlForm.getSrcIp() + " --sport " + whlForm.getSrcPort() + " -d " +
            whlForm.getDstIp() + " --dport " + whlForm.getDstPort() + " -j LOG";
        sqlRule = "INSERT INTO whl values('" + devIp + "','" + whlForm.getDstIp() + "','" + whlForm.getSrcIp() + "','" + whlForm.getDstPort() + "','" + whlForm.getSrcPort() + "')";
    }
    if (!addOrDelete) {
        rule = "iptables -A FORWARD -p tcp -s " + whlForm.getSrcIp() + " --sport " + whlForm.getSrcPort() + " -d " +
            whlForm.getDstIp() + " --dport " + whlForm.getDstPort() + " -j ACCEPT";
        logRule = "WHL0iptables -A FORWARD -p tcp -s " + whlForm.getSrcIp() + " --sport " + whlForm.getSrcPort() + " -d " +
            whlForm.getDstIp() + " --dport " + whlForm.getDstPort() + " -j LOG";
        sqlRule = "DELETE FROM whl where(devIP='" + devIp + "' and dst_IP='" + whlForm.getDstIp() + "' and src_IP='" + whlForm.getSrcIp() + "' and dst_port='" + whlForm.getDstPort() + "' and src_port='" + whlForm.getSrcPort() + "')";
    }
    WHLRules = logRule + " && " + rule;
    console.log(WHLRules);
    console.log(sqlRule);
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(devIp, WHLRules);
    return configInfo.listenConfigInfo();
}
exports.configWHLRules = configWHLRules;