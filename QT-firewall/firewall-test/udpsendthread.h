#ifndef UDPSENDTHREAD_H
#define UDPSENDTHREAD_H

#include <QObject>
#include <QThread>
#include <QTimer>
#include <QUdpSocket>
#include <iostream>
#include <qDebug>
#include <QString>


#include "qcomminfo.h"

/**
 * @brief The UdpSendThread class
 *  设置定时发送Udp消息。发送仍然在主线程，但定时器在其他线程。
 *
 */
class UdpSendThread:public QThread
{
    Q_OBJECT
private:
    QTimer *timer;
    int interval_ms; // 发送间隔，参数必须大于0。默认为5000ms
    int sendTimes; //发送次数，默认为 1
    int remainSendTimes;    // 剩余发送次数
    bool startedFlag;   // 启动结果
    char *sendMessage;  // 待发送消息
    int sendLength; // 待发送消息的长度
    QString dstIp;  // 待发送的目标IP
    int dstPort; // 待发送的目标端口
    bool sendOnstartFlag; // true：启动后立即发送一次；false：只有时间超时之后再发送。默认为true
public:
    /**
     * @brief UdpSendThread 构造函数，
     * @param parent 传递父对象可使本对象随父对象自动析构
     *  实例化后，必须进行以下，才能启动发送：
     *      .setSendMessage() 设置待发送消息及其长度
     *      .setDstInfo() 设置发送的目的地
     *  可以选择设置：
     *      .setIntervalMs() 每次发送间隔。默认5000ms
     *      .setSendTimes() 发送次数。默认1次
     *      .setSendOnStartFlag 启动之后是否立即发送一条消息，默认为true。
     *
     */
    UdpSendThread(QObject *parent);

    /**
      * 析构函数。
      * 注意，析构函数中会调用stop()，可能导致触发stoped信号
      */
    ~UdpSendThread();

public:
    /**
     * @brief setIntervalMs 设置每次发送时间间隔
     * @param interval_ms 必须为正值，单位ms
     */
    void setIntervalMs(int interval_ms);
    int getIntervalMs() const;

    /**
     * @brief setSendTimes 设置发送的次数
     * @param value 若为负值，则无限制发送，若为正值，则发送次数为正值。不能为0。
     */
    void setSendTimes(int value);
    int getSendTimes() const;

    /**
     * @brief getRemainSendTimes 获取当前剩余的发送次数
     * @return
     */
    int getRemainSendTimes() const;

    /**
     * @brief isStarted 判断是否启动
     * @return
     */
    bool isStarted() const;

    /**
     * @brief setSendMessage 设置要发送的数据
     * @param msg 待发送数据
     * @param msgLen 待发送数据的长度
     */
    void setSendMessage(const char *msg,int msgLen);

    /**
     * @brief setDstInfo 设置要发送的目标
     * @param dstIp 目标Ip
     * @param dstPort 目标端口
     */
    void setDstInfo(QString dstIp,int dstPort);

    /**
     * @brief checkDstInfo 检查是否设置了目标信息
     * @return
     */
    bool checkDstInfo() const;

    /**
     * @brief checkSendMsg 检查是否设置了发送的消息
     * @return
     */
    bool checkSendMsg();

    /**
     * @brief setSendOnStartFlag 设置是否在启动之后立即发送一次
     * @param immediately
     */
    void setSendOnStartFlag(bool immediately);
    bool getSendOnStartFlag() const;

signals:
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

private slots:

    /**
     * 私有槽，勿使用。
     * @brief whenTimeout 当计时器超时后，会触发此槽，然后，在此槽函数内，发送消息，设置剩余发送次数，并发送timeout()信号。
     *
     */
    void whenTimeout();

signals:

    /**
      * @brief started 启动结果信号，
      * @param result 为启动结果，1为启动成功，0为启动失败。
      *  当调用start()函数后，会触发此信号，result为启动结果
      */
    void started(bool result);

    /**
     * @brief timeout 超时信号
     *  当计时器达到设定超时时间之后，发送此信号。
     */
    void timeout();

    /**
     * @brief stoped 停止信号
     *  当调用stop()函数时，会触发此信号。
     *      注意，在析构函数中，会调用stop()函数。
     */
    void stoped();

    /**
     * @brief sended 每次发送了消息，都会出发此信号，表示已发送
     */
    void sended();

public slots:

    /**
     * @brief start 使用相关set函数或者以下重载版本，设置好相关参数后(必须设置目标信息和待发送消息)，启动定时发送。
     *  注意：如果设置了启动时发送，并且只发送一次，则不启动定时器，而是直接发出去。不会触发started()信号
     */
    void start();
    void start(const QString &dstIp,const int dstPort,const char * msg,const int msgLength);
    void start(const int interval_ms,const int sendTimes);
    void start(const int interval_ms);

    /**
     * @brief stop 如果需要停止定时发送，则调用本函数。
     *  会触发stoped()信号
     *  注意，停止之后，会重置剩余发送次数。
     */
    void stop();

protected:

    /**
     * @brief run 开启事件循环。
     */
    void run();
};



#endif // UDPSENDTHREAD_H
