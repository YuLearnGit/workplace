module.exports=whiteListForm;

/**
 * 白名单规则
 * Author：于仁飞
 * @param {*目的IP地址} dstIp 
 * @param {*源IP地址} srcIp 
 * @param {*目的端口} dstPort 
 * @param {*源端口} srcPort 
 */

function whiteListForm(dstIp,srcIp,dstPort,srcPort){
    this.dstIp=dstIp;
    this.srcIp=srcIp;
    this.dstPort=dstPort;
    this.srcPort=srcPort;

    this.getDstIp=function(){
        return dstIp;
    };

    this.getSrcIp=function(){
        return srcIp;
    };

    this.getDstPort=function(){
        return dstPort;
    };

    this.getSrcPort=function(){
        return srcPort;
    };

}