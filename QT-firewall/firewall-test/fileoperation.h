#ifndef FILEOPERATION_H
#define FILEOPERATION_H

/*
 * 文件操作类
 * 采用json文件存取规则
 * 成员函数:参数为以空格区分开的std字符串
 * Author:于仁飞
 * Date:2018-5-8
*/

#include <string>
#include <iostream>
#include <list>
#include <QJsonDocument>
#include <QJsonObject>
#include <QTextStream>
#include <QFile>
#include <QDir>
#include <QString>
#include <QHostAddress>

/*
 * 采用json文件格式存取规则，保存在config目录下，如果存在冲突则可以在构造函数中修改目录名称
 * ruelstr 按空格区分，0为规则大类，会作为规则文件夹名称，1为规则名称，2为防火墙MAC地址，后续为规则详细内容
添加一个新的规则仅需要更改实现中的buildObject()函数，其中key是可以随意命名
例如：modbusTcp规则属于DPI大类，那么其传入的rulestr应该为"DPI modbustcp fwMAC"(这三个字段固定)+" 规则各字段值 "
*/

class FileOperation
{
public:
    FileOperation();
    bool addRule(const std::string& ruleStr) const;//添加规则
    bool deleteRule(const std::string& ruleStr) const;//删除规则
    bool updateRule(const std::string& oldruleStr,const std::string& newruleStr) const;//更新规则
    std::list<QJsonObject> searchRule(const std::string& ruleStr) const;//查询规则
};

#endif // FILEOPERATION_H
