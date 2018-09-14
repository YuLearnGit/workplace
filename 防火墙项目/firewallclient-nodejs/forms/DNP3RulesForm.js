module.exports=DNP3RulesForm;

/**
 * DNP3规则
 * Author：于仁飞
 * @param {*规则目的IP地址} dstIp 
 * @param {*规则源IP地址} srcIp 
 */

function DNP3RulesForm(dstIp,srcIp){
this.dstIp=dstIp;
this.srcIp=srcIp;

this.getDstIp=function(){
    return dstIp;
};

this.getSrcIp=function(){
    return srcIp;
};
}