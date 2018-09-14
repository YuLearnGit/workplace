var protectForm = require('../forms/protectDeviceForm.js');
var fwDeviceForm = require('../forms/fwDeviceForm.js');
var sendInfo = require('../tools/sendInfo.js');
var dbOperations = require('../tools/dbOperations.js');
var configInfo = require('../tools/listenConfigInfo.js');

/**
 * 配置ModbusTCP规则
 * Author：于仁飞
 * @param {*设备对象（防火墙或者被保护设备）} dev 
 * @param {*modbus规则对象} modbusTcpForm 
 * @param {*日志标志} logFlag 
 * @param {*true为添加规则，false为删除规则} addOrDelete 
 */

function configModbusTcpRules(dev, modbusTcpForm, logFlag, addOrDelete) {
    var dpiProto = "modbusTcp";
    var flag;
    var dpiRules, dpiLogRule;
    var sqlRule;

    if (modbusTcpForm.getSrcIp() == "any" && modbusTcpForm.getDstIp() == "any") {
        dpiRules = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "DROP";

        dpiLogRule = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "DROP&modbus&data_illegal " + "\"";
    }
    if (modbusTcpForm.getSrcIp() == "any" && modbusTcpForm.getDstIp() != "any") {
        dpiRules = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-d" + " " + modbusTcpForm.getDstIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "DROP";

        dpiLogRule = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-d" + " " + modbusTcpForm.getDstIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "DROP&modbus&data_illegal " + "\"";
    }

    if (modbusTcpForm.getSrcIp() != "any" && modbusTcpForm.getDstIp() == "any") {
        dpiRules = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-s " + modbusTcpForm.getSrcIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "DROP";

        dpiLogRule = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-s " + modbusTcpForm.getSrcIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "DROP&modbus&data_illegal " + "\"";
    }

    if (modbusTcpForm.getSrcIp() != "any" && modbusTcpForm.getDstIp() != "any") {
        dpiRules = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-s " + modbusTcpForm.getSrcIp() + " " + "-d" + " " + modbusTcpForm.getDstIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + " DROP";

        dpiLogRule = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " +
            "-s " + modbusTcpForm.getSrcIp() + " " + "-d" + " " + modbusTcpForm.getDstIp() + " " + "-m" + " " + dpiProto + " " + "--data-addr" + " " + modbusTcpForm.getMinAddr() + ":" + modbusTcpForm.getMaxAddr() + " " + "--modbus-func " +
            modbusTcpForm.getFunc() + " " + "--modbus-data " + modbusTcpForm.getMinData() + ":" + modbusTcpForm.getMaxData() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "DROP&modbus&data_illegal " + "\"";
    }
    if (addOrDelete) {
        flag = "DPI1";
        sqlRule = "INSERT INTO modbustcp values " + "('" + dev.getDevMAC() + "','" + dpiProto + "','" + modbusTcpForm.getSrcIp() +
            "','" + modbusTcpForm.getDstIp() + "','" + modbusTcpForm.getMinAddr() + "','" + modbusTcpForm.getMaxAddr() + "','" + modbusTcpForm.getMinData() + "','" + modbusTcpForm.getMaxData() + "','" + modbusTcpForm.getFunc() + "','" + logFlag + "','DROP')";
    } else {
        flag = "DPI0";
        sqlRule = "delete from modbustcp where fw_mac= '" + dev.getDevMAC() + "' and protocol = '" + dpiProto + "' and source = '" +
            modbusTcpForm.getSrcIp() + "' and destination = '" + modbusTcpForm.getDstIp() + "' and coiladdressstart = '" + modbusTcpForm.getMinAddr() + "' and coiladdressstart = '" +
            modbusTcpForm.getMaxAddr() + "'and minspeed ='" + modbusTcpForm.getMinData() + "' and maxspeed ='" + modbusTcpForm.getMaxData() + "' and functioncode = '" + modbusTcpForm.getFunc() + "' and log_flag = '" + logFlag + "';";
    }
    var modbusTcpRules = flag + dpiLogRule + " && " + dpiRules;
    sendInfo.sendConfigInfo(dev.getDevIP(), modbusTcpRules);
    dbOperations.dbOperation(sqlRule);
    return (configInfo.listenConfigInfo());
}
exports.configModbusTcpRules = configModbusTcpRules;


/**
 * 用于配置OPC规则
 * Author：于仁飞
 * @param {*防火墙或被保护设备对象} dev 
 * @param {*OPC规则对象} OPCRulesForm 
 * @param {*记录日志} logFlag 
 * @param {*true为添加日志，false为删除日志} addOrDelete 
 */
