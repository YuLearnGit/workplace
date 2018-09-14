#ifndef QCOMMINFO_H
#define QCOMMINFO_H

#include "udplistenerthread.h"
#include "comminfo.h"
#include "myqt.h"

#include <string>
#include <QDebug>
#include <QObject>
#include <QByteArray>
#include <cstdlib>
#include <ctime>
#include <memory>
#include <cstdio>
#include <queue>

/**
 * @brief The SendInfo class
 *  用于存储待发送消息
 */
class SendInfo
{
public:
    std::string dstIp;
    int dstPort;
    char *cmdTypeByte;
    std::string cmdStr;
    SendInfo(const std::string &dstIp,const int dstPort,const char *cmdTypeByte,const std::string &cmdStr)
    {
        this->dstIp = dstIp;
        this->dstPort = dstPort;
        this->cmdTypeByte = (char *) cmdTypeByte;
        this->cmdStr = cmdStr;
    }
    SendInfo()
    {

    }
    ~SendInfo()
    {

    }
    friend std::ostream & operator << (std::ostream &out, SendInfo &info);
};

/**
 * @brief The QCommSend class
 *  封装了几个静态发送函数
 */
class QCommSend
{
public:

    /**
     * @brief send 发送函数，将data中的数据，全部填充到Udp协议的数据部分，并进行发送。
     * @param dstIp
     * @param dstPort
     * @param data 填充到UDP协议的数据部分
     * @param dataLength
     * @return 发送的数据量
     */
    static int send(const std::string &dstIp,const int dstPort,const char *data,const int dataLength);

    /**
     * @brief sendCmd 发送一条命令
     * @param dstIp
     * @param dstPort
     * @param timeBytes
     * @param cmdTypeByte
     * @param cmdStr 第15字节开始的命令（不包括结束字节）
     * @return 发送的数据量
     */
    static int sendCmd(const std::string &dstIp,const int dstPort,const char *timeBytes,const char *cmdTypeByte,const std::string &cmdStr);

    /**
     * @brief udpSend 实现用传递过来的QUdpSocket进行发送数据，当发送大量数据包的时候，可以避免反复申请QUdpSocket进行的开销（本函数不会关闭传递过来的QUdpSocket）
     * @param udpSocket
     * @param dstIp
     * @param dstPort
     * @param data  填充到UDP协议的数据部分
     * @param dataLength
     * @return 发送的数据量
     */
    static int udpSend(MyQUdpSocket *udpSocket,const std::string &dstIp,const int dstPort,const char *data,const int dataLength);

    /**
     * @brief udpSendCmd 按协议组合命令并发送，使用传递过来的QUdpSocket进行发送数据。
     * @param udpsocket
     * @param dstIp
     * @param dstPort
     * @param timeBytes  时间校验位的数据
     * @param cmdTypeByte
     * @param cmdStr 要发送的命令, 第15字节开始的命令（不包括结束字节）
     * @return 发送的数据量
     */
    static int udpSendCmd(MyQUdpSocket *udpsocket,const std::string &dstIp,const int dstPort,const char *timeBytes,const char *cmdTypeByte,const std::string &cmdStr);
};

/**
 * @brief The QCommInfo class
 *      使用sendCmd()发送一条命令，并自动开始监听。
 *      使用appendCmd()添加多条要发送的命令（只添加一条也可以），并使用sendCmd()发送出去，并统一监听返回包
 *      注意，调用sendCmd()函数之后，会使得之前调用sendCmd()发出去的命令的返回包（如果有）得不到响应（原因：每次调用sendCmd()使用了新的时间字符串）
 */
class QCommInfo:public UdpListenerThread
{
    Q_OBJECT
public:
    /**
     * @brief QCommInfo 构造函数。
     * @param parent
     *      实例化后，除非关闭监听，否则在发送数据前，应该调用以下方法（在需要的地方，可重新设置这些值）
     *      .setBindInfo(); 详见 UdpListenerThread 类
     *      .setTimeoutMs(); 详见 UdpListenerThread 类
     *  可选设置：
     *      .setAutoClose(); 接受到认可的第一条数据后自动关闭。
     *  开启或关闭监听方法：
     *      .setActivateListener();
     */
    QCommInfo(QObject *parent);
private:
    std::queue<SendInfo> sendQueue; // 待发送命令队列
    bool onSendFlag;
public:
    unsigned int randomNum;
protected:
    bool autoCloseFlag;
    bool activateListenerFlag;
public:
    /**
     * @brief appendCmd 添加要发送的命令到待发送命令队列
     * @param dstIp 要发送的IP
     * @param dstPort 要发送的Port
     * @param cmdTypeByte 要发送的命令类型，协议中第14字节。此值可选范围已定义为 CommInfo类中 TypeByte 结尾的静态成员。
     * @param cmdStr
     */
    void appendCmd(const std::string &dstIp,const int dstPort,const char *cmdTypeByte,const std::string &cmdStr);

