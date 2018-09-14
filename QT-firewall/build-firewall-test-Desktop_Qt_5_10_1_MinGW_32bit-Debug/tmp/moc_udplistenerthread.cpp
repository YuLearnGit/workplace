/****************************************************************************
** Meta object code from reading C++ file 'udplistenerthread.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.10.1)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../firewall-test/udplistenerthread.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'udplistenerthread.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.10.1. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_UdpListenerThread_t {
    QByteArrayData data[24];
    char stringdata0[219];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_UdpListenerThread_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_UdpListenerThread_t qt_meta_stringdata_UdpListenerThread = {
    {
QT_MOC_LITERAL(0, 0, 17), // "UdpListenerThread"
QT_MOC_LITERAL(1, 18, 13), // "startListener"
QT_MOC_LITERAL(2, 32, 0), // ""
QT_MOC_LITERAL(3, 33, 6), // "bindIp"
QT_MOC_LITERAL(4, 40, 8), // "bindPort"
QT_MOC_LITERAL(5, 49, 13), // "closeListener"
QT_MOC_LITERAL(6, 63, 10), // "startTimer"
QT_MOC_LITERAL(7, 74, 4), // "msec"
QT_MOC_LITERAL(8, 79, 9), // "stopTimer"
QT_MOC_LITERAL(9, 89, 7), // "started"
QT_MOC_LITERAL(10, 97, 6), // "result"
QT_MOC_LITERAL(11, 104, 7), // "timeout"
QT_MOC_LITERAL(12, 112, 6), // "stoped"
QT_MOC_LITERAL(13, 119, 15), // "receivedUdpData"
QT_MOC_LITERAL(14, 135, 3), // "msg"
QT_MOC_LITERAL(15, 139, 5), // "srcIp"
QT_MOC_LITERAL(16, 145, 7), // "srcPort"
QT_MOC_LITERAL(17, 153, 10), // "bindResult"
QT_MOC_LITERAL(18, 164, 11), // "whenTimeout"
QT_MOC_LITERAL(19, 176, 8), // "received"
QT_MOC_LITERAL(20, 185, 5), // "start"
QT_MOC_LITERAL(21, 191, 10), // "timeout_ms"
QT_MOC_LITERAL(22, 202, 4), // "stop"
QT_MOC_LITERAL(23, 207, 11) // "deleteLater"

    },
    "UdpListenerThread\0startListener\0\0"
    "bindIp\0bindPort\0closeListener\0startTimer\0"
    "msec\0stopTimer\0started\0result\0timeout\0"
    "stoped\0receivedUdpData\0msg\0srcIp\0"
    "srcPort\0bindResult\0whenTimeout\0received\0"
    "start\0timeout_ms\0stop\0deleteLater"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_UdpListenerThread[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
      16,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       8,       // signalCount

 // signals: name, argc, parameters, tag, flags
       1,    2,   94,    2, 0x06 /* Public */,
       5,    0,   99,    2, 0x06 /* Public */,
       6,    1,  100,    2, 0x06 /* Public */,
       8,    0,  103,    2, 0x06 /* Public */,
       9,    1,  104,    2, 0x06 /* Public */,
      11,    0,  107,    2, 0x06 /* Public */,
      12,    0,  108,    2, 0x06 /* Public */,
      13,    3,  109,    2, 0x06 /* Public */,

 // slots: name, argc, parameters, tag, flags
      17,    1,  116,    2, 0x08 /* Private */,
      18,    0,  119,    2, 0x08 /* Private */,
      19,    0,  120,    2, 0x08 /* Private */,
      20,    0,  121,    2, 0x0a /* Public */,
      20,    3,  122,    2, 0x0a /* Public */,
      20,    1,  129,    2, 0x0a /* Public */,
      22,    0,  132,    2, 0x0a /* Public */,
      23,    0,  133,    2, 0x0a /* Public */,

 // signals: parameters
    QMetaType::Void, QMetaType::QString, QMetaType::UShort,    3,    4,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Int,    7,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Bool,   10,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::QByteArray, QMetaType::QString, QMetaType::Int,   14,   15,   16,

 // slots: parameters
    QMetaType::Void, QMetaType::Bool,   10,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::QString, QMetaType::Int, QMetaType::Int,    3,    4,   21,
    QMetaType::Void, QMetaType::Int,   21,
    QMetaType::Void,
    QMetaType::Void,

       0        // eod
};

void UdpListenerThread::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        UdpListenerThread *_t = static_cast<UdpListenerThread *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->startListener((*reinterpret_cast< QString(*)>(_a[1])),(*reinterpret_cast< quint16(*)>(_a[2]))); break;
        case 1: _t->closeListener(); break;
        case 2: _t->startTimer((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 3: _t->stopTimer(); break;
        case 4: _t->started((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 5: _t->timeout(); break;
        case 6: _t->stoped(); break;
        case 7: _t->receivedUdpData((*reinterpret_cast< QByteArray(*)>(_a[1])),(*reinterpret_cast< QString(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 8: _t->bindResult((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 9: _t->whenTimeout(); break;
        case 10: _t->received(); break;
        case 11: _t->start(); break;
        case 12: _t->start((*reinterpret_cast< const QString(*)>(_a[1])),(*reinterpret_cast< const int(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 13: _t->start((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 14: _t->stop(); break;
        case 15: _t->deleteLater(); break;
        default: ;
        }
    } else if (_c == QMetaObject::IndexOfMethod) {
        int *result = reinterpret_cast<int *>(_a[0]);
        {
            typedef void (UdpListenerThread::*_t)(QString , quint16 );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::startListener)) {
                *result = 0;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::closeListener)) {
                *result = 1;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)(int );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::startTimer)) {
                *result = 2;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::stopTimer)) {
                *result = 3;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)(bool );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::started)) {
                *result = 4;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::timeout)) {
                *result = 5;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::stoped)) {
                *result = 6;
                return;
            }
        }
        {
            typedef void (UdpListenerThread::*_t)(QByteArray , QString , int );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpListenerThread::receivedUdpData)) {
                *result = 7;
                return;
            }
        }
    }
}

QT_INIT_METAOBJECT const QMetaObject UdpListenerThread::staticMetaObject = {
    { &MyQThread::staticMetaObject, qt_meta_stringdata_UdpListenerThread.data,
      qt_meta_data_UdpListenerThread,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *UdpListenerThread::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *UdpListenerThread::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_UdpListenerThread.stringdata0))
        return static_cast<void*>(this);
    return MyQThread::qt_metacast(_clname);
}

int UdpListenerThread::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = MyQThread::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 16)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 16;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 16)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 16;
    }
    return _id;
}

// SIGNAL 0
void UdpListenerThread::startListener(QString _t1, quint16 _t2)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void UdpListenerThread::closeListener()
{
    QMetaObject::activate(this, &staticMetaObject, 1, nullptr);
}

// SIGNAL 2
void UdpListenerThread::startTimer(int _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void UdpListenerThread::stopTimer()
{
    QMetaObject::activate(this, &staticMetaObject, 3, nullptr);
}

// SIGNAL 4
void UdpListenerThread::started(bool _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}

// SIGNAL 5
void UdpListenerThread::timeout()
{
    QMetaObject::activate(this, &staticMetaObject, 5, nullptr);
}

// SIGNAL 6
void UdpListenerThread::stoped()
{
    QMetaObject::activate(this, &staticMetaObject, 6, nullptr);
}

// SIGNAL 7
void UdpListenerThread::receivedUdpData(QByteArray _t1, QString _t2, int _t3)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)) };
    QMetaObject::activate(this, &staticMetaObject, 7, _a);
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