function configOPCRules(dev, OPCRulesForm, logFlag, addOrDelete) {
    var flag, dpiRules, dpiLogRule, sqlRule;

    if (OPCRulesForm.getSrcIp() == "any" && OPCRulesForm.getDstIp() == "any") {
        dpiRules = "iptables -A FORWARD -p tcp --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
        dpiLogRule = "iptables -A FORWARD -p tcp --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
    }
    if (OPCRulesForm.getSrcIp() != "any" && OPCRulesForm.getDstIp() == "any") {
        dpiRules = "iptables -A FORWARD -p tcp -s " + OPCRulesForm.getSrcIp() + " --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
        dpiLogRule = "iptables -A FORWARD -p tcp -s " + OPCRulesForm.getSrcIp() + " --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
    }
    if (OPCRulesForm.getSrcIp() == "any" && OPCRulesForm.getDstIp() != "any") {
        dpiRules = "iptables -A FORWARD -p tcp -d " + OPCRulesForm.getDstIp() + " --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
        dpiLogRule = "iptables -A FORWARD -p tcp -d " + OPCRulesForm.getDstIp() + " --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
    }
    if (OPCRulesForm.getSrcIp() != "any" && OPCRulesForm.getDstIp() != "any") {
        dpiRules = "iptables -A FORWARD -p tcp -s " + OPCRulesForm.getSrcIp() + " -d " + OPCRulesForm.getDstIp() + " --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
        dpiLogRule = "iptables -A FORWARD -p tcp -s " + OPCRulesForm.getSrcIp() + " -d " + OPCRulesForm.getDstIp() + " --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
    }
    if (addOrDelete) {
        flag = "DPI1";
        sqlRule = "INSERT INTO OPC values " + "('" + dev.getDevIP() + "','" + OPCRulesForm.getDstIp() + "','" + OPCRulesForm.getSrcIp() + "','" + logFlag + "','ACCEPT')";
    } else {
        flag = "DPI0";
        sqlRule = "DELETE FROM OPC where (dev_IP='" + dev.getDevIP() + "' and dst_IP='" + OPCRulesForm.getDstIp() + "' and src_IP='" + OPCRulesForm.getSrcIp() + "' and method='ACCEPT')";
    }
    var OPCRules = flag + dpiLogRule + " && " + dpiRules;
    sendInfo.sendConfigInfo(dev.getDevIP(), OPCRules);
    dbOperations.dbOperation(sqlRule);
    return (configInfo.listenConfigInfo());
}
exports.configOPCRules = configOPCRules;


/**
 * 用于配置DNP3规则
 * Author：于仁飞
 * @param {*防火墙或被保护设备对象} dev 
 * @param {*DNP3规则对象} OPCRulesForm 
 * @param {*记录日志} logFlag 
 * @param {*true为添加日志，false为删除日志} addOrDelete 
 */
function configDNP3Rules(dev, DNP3Form, logFlag, addOrDelete) {
    var dpiRules0, dpiRules1, dpiLogRule, flag, sqlRule;
    if (DNP3Form.getSrcIp() == "any" && DNP3Form.getDstIp() == "any") {
        dpiRules0 = "iptables -A FORWARD -p tcp " + " --dport 20000 -m state --state NEW -j DROP";
        dpiRules1 = "iptables -A FORWARD -p tcp " + " --dport 20000 -m state --state ESTABLISHED -j DROP";
        dpiLogRule = "iptables -A FORWARD -p tcp " + " --dport 20000 -m state --state new -j LOG --log-prefix " + "\"" + "DROP&DNP3&illegal " + "\"";
    }
    if (DNP3Form.getSrcIp() != "any" && DNP3Form.getDstIp() == "any") {
        dpiRules0 = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " --dport 20000 -m state --state NEW -j DROP";
        dpiRules1 = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " --dport 20000 -m state --state ESTABLISHED -j DROP";
        dpiLogRule = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " --dport 20000 -m state --state new -j LOG --log-prefix " + "\"" + "DROP&DNP3&illegal " + "\"";
    }
    if (DNP3Form.getSrcIp() == "any" && DNP3Form.getDstIp() != "any") {
        dpiRules0 = "iptables -A FORWARD -p tcp" + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state NEW -j DROP";
        dpiRules1 = "iptables -A FORWARD -p tcp" + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state ESTABLISHED -j DROP";
        dpiLogRule = "iptables -A FORWARD -p tcp" + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state new -j LOG --log-prefix " + "\"" + "DROP&DNP3&illegal " + "\"";
    }
    if (DNP3Form.getSrcIp() != "any" && DNP3Form.getDstIp() != "any") {
        dpiRules0 = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state NEW -j DROP";
        dpiRules1 = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state ESTABLISHED -j DROP";
        dpiLogRule = "iptables -A FORWARD -p tcp -s " + DNP3Form.getSrcIp() + " -d " + DNP3Form.getDstIp() + " --dport 20000 -m state --state new -j LOG --log-prefix " + "\"" + "DROP&DNP3&illegal " + "\"";
    }
    if (addOrDelete) {
        flag = "DPI1";
        sqlRule = "INSERT INTO dnp3 values " + "('" + dev.getDevIP() + "','" + DNP3Form.getDstIp() + "','" + DNP3Form.getSrcIp() + "','" + logFlag + "','DROP')";
    } else {
        flag = "DPI0";
        sqlRule = "DELETE FROM DNP3 where (dev_IP='" + dev.getDevIP() + "' and dst_IP='" + DNP3Form.getDstIp() + "' and src_IP='" + DNP3Form.getSrcIp() + "' and method='DROP')";
    }
    var DNP3Rules = flag + dpiLogRule + " && " + dpiRules0 + " && " + dpiRules1;
    sendInfo.sendConfigInfo(dev.getDevIP(), DNP3Rules);
    dbOperations.dbOperation(sqlRule);
    return (configInfo.listenConfigInfo());
}
exports.configDNP3Rules = configDNP3Rules;

/**
 * 清除配置的所有规则
 * @param {*防火墙或者被保护设备对象} dev 
 */
function clearAllRules(dev) {
    var clearRule = "iptables -F && iptables -X && iptables -Z && iptables-restore</etc/iptables.up.rules";
    sendInfo.sendConfigInfo(dev.getDevIP(), clearRule);
}
exports.clearAllRules = clearAllRules;