#include "fileoperation.h"

QDir dir;
FileOperation::FileOperation()
{
    //如果不存在config目录则创建之
    if(!dir.exists("config"))
    {
        dir.mkdir("config");
    }
}
/*
 * 在进行文件操作之前进行一些初始化工作
*/
QStringList before_file_operate(const std::string& ruleStr)
{
    QString QruleStr = QString::fromStdString(ruleStr);
    QStringList ruleStrList = QruleStr.split(" ");
    //判断此类规则目录是否存在，若不存在则创建之
    if(!dir.exists("config/"+ruleStrList[0]))
    {
        dir.mkdir("config/"+ruleStrList[0]);
        dir.cd("config/"+ruleStrList[0]);
    }
    //判断规则类目录下的某种规则json文件是否存在，不存在则创建之
    QFile file("config/"+ruleStrList[0]+"/"+ruleStrList[1]+".json");
    if(!file.exists())
    {
        file.open(QIODevice::WriteOnly);
        file.close();
    }
    return ruleStrList;
}
/*
 * 根据规则前缀构造规则对象，用于添加、删除、查询规则
*/
QString buildObject(const QStringList& ruleList)
{
    if(ruleList[1] == "modbustcp")
    {
        QJsonObject modjson
        {
            {"applyDevMAC",ruleList[2]},
            {"rule_dstIP",ruleList[3]},
            {"rule_srcIP",ruleList[4]},
            {"min_addr",ruleList[5]},
            {"max_addr",ruleList[6]},
            {"function code",ruleList[7]},
            {"min_data",ruleList[8]},
            {"max_data",ruleList[9]},
            {"logFlag",ruleList[10]}
        };
        QJsonDocument document(modjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1] == "opc")
    {
        QJsonObject opcjson
        {
            {"applyDevMAC",ruleList[2]},
            {"rule_dstIP",ruleList[3]},
            {"rule_srcIP",ruleList[4]},
            {"logFlag",ruleList[5]}
        };
        QJsonDocument document(opcjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="dnp3")
    {
        QJsonObject dnpjson
        {
            {"applyDevMAC",ruleList[2]},
            {"rule_dstIP",ruleList[3]},
            {"rule_srcIP",ruleList[4]},
            {"logFlag",ruleList[5]}
        };
        QJsonDocument document(dnpjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="white")
    {
        QJsonObject whitejson
        {
            {"applyDevMAC",ruleList[2]},
            {"rule_dstIP",ruleList[3]},
            {"rule_srcIP",ruleList[4]},
            {"rule_dstPORT",ruleList[5]},
            {"rule_srcPORT",ruleList[6]}
        };
        QJsonDocument document(whitejson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="ApplicationProtocol")
    {
        QJsonObject apcjson
        {
            {"applyDevMAC",ruleList[2]},
            {"protocol",ruleList[3]},
            {"status",ruleList[4]}
        };
        QJsonDocument document(apcjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="ConnectionControl")
    {
        QJsonObject cncjson
        {
            {"applyDevMAC",ruleList[2]},
            {"rule_srcIP",ruleList[3]},
            {"rule_dstIP",ruleList[4]},
            {"rule_sport",ruleList[5]},
            {"rule_dport",ruleList[6]},
            {"number",ruleList[7]}
        };
        QJsonDocument document(cncjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="StateDetection")
    {
        QJsonObject stdjson
        {
            {"applyDevMAC",ruleList[2]},
            {"protocol",ruleList[3]},
            {"rule_srcIP",ruleList[4]},
            {"rule_dstIP",ruleList[5]},
            {"rule_sport",ruleList[6]},
            {"rule_dport",ruleList[7]}
        };
        QJsonDocument document(stdjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="SNAT")
    {
        QJsonObject snatjson
        {
            {"applyDevMAC",ruleList[2]},
            {"ethName",ruleList[3]},
            {"ethIP",ruleList[4]},
            {"srcIP",ruleList[5]}
        };
        QJsonDocument document(snatjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="DNAT")
    {
        QJsonObject dnatjson
        {
            {"applyDevMAC",ruleList[2]},
            {"originDstIP",ruleList[3]},
            {"originDstPort",ruleList[4]},
            {"mapIP",ruleList[5]},
            {"mapPort",ruleList[6]}
        };
        QJsonDocument document(dnatjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="DefaultRoute")
    {
        QJsonObject prtjson
        {
            {"applyDevMAC",ruleList[2]},
            {"ethName",ruleList[3]},
            {"gateway",ruleList[4]}
        };
        QJsonDocument document(prtjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="HostRoute")
    {
        QJsonObject prtjson
        {
            {"applyDevMAC",ruleList[2]},
            {"Host",ruleList[3]},
            {"ethName",ruleList[4]},
            {"gateway",ruleList[5]}
        };
        QJsonDocument document(prtjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="NetRoute")
    {
        QJsonObject prtjson
        {
            {"applyDevMAC",ruleList[2]},
            {"net",ruleList[3]},
            {"netmask",ruleList[4]},
            {"ethName",ruleList[5]},
            {"gateway",ruleList[6]}
        };
        QJsonDocument document(prtjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="fwlist")   //2018-07-03 20:29添加用于存放防火墙及其设备信息 谢辉
    {
        QJsonObject fwlistjson
        {
            {"fw_ip", ruleList[2]},
            {"fw_mac", ruleList[3]},
            {"dev_ip", ruleList[4]},
            {"dev_mac", ruleList[5]},
            {"dev_type", ruleList[6]}
        };
        QJsonDocument document(fwlistjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="fwproperty")   //2018-07-03 20:32添加用于存放防火墙属性信息 谢辉
    {
        QJsonObject fwpropertyjson
        {
            {"fw_name", ruleList[2]},
            {"fw_ID", ruleList[3]},
            {"fw_IP", ruleList[4]},
            {"fw_description", ruleList[5]},
        };
        QJsonDocument document(fwpropertyjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    else if(ruleList[1]=="useraccount") //2017-07-05 14:02添加用于存放用户信息 谢辉
    {
        QJsonObject useraccountjson
        {
            {"UserID", ruleList[2]},
            {"UserName", ruleList[3]},
            {"Password", ruleList[4]},
            {"Purview", ruleList[5]},
        };
        QJsonDocument document(useraccountjson);
        QByteArray byte_array = document.toJson(QJsonDocument::Compact);
        QString jsonStr(byte_array);
        return jsonStr;
    }
    return NULL;
}
/*
 * 判断规则是已经否存在，true表示存在
*/
bool rule_exist(const QStringList& ruleList, const QString& jsonStr)
{
    bool exist_flag = false;
    QFile file("config/"+ruleList[0]+"/"+ruleList[1]+".json");
    QTextStream in(&file);
    if(file.open(QIODevice::ReadOnly))
    {
        while(!in.atEnd())
        {
            QString line = in.readLine();
            if(jsonStr == line)
            {
                qDebug()<<"rule exists!";
                exist_flag = true;
            }
        }
        file.close();
    }
    return exist_flag;
}
/*
 * 将QJsonObject存入文件
*/
bool saveObject(const QStringList& ruleList, const QString& jsonStr)
{
    QFile file("config/"+ruleList[0]+"/"+ruleList[1]+".json");
    if(file.open(QIODevice::Append))
    {
        QTextStream in(&file);
        in<<jsonStr<<"\n";
        file.flush();
        file.close();
        return true;
    }
    else
        return false;
}
bool deleteObject(const QStringList& ruleList, const QString& jsonStr)
{
    bool flag = false;
    //如果不存在字符串则返回false
    if(!rule_exist(ruleList,jsonStr))
        return flag;
    QFile file("config/"+ruleList[0]+"/"+ruleList[1]+".json");
    QStringList tmp;
    QTextStream in(&file);
    //将要删除规则文件内的规则字符串缓存区
    if(file.open(QIODevice::ReadOnly))
    {
        while(!in.atEnd())
        {
            QString line = in.readLine();
            if(line != jsonStr)
                tmp.append(line);
        }
        file.close();
    }
    //以只读方式打开文件，将缓存区内容逐个写入
    if(file.open(QIODevice::WriteOnly))
    {
        for(auto item : tmp)
        {
            QTextStream in(&file);
            in<<item<<"\n";
        }
        file.flush();
        file.close();
        flag = true;
    }
    return flag;
}
/*
 * 添加规则
*/
bool FileOperation::addRule(const std::string& ruleStr) const
{
    //若规则为空则返回false
    if(ruleStr == "")
        return false;
    QStringList ruleList = before_file_operate(ruleStr);
    //构造json对象
    QString jsonStr = buildObject(ruleList);
    //首先判断是否已经存在此规则，若存在则返回false
    if(rule_exist(ruleList,jsonStr))
        return false;
    if(saveObject(ruleList,jsonStr))
        return true;
    else
        return false;

}
/*删除规则*/
bool FileOperation::deleteRule(const std::string& ruleStr) const
{
    //若规则为空则返回false
    if(ruleStr == "")
        return false;
    QStringList ruleList = before_file_operate(ruleStr);
    QString jsonStr = buildObject(ruleList);
    if(deleteObject(ruleList,jsonStr))
        return true;
    else
        return false;


}
/*更新规则 */
bool FileOperation::updateRule(const std::string& oldruleStr,const std::string& newruleStr) const
{
    if(deleteRule(oldruleStr) && addRule(newruleStr))
        return true;
    else
        return false;
}

/*查询规则*/
std::list<QJsonObject> FileOperation::searchRule(const std::string& rule_pro) const
{
    std::list<QJsonObject> ruleObjectList;
    //若规则为空则返回false
    if(rule_pro != "")
    {
        QStringList rule_pro_list = before_file_operate(rule_pro);
        QFile file("config/"+rule_pro_list[0]+"/"+rule_pro_list[1]+".json");
        QTextStream in(&file);
        if(file.open(QIODevice::ReadOnly))
        {
            while(!in.atEnd())
            {
                QByteArray byte_array = in.readLine().toLatin1();
                QJsonParseError json_error;
                QJsonDocument parse_document = QJsonDocument::fromJson(byte_array,&json_error);
                if(!parse_document.isNull() && json_error.error == QJsonParseError::NoError)
                {
                    if(parse_document.isObject())
                    {
                        QJsonObject obj = parse_document.object();
                        ruleObjectList.push_back(obj);
                    }
                }
            }
            file.close();
        }
    }

    return ruleObjectList;

}
