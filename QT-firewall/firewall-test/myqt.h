#ifndef MYQT_H
#define MYQT_H

#include <QObject>
#include <QThread>
#include <iostream>
#include <QTimer>
#include <iostream>
#include <QUdpSocket>


#ifndef MYQTHREAD_H
#define MYQTHREAD_H

class MyQThread : public QThread
{
    Q_OBJECT
public:
    MyQThread(QObject *parent = nullptr);
    ~MyQThread();
};

#endif // MYQTHREAD_H



#ifndef MYQTIMER_H
#define MYQTIMER_H

class MyQTimer : public QTimer
{
    Q_OBJECT
public:
    MyQTimer(QObject *parent = nullptr);
    ~MyQTimer();
public slots:
    void deleteLater();
    void start(int mesc);
    void start();
};

#endif // MYQTIMER_H



#ifndef MYQUDPSOCKET_H
#define MYQUDPSOCKET_H

class MyQUdpSocket:public QUdpSocket
{
    Q_OBJECT
public:
    MyQUdpSocket(QObject *parent = nullptr);
    ~MyQUdpSocket();
signals:
    void bindResult(bool);
public slots:
    void close();
    void deleteLater();
    bool bind(QString bindIp, quint16 bindPort);
};

#endif // MYQUDPSOCKET_H


#endif // MYQT_H
