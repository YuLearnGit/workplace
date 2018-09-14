#include "protectdeviceform.h"

std::string ProtectDeviceForm::getdev_type(const std::string& devmac) const
{
    QString check_devmac = QString::fromStdString(devmac);
    QStringList devmacList = check_devmac.split(":");
    QString dev = devmacList[0]+"-"+devmacList[1]+"-"+devmacList[2];
    std::string type("unknown type!");
    QFile file("macs.json");
    QString val("");
    QByteArray byte_array;
    if(file.open(QIODevice::ReadOnly | QIODevice::Text))
    {
        qDebug()<<"open success";
        val = file.readAll();
        byte_array = val.toLatin1();
    }
    file.close();
    QJsonParseError json_error;
    QJsonDocument parse_document = QJsonDocument::fromJson(byte_array,&json_error);
    if(!parse_document.isNull() && json_error.error == QJsonParseError::NoError)
    {
        if(parse_document.isObject())
        {
            QJsonObject obj = parse_document.object();
            QJsonValue value = obj.value("RECORDS");
//            qDebug()<<value.toString();
            if(value.isArray())
            {
                QJsonArray test = value.toArray();
                QJsonObject tmp;
                for(auto item : test)
                {
                    tmp = item.toObject();
                    QJsonValue mac_value = tmp.value("Macs");
                    if(dev == mac_value.toString())
                        type = tmp.value("Manufacturers").toString().toStdString();
                }
            }
        }
    }
    std::cout<<type<<std::endl;
    return type;
}