    /**
     * @brief sendCmd 一次只发送一条命令，然后监听这条数据的返回包（如果有）。
     *       如果需要同时发送多条数据（例如设备扫描时），并统一监听这些数据的返回包，请使用appendCmd()添加要发送的消息，添加完后，使用本函数的重载版本进行发送
     *      注意：调用了sendCmd()函数进行发送后，则之前调用sendCmd()的返回数据包将不被解析（如果有）（原因是每次调用sendCmd将使用新的时间字符串，则返回函数只能匹配新的随机数）。
     * @param dstIp
     * @param dstPort
     * @param cmdTypeByte
     * @param cmdStr
     */
    void sendCmd(const std::string &dstIp,const int dstPort,const char *cmdTypeByte,const std::string &cmdStr);

    /**
     * @brief sendCmd 上一函数的重载版本，可将所有appendCmd()添加进待发送命令队列的命令进行发送
     */
    void sendCmd();
    /**
     * @brief isOnSend 判断是否正在发送
     * @return
     */
    bool isOnSend() const;
private:

    /** 私有函数
     * @brief sendQueue 发送队列中的消息（命令队列中构造的命令全部使用同一时间字符串，）
     * @return 此时间字符串计算的随机数值（则返回的数据包，全部有此随机数值校验）
     */
    int sendCmdQueue();

public:

    /**
     * @brief setAutoClose 是否在接收数据第一条后，自动关闭监听。默认为是。
     * @param closeWhenReceived
     */
    void setAutoClose(bool autoCloseFlag);
    bool isAutoClose() const;

    /**
     * @brief setActivateListener 设置是否启用监听，是，则在发送数据之前，将启动监听；否，只发送数据，不监听返回消息。默认为是。
     *                              如果不需要监听，也可以只用CommInfo::composeUdpData()构造好数据，然后用UdpSend::send()发送出去即可。
     * @param activate
     */
    void setActivateListener(bool activate);
    bool isActivateListener() const;

private slots:
    /**
     * @brief whenListenerStarted 当UdpListenerThread监听之后的槽函数
     * @param result
     */
    void whenListenerStarted(bool result);
private slots:

    /**
     * @brief receivedBytes 私有接收返回数据槽，会调用虚函数handResultData()来处理返回的结果数据（第15字节到结束字节之前，不包括结束字节）
     * @param dataBytes UDP协议中数据部分的所有值
     * @param srcIp 收到的数据包的源IP
     * @param srcPort 源Port
     */
    void receivedBytes(QByteArray dataBytes, QString srcIp, int srcPort);

private:
    /** 虚函数
     * @brief handResultData 处理返回包结果数据，由receivedBytes()槽调用
     * @param result 返回结果状态，true：返回成功，false：返回失败。目前未设置false值
     * @param commandTypeByte 返回包的命令类型，即第14字节
     * @param commandBytes 返回包的命令结果字符串，即第15字节到结束字节之前（不包含结束字节）
     * @param commandLength  commandBytes的数据长度（）
     *  注意：若要实现其他类型命令的操作，在CommInfo函数中，定义好第14字节的命令类型。然后，继承本类，重载本函数，可处理命令的返回结果。（参考ConfigSyslogServer的实现）
     */
    virtual void handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp,const int srcPort);

public: // 发送与iptables配置命令有关的操作，后期此部分会脱离出去
    void sendIptablesCmd(const std::string dstIp,const int dstPort,const std::string cmd)
    {
        QCommInfo::sendCmd(dstIp,dstPort,CommInfo::iptablesCmdTypeByte,cmd);
    }

    //public: // 发送扫描设备的命令
    //    void sendScanDeviceCmd(const std::string dstIp,const int dstPort,const std::string cmd)
    //    {
    //        QCommInfo::sendCmd(dstIp,dstPort,CommInfo::scanDeviceCmdTypeByte,cmd);
    //    }
};



#endif // QCOMMINFO_H
