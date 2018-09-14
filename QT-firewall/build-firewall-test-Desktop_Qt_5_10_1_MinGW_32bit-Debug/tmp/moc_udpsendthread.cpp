/****************************************************************************
** Meta object code from reading C++ file 'udpsendthread.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.10.1)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../firewall-test/udpsendthread.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'udpsendthread.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.10.1. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_UdpSendThread_t {
    QByteArrayData data[20];
    char stringdata0[163];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_UdpSendThread_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_UdpSendThread_t qt_meta_stringdata_UdpSendThread = {
    {
QT_MOC_LITERAL(0, 0, 13), // "UdpSendThread"
QT_MOC_LITERAL(1, 14, 10), // "startTimer"
QT_MOC_LITERAL(2, 25, 0), // ""
QT_MOC_LITERAL(3, 26, 4), // "msec"
QT_MOC_LITERAL(4, 31, 9), // "stopTimer"
QT_MOC_LITERAL(5, 41, 7), // "started"
QT_MOC_LITERAL(6, 49, 6), // "result"
QT_MOC_LITERAL(7, 56, 7), // "timeout"
QT_MOC_LITERAL(8, 64, 6), // "stoped"
QT_MOC_LITERAL(9, 71, 6), // "sended"
QT_MOC_LITERAL(10, 78, 11), // "whenTimeout"
QT_MOC_LITERAL(11, 90, 5), // "start"
QT_MOC_LITERAL(12, 96, 5), // "dstIp"
QT_MOC_LITERAL(13, 102, 7), // "dstPort"
QT_MOC_LITERAL(14, 110, 11), // "const char*"
QT_MOC_LITERAL(15, 122, 3), // "msg"
QT_MOC_LITERAL(16, 126, 9), // "msgLength"
QT_MOC_LITERAL(17, 136, 11), // "interval_ms"
QT_MOC_LITERAL(18, 148, 9), // "sendTimes"
QT_MOC_LITERAL(19, 158, 4) // "stop"

    },
    "UdpSendThread\0startTimer\0\0msec\0stopTimer\0"
    "started\0result\0timeout\0stoped\0sended\0"
    "whenTimeout\0start\0dstIp\0dstPort\0"
    "const char*\0msg\0msgLength\0interval_ms\0"
    "sendTimes\0stop"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_UdpSendThread[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
      12,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       6,       // signalCount

 // signals: name, argc, parameters, tag, flags
       1,    1,   74,    2, 0x06 /* Public */,
       4,    0,   77,    2, 0x06 /* Public */,
       5,    1,   78,    2, 0x06 /* Public */,
       7,    0,   81,    2, 0x06 /* Public */,
       8,    0,   82,    2, 0x06 /* Public */,
       9,    0,   83,    2, 0x06 /* Public */,

 // slots: name, argc, parameters, tag, flags
      10,    0,   84,    2, 0x08 /* Private */,
      11,    0,   85,    2, 0x0a /* Public */,
      11,    4,   86,    2, 0x0a /* Public */,
      11,    2,   95,    2, 0x0a /* Public */,
      11,    1,  100,    2, 0x0a /* Public */,
      19,    0,  103,    2, 0x0a /* Public */,

 // signals: parameters
    QMetaType::Void, QMetaType::Int,    3,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Bool,    6,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::QString, QMetaType::Int, 0x80000000 | 14, QMetaType::Int,   12,   13,   15,   16,
    QMetaType::Void, QMetaType::Int, QMetaType::Int,   17,   18,
    QMetaType::Void, QMetaType::Int,   17,
    QMetaType::Void,

       0        // eod
};

void UdpSendThread::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        UdpSendThread *_t = static_cast<UdpSendThread *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->startTimer((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 1: _t->stopTimer(); break;
        case 2: _t->started((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 3: _t->timeout(); break;
        case 4: _t->stoped(); break;
        case 5: _t->sended(); break;
        case 6: _t->whenTimeout(); break;
        case 7: _t->start(); break;
        case 8: _t->start((*reinterpret_cast< const QString(*)>(_a[1])),(*reinterpret_cast< const int(*)>(_a[2])),(*reinterpret_cast< const char*(*)>(_a[3])),(*reinterpret_cast< const int(*)>(_a[4]))); break;
        case 9: _t->start((*reinterpret_cast< const int(*)>(_a[1])),(*reinterpret_cast< const int(*)>(_a[2]))); break;
        case 10: _t->start((*reinterpret_cast< const int(*)>(_a[1]))); break;
        case 11: _t->stop(); break;
        default: ;
        }
    } else if (_c == QMetaObject::IndexOfMethod) {
        int *result = reinterpret_cast<int *>(_a[0]);
        {
            typedef void (UdpSendThread::*_t)(int );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::startTimer)) {
                *result = 0;
                return;
            }
        }
        {
            typedef void (UdpSendThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::stopTimer)) {
                *result = 1;
                return;
            }
        }
        {
            typedef void (UdpSendThread::*_t)(bool );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::started)) {
                *result = 2;
                return;
            }
        }
        {
            typedef void (UdpSendThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::timeout)) {
                *result = 3;
                return;
            }
        }
        {
            typedef void (UdpSendThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::stoped)) {
                *result = 4;
                return;
            }
        }
        {
            typedef void (UdpSendThread::*_t)();
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&UdpSendThread::sended)) {
                *result = 5;
                return;
            }
        }
    }
}

QT_INIT_METAOBJECT const QMetaObject UdpSendThread::staticMetaObject = {
    { &QThread::staticMetaObject, qt_meta_stringdata_UdpSendThread.data,
      qt_meta_data_UdpSendThread,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *UdpSendThread::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *UdpSendThread::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_UdpSendThread.stringdata0))
        return static_cast<void*>(this);
    return QThread::qt_metacast(_clname);
}

int UdpSendThread::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QThread::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 12)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 12;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 12)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 12;
    }
    return _id;
}

// SIGNAL 0
void UdpSendThread::startTimer(int _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void UdpSendThread::stopTimer()
{
    QMetaObject::activate(this, &staticMetaObject, 1, nullptr);
}

// SIGNAL 2
void UdpSendThread::started(bool _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void UdpSendThread::timeout()
{
    QMetaObject::activate(this, &staticMetaObject, 3, nullptr);
}

// SIGNAL 4
void UdpSendThread::stoped()
{
    QMetaObject::activate(this, &staticMetaObject, 4, nullptr);
}

// SIGNAL 5
void UdpSendThread::sended()
{
    QMetaObject::activate(this, &staticMetaObject, 5, nullptr);
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
