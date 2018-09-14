#ifndef UDPLISTENERTHREAD_H
#define UDPLISTENERTHREAD_H

#include "myqt.h"

#include <QThread>
#include <QTimer>
#include <QString>
#include <QUdpSocket>
#include <QNetworkDatagram>

#include <iostream>

/**
 * @brief The UdpListenerThread class
 *  自定义的UDP监听类，在线程中运行
 *  实现的功能：
 *      UDP在线程中进行监听，与主线程分开；
 *      可以一直监听某个端口，或者定义超时时间，超时之后自动结束监听；
 */
class UdpListenerThread:public MyQThread
{
    Q_OBJECT
private:
    MyQTimer *timer;
    MyQUdpSocket *listener;

private:
    int timeout_ms; // 设置的超时时间。若timeout_ms > 0，则启动超时自动关闭功能。若timeout_ms <= 0 ,则不使用超时。
    bool onStartFlag; // 是否正在启动监听标志
    bool startedFlag; // 是否启动监听的标志，True：已启动监听，False：未启动监听。
    bool stopingFlag; // 是否正在停止监听标志。
    //    bool onBindFlag; // 是否正在绑定中
    bool bindFlag; // 是否绑定成功
    bool activateTimeoutFlag; // 是否开始超时设置标志。
    QString bindIp; // 要监听的IP
    int bindPort;   // 要监听的端口

public:
    /**
     * @brief UdpListenerThread 构造函数
     * @param bindIp    要监听的IP(使用setBindInfo设置)
     * @param bindPort  要监听的端口
     * @param timeout_ms    超时时间，单位ms。超出时间后，自动停止监听，并发出timeout()信号(使用setTimeoutMs设置)
     * @param parent    父类
     */
    UdpListenerThread(QObject *parent=nullptr);

    /**
      * 析构函数。析构函数中，会调用stop()函数
      */
    ~UdpListenerThread();

public:
    /**
     * @brief isStarted 判断是否启动了监听
     * @return 是否启动了监听
     */
    bool isStarted() const;
    bool isOnStart() const;

    /**
     * @brief setTimeoutMs 设置超时时间。如果启动了，则不能设置此值。
     * @param timeout_ms 超时时间，单位ms。
     */
    void setTimeoutMs(int timeout_ms);
    int getTimeoutMs() const;

    /**
     * @brief setBindInfo 设置要监听的IP和端口
     * @param bindIp      绑定的IP
     * @param bindPort    绑定的Port
     */
    void setBindInfo(const QString &bindIp,const int bindPort);

    /**
     * @brief checkBindInfo 检查是否绑定了监听的IP和Port
     * @return
     */
    bool checkBindInfo();
    QString getBindIp() const;
    void setBindIp(const QString &value);
    int getBindPort() const;
    void setBindPort(int value);

    /**
     * @brief isOnBind 是否正在绑定监听中
     * @return
     */
    //    bool isOnBind()
    //    {
    //        return this->onBindFlag;
    //    }
    /**
     * @brief isBindSuc 是否绑定成功
     * @return
     */
    bool isBindSuc()
    {
        return this->bindFlag;
    }
    /**
     * @brief waitBind 通过死循环，来等待绑定结果，注意，如果没有事件循环，可能会卡死主线程
     * @return
     */
//    bool waitBind()
//    {
//        for(;this->onStartFlag;);
//        return this->isBindSuc();
//    }
signals:

    /**
     *  私有信号，勿使用。
     * @brief startListener 用于启动线程中的Udp监听
     * @param bindIp    监听的IP
     * @param bindPort  监听的端口
     */
    void startListener(QString bindIp,quint16 bindPort);

    /**
     *  私有信号，勿使用。
     * @brief closeListener 用于关闭线程的Udp监听
     */
    void closeListener();

    /**
     *  私有信号，勿使用
     * @brief startTimer 用于启动线程的计时器
     * @param msec  设置定时器时间，单位ms
     */
    void startTimer(int msec);

    /**
     *  私有信号，勿使用。
     * @brief stopTimer 用于关闭线程中的计时器
     */
    void stopTimer();

signals:

    /**
     * @brief started 启动结果信号，
     * @param result 为启动结果，1为启动成功，0为启动失败。
     *  当调用start()函数后，会触发此信号，result为启动结果
     */
    void started(bool result);

    /**
     * @brief timeout 超时信号
     *  当计时器达到设定超时时间之后，在停止监听后，发送此信号。
     */
    void timeout();

    /**
     * @brief stoped 停止信号
     *  当调用stop()函数时，会触发此信号。
     *      注意，在析构函数中，会调用stop()函数。
     */
    void stoped();

    /**
     * @brief receivedUdpData 收到消息后，会触发本槽函数
     * @param msg UDP协议中数据部分的所有值
     * @param srcIp 收到的数据包的源IP
     * @param srcPort 源Port
     */
    void receivedUdpData(QByteArray msg,QString srcIp,int srcPort);

private slots:
    /**
     *  私有槽，勿使用。
     * @brief bindResult 获取线程中listener的绑定是否成功结果
     * @param result 为listener.bind()的返回值。
     */
    void bindResult(bool result);

    /**
     * 私有槽，勿使用。
     * @brief whenTimeout 当计时器超时后，会触发此槽，然后，在此槽函数内，停止监听，并发送timeout()信号。
     *  接收到timer对象的timeout()信号，则自动停止Udp监听，并发送本类的timeout()信号（不触发stoped()信号）。
     */
    void whenTimeout();

    /**
     * 私有槽，勿使用。
     * @brief dataReceived listener接收到数据后，会触发本槽。(直接连接，在线程中处理)
     */
    void received();

public slots:

    /**
     * @brief start 开始监听
     *  并发送started()信号，参数为是否启动成功。
     */
    void start();
    void start(const QString &bindIp,const int bindPort,int timeout_ms);

    /**
     * @brief start 开始监听,重载版本。
     * @param timeout_ms 超时时间。
     *  并发送started()信号，参数为是否启动成功。
     */
    void start(int timeout_ms);

    /**
     * @brief stop 停止监听
     *  并触发stoped()的信号。
     */
    void stop();

    /**
     * @brief deleteLater 实现稍后删除功能
     */
    void deleteLater();

protected:

    /**
     * @brief run 开启事件循环。
     */
    void run();
};



#endif // UDPLISTENERTHREAD_H
