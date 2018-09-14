var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');

//策略路由规则配置
/**
 * 配置默认路由
 * Author:于仁飞
 * @param {*设备IP地址} devIp 
 * @param {*路由数据包通过的网关} gateway 
 * @param {*true为添加,false为删除} addOrDelete 
 */
function configDefaultRoute(devIp, gateway, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var defaultRoute, sqlRule;
    if (addOrDelete) {
        defaultRoute = "PRT1route add default gw " + gateway;
        sqlRule = "INSERT INTO prt VALUES " + "('" + devIp + "'," + "'默认路由','" + "" + "','" + "" + "','" + "" + "','" + "br0" + "','" + gateway + "')";
    }
    if (!addOrDelete) {
        defaultRoute = "PRT0route del default gw " + gateway;
        sqlRule = "DELETE FROM prt where (devIP='" + devIp + "' and route_type='默认路由' and gateway='" + gateway + "')";
    }
    console.log(defaultRoute, sqlRule);
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(devIp, defaultRoute);
    return configInfo.listenConfigInfo();
}
exports.configDefaultRoute = configDefaultRoute;

/**
 * 配置到主机的路由
 * Author:于仁飞
 * @param {*设备IP地址} devIp 
 * @param {*目的地址的主机IP地址} host 
 * @param {*路由数据包通过的网关} gateway 
 * @param {*true为添加,false为删除} addOrDelete 
 */
function configHostRoute(devIp, host, gateway, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var hostRoute, sqlRule;
    if (addOrDelete) {
        hostRoute = "PRT1route add -host " + host + " gw " + gateway;
        sqlRule = "INSERT INTO prt values('" + devIp + "','主机路由','" + host + "','','','br0','" + gateway + "')";
    }
    if (!addOrDelete) {
        hostRoute = "PRT0route del -host " + host + " gw " + gateway;
        sqlRule = "DELETE FROM prt where(devIP='" + devIp + "' and route_type='主机路由' and host='" + host + "' and gateway='" + gateway + "')";
    }
    console.log(hostRoute, sqlRule);
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(devIp, hostRoute);
    return configInfo.listenConfigInfo();
}
exports.configHostRoute = configHostRoute;

/**
 * 配置到网络的路由
 * Author:于仁飞
 * @param {*设备IP地址} devIp 
 * @param {*目的网路地址} net 
 * @param {*目的地址的网络掩码} netmask 
 * @param {*路由数据包通过的网关} gateway 
 * @param {*true为添加,false为删除} addOrDelete 
 */

function configNetRoute(devIp, net, netmask, gateway, addOrDelete) {
    if (devIp == "0.0.0.0")
        return false;
    var netRoute, sqlRule;
    if (addOrDelete) {
        netRoute = "PRT1route add -net " + net + " netmask " + netmask + " gw " + gateway;
        sqlRule = "INSERT INTO prt VALUES('" + devIp + "','网络路由','','" + net + "','" + netmask + "','br0','" + gateway + "')";
    }
    if (!addOrDelete) {
        netRoute = "PRT0route del -net " + net + " netmask " + netmask + " gw " + gateway;
        sqlRule = "DELETE FROM prt where(devIP='" + devIp + "' and route_type='网络路由' and net='" + net + "' and netmask='" + netmask + "' and gateway='" + gateway + "')";
    }
    console.log(netRoute, sqlRule);
    dbOperations.dbOperation(sqlRule);
    sendInfo.sendConfigInfo(devIp, netRoute);
    return configInfo.listenConfigInfo();
}
exports.configNetRoute = configNetRoute;