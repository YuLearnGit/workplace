module.exports=modbusTcpRulesForm;

/**
 * modbusTcp规则
 * Author：于仁飞
 * @param {*目的IP地址,any表示任意} dstIp 
 * @param {*源IP地址,any表示任意} srcIp 
 * @param {*起始线圈地址} minAddr 
 * @param {*结束线圈地址} maxAddr 
 * @param {*功能码} func 
 * @param {*最小数值} minData 
 * @param {*最大数值} maxData 
 */

function modbusTcpRulesForm(dstIp,srcIp,minAddr,maxAddr,func,minData,maxData){
this.dstIp=dstIp;
this.srcIp=srcIp;
this.minAddr=minAddr;
this.maxAddr=maxAddr;
this.func=func;
this.minData=minData;
this.maxData=maxData;

this.getDstIp=function(){
    return dstIp;
};

this.getSrcIp=function(){
    return srcIp;
};

this.getMinAddr=function(){
    return minAddr;
};

this.getMaxAddr=function(){
    return maxAddr;
};

this.getFunc=function(){
    return func;
};

this.getMinData=function(){
    return minData;
};

this.getMaxData=function(){
    return maxData;
};
}