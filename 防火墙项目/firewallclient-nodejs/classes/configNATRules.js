var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');

/**
 * 配置源地址映射规则
 * Author：于仁飞
 * @param {*防火墙对象，无IP模式下不能配置} dev 
 * @param {*网口} ethName 
 * @param {*网口对应设备的IP地址} devIp 
 * @param {*网口IP地址} ethIp 
 * @param {*网口IP对应的掩码}netmask
 * @param {*true为添加,false为删除} addOrDelete 
 */

function configSNATRules(dev, ethName, devIp, ethIp, netmask, addOrDelete) {
    if (dev.getDevIP() == "0.0.0.0")
        return false;
    var configEthBr, configEthIp, sqlRule, SNATRules;
    var rule = "iptables -t nat -A POSTROUTING -s " + devIp + " -o br0 -j SNAT --to-source " + dev.getDevIP();
    if (addOrDelete) {
        configEthBr = "brctl delif br0 " + ethName;
        configEthIp = "ifconfig " + ethName + " " + ethIp + " netmask " + netmask + " up";
        SNATRules = "NAT1" + configEthBr + " && " + configEthIp + " && " + rule;
        sqlRule = "INSERT INTO snat VALUES('" + dev.getDevMAC() + "','" + devIp + "','" + ethName + "','" + ethIp + "','" + netmask + "','" + dev.getDevIP() + "')";
    }
    if (!addOrDelete) {
        configEthBr = "brctl addif br0 " + ethName;
        configEthIp = "ifconfig " + ethName + " 0.0.0.0 up";
        SNATRules = "NAT0" + configEthIp + " && " + configEthBr + " && " + rule;
        sqlRule = "DELETE FROM snat where(fwMAC='" + dev.getDevMAC() + "' and origin_devIP='" + devIp + "' and EthName='" + ethName + "' and EthIP='" + ethIp + "' and netmask='" + netmask + "' and NATIP='" + dev.getDevIP() + "')";
    }
    console.log(SNATRules);
    console.log(sqlRule);
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(dev.getDevIP(), SNATRules);
    return configInfo.listenConfigInfo();
}
exports.configSNATRules = configSNATRules;

/**
 * 配置目的地址映射规则
 * Author：于仁飞
 * @param {*防火墙设备对象，无IP模式下不能配置} dev 
 * @param {*原目的IP地址} originIp 
 * @param {*原目的端口，any表示任意端口} originPort 
 * @param {*映射IP地址} mapIp 
 * @param {*映射端口，any表示任意端口} mapPort 
 * @param {*true为添加,false为删除} addOrDelete 
 */
function configDNATRules(dev, originIp, originPort, mapIp, mapPort, addOrDelete) {
    if (dev.getDevIP() == "0.0.0.0")
        return false;
    var preRule, sqlRule, DNATRules;
    if (originPort != "any" & mapPort != "any") {
        preRule = "iptables -t nat -A PREROUTING -d" + " " + originIp + " " + "-p tcp --dport " + originPort +
            " -j DNAT --to-destination " + mapIp + ":" + mapPort;
    }
    if (originPort != "any" & mapPort == "any") {
        preRule = "iptables -t nat -A PREROUTING -d" + " " + originIp + " " + "-p tcp --dport " + originPort +
            " -j DNAT --to-destination " + mapIp;
    }
    if (originPort == "any" & mapPort != "any") {
        preRule = "iptables -t nat -A PREROUTING -d" + " " + originIp + " " + "-p tcp " + " -j DNAT --to-destination " + mapIp + ":" + mapPort;
    }
    if (originPort == "any" & mapPort == "any") {
        preRule = "iptables -t nat -A PREROUTING -d" + " " + originIp + " " + "-p tcp " + " -j DNAT --to-destination " + mapIp;
    }
    if (addOrDelete) {
        DNATRules = "NAT1" + preRule;
        sqlRule = "INSERT INTO dnat values('" + dev.getDevMAC() + "','" + originIp + "','" + originPort + "','" + mapIp + "','" + mapPort + "')";
    }
    if (!addOrDelete) {
        DNATRules = "NAT0" + preRule;
        sqlRule = "DELETE FROM dnat where(fwMAC='" + dev.getDevMAC() + "' and origin_dstIP='" + originIp + "' and origin_dport='" + originPort + "' and map_IP='" + mapIp + "' and map_port='" + mapPort + "')";
    }
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(dev.getDevIP(), DNATRules);
    return configInfo.listenConfigInfo();
}
exports.configDNATRules = configDNATRules;