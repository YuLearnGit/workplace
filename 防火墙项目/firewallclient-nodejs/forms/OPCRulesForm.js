module.exports = OPCRulesForm;

/**
 * OPC规则
 * Author：于仁飞
 * @param {*目的IP地址,any表示任意} dstIp 
 * @param {*源IP地址,any表示任意} srcIp 
 */
function OPCRulesForm(dstIp, srcIp) {
    this.dstIp = dstIp;
    this.srcIp = srcIp;

    this.getDstIp = function () {
        return dstIp;
    };

    this.getSrcIp = function () {
        return srcIp;
    };
}