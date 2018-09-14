/****************************************************************************
** Meta object code from reading C++ file 'qcomminfo.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.10.1)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../firewall-test/qcomminfo.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'qcomminfo.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.10.1. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_QCommInfo_t {
    QByteArrayData data[8];
    char stringdata0[76];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_QCommInfo_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_QCommInfo_t qt_meta_stringdata_QCommInfo = {
    {
QT_MOC_LITERAL(0, 0, 9), // "QCommInfo"
QT_MOC_LITERAL(1, 10, 19), // "whenListenerStarted"
QT_MOC_LITERAL(2, 30, 0), // ""
QT_MOC_LITERAL(3, 31, 6), // "result"
QT_MOC_LITERAL(4, 38, 13), // "receivedBytes"
QT_MOC_LITERAL(5, 52, 9), // "dataBytes"
QT_MOC_LITERAL(6, 62, 5), // "srcIp"
QT_MOC_LITERAL(7, 68, 7) // "srcPort"

    },
    "QCommInfo\0whenListenerStarted\0\0result\0"
    "receivedBytes\0dataBytes\0srcIp\0srcPort"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_QCommInfo[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags
       1,    1,   24,    2, 0x08 /* Private */,
       4,    3,   27,    2, 0x08 /* Private */,

 // slots: parameters
    QMetaType::Void, QMetaType::Bool,    3,
    QMetaType::Void, QMetaType::QByteArray, QMetaType::QString, QMetaType::Int,    5,    6,    7,

       0        // eod
};

void QCommInfo::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        QCommInfo *_t = static_cast<QCommInfo *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->whenListenerStarted((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 1: _t->receivedBytes((*reinterpret_cast< QByteArray(*)>(_a[1])),(*reinterpret_cast< QString(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        default: ;
        }
    }
}

QT_INIT_METAOBJECT const QMetaObject QCommInfo::staticMetaObject = {
    { &UdpListenerThread::staticMetaObject, qt_meta_stringdata_QCommInfo.data,
      qt_meta_data_QCommInfo,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *QCommInfo::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *QCommInfo::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_QCommInfo.stringdata0))
        return static_cast<void*>(this);
    return UdpListenerThread::qt_metacast(_clname);
}

int QCommInfo::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = UdpListenerThread::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 2)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 2;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 2)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 2;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
