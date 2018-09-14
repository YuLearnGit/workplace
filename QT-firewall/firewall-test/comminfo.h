#ifndef COMMINFO_H
#define COMMINFO_H

#include <iostream>
#include <cstring>
#include <string>
#include <ctime>
#include <cstdlib>
#include <stdio.h>
#include <memory>
#include <cassert>

#ifndef DEBUG_POINTER
#define DEBUG_POINTER
#endif // DEBUG_POINTER

#ifndef DEBUG_PRINT
#define DEBUG_PRINT
#endif // DEBUG_PRINT

#ifndef SAFE_DELETE
#define SAFE_DELETE
// 删除普通指针，并置为 nullptr
#define DELETE(pointer) if ( pointer != nullptr){delete pointer;pointer = nullptr;}
// 删除指针数组，并置为 nullptr
#define DELETE_ARRAY(pointer) if ( pointer != nullptr){delete []pointer;pointer = nullptr;}
#endif

class MyRandom
{
private:
    static unsigned long random_max;
    static unsigned long next_seed;
private:
    static long doRand(unsigned long *value);
public:
    static int rand(void); // 生成随机数
    static void srand(unsigned int seed); // 设置随机数种子
};

/**
 * @brief The CommInfo class
 *      用于解析和构造自定义UDP数据协议
 */
class CommInfo
{
public:
    CommInfo();
    ~CommInfo();
private:
    static const char *headBytes; // 自定义协议头：0x0F0E0D0C0B0A
    static const char *versionByte; // 自定协议版本:0x02
    static const char *endByte; // 自定义结束字节：0x18

    static const int headLength; // 长度为6，指第1-6字节
    static const int versionLength; // 长度为1，指第7字节
    static const int checkLength; // 长度为4，指第8-11字节（时间字符串或随机数字符串位）
    static const int validNumLength; // 长度为2，指12-13字节
    static const int commandTypeLength; // 长度为1，指第14字节

    static const int endLength; // 长度为1，指结束字节

public:
    // 第14字节：命令类型，可直接填写在 QCommSend 的形参 cmdTypeByte 中。
    static const char *iptablesCmdTypeByte; // iptables命令操作
    static const char *syslogServerCmdTypeByte; // syslog服务器配置操作
    static const char *scanDeviceCmdTypeByte; // 扫描设备操作
    static const char *commonShellCmdTypeByte; // 通用shell操作。即使用shell直接协议中的命令字符串。
    static const char *returnShellCmdTypeByte; // shell操作，但是返回值是shell命令在终端的执行结果（结果可能为空）

public:

    static char *parseUintToBytes(unsigned int num); // unsigned int 转为 4位 char（返回值需要手动delete，例如使用自定义宏DELETE_ARRAY删除）
    static unsigned int parseBytesToUint(const char *tb); // 4位 char 转为  unsigned int
    static char *parseUsintToBytes(unsigned short int num); // unsigned short int 转为 2位 char （返回值需要手动delete，例如使用自定义宏DELETE_ARRAY删除）
    static unsigned short int parseBytesToUsint(const char *tb); // 2位 char 转为  unsigned int

    static char *getTimeBytes(); // 获取当前系统时间的时间格式串，长度4位 （返回值需要手动delete，例如使用自定义宏DELETE_ARRAY删除）
    static unsigned int getRandomNum(unsigned int seed); // 获取某随机种子的第一个返回值
    static char *timeBytesToRandomBytes(const char *tb); // 将时间字符串转为无符号整形，并作为随机数种子，获取随机数值，再转为字符串，长度4位char（返回值需要手动delete，例如使用自定义宏DELETE_ARRAY删除）

    /**
     * @brief copyBytes 字符串数据，内存拷贝函数，取代系统strncpy()函数。
     *                  strncpy()拷贝遇到0x00便停止，不会继续拷贝后面的值，因此重新实现
     * @param dst   拷贝到dst所指的位置后面
     * @param src   将src地址后面len个字符，拷贝到dst后面
     * @param len   拷贝数据的长度。
     */
    static void copyBytes(char *dst,const char *src,int len);

    /**
     * @brief composeUdpData 构造自定义协议数据
     * @param checkBytes 第8-11字节，可传递时间字符串或随机数字符串
     * @param commandTypeByte 第14字节
     * @param commandBytes 其余字节（第15字节到结束字节之前（不包含结束字节））
     * @param dataLength 回调值，指示返回结果的字符串长度
     * @return 自定义协议的第1字节到结束字节的所有字节（返回值需要手动delete，例如使用自定义宏DELETE_ARRAY删除）
     *          注意：勿使用strlen计算字符串长度
     */
    static char *composeUdpData(const char *checkBytes, const char *commandTypeByte, const char *commandBytes, int *dataLength=nullptr);

    /**
     * @brief discomposeUdpData 拆解自定义协议数据
     * @param udpData   接收到UDP数据内容，即第1字节到结束字节的所有字节
     * @param udpDataLength udpData的长度
     * @param checkBytes    回调值，指向第8-11字节（指向udpData的对应地址，所以，此实参不需要申请内存）
     * @param commandTypeByte 回调值，指向第14字节（指向udpData的对应地址，所以，此实参不需要申请内存）
     * @param commandBytes  回调值，指向第15字节到结束字节之前（不含结束字节）（指向udpData的对应地址，所以，此实参不需要申请内存）
     * @param commandLength 回调值，commandBytes的长度
     * @return 解析成功或失败，成功，上述回调值才有效
     */
    static bool discomposeUdpData(const char *udpData,const int udpDataLength,char **checkBytes,char **commandTypeByte,char **commandBytes,int *commandLength);

    /**
     * @brief checkHeadBytes 检查是否是自定义头部
     * @param head 比较从head开始的6字节是否是自定义头部。
     * @return 是或不是
     */
    static bool checkHeadBytes(const char * head);

};

/**
 * @brief The SyslogServerInfo class
 * 用于配置Syslog服务器的参数
 * 包括：通信协议，服务器IP地址和端口
 */
class SyslogServerInfo
{
public:
    // 默认的两种通信协议
    static const std::string UDP;
    static const std::string TCP;
private:
    std::string protocol;
    std::string serverIp;
    std::string serverPort;
public:
    /**
     * @brief ServerConfig
     * @param protocol 通信协议，只能取值ServerConfig::UDP或ServerConfig::TCP
     *                 注意，要判断是否是某协议时，请用 == ServerConfig::TCP
     * @param ip 服务器IP地址，例如：“192.16.10.254”
     * @param port 端口，例如：“514”
     */
    SyslogServerInfo(const std::string &protocol,const std::string &serverIp,const std::string &serverPort);
    SyslogServerInfo();

public:
    void display() const;
    std::string getProtocol() const;
    void setProtocol(const std::string &value);
    std::string getIp() const;
    void setIp(const std::string &value);
    std::string getPort() const;
    void setPort(const std::string &value);

public:
    static const char *writeServerOperationTypeByte; // 第15字节的操作类型。写入操作
    static const char *readServerOperationTypeByte; // 第15字节的操作类型。读取操作
public:

    // 组合成写入syslog服务器的命令：第15字节开始的命令字符串
    static std::string composeWriteServerCmd(const SyslogServerInfo &info);

    // 组合成读取syslog服务器的命令：第15字节开始的命令字符串
    static std::string composeReadServerCmd()
    {
        std::string cmdStr = "sed -n '/SERVER_CONFIG/,/END_CONFIG/{2,2p}' /etc/rsyslog.conf";
        return SyslogServerInfo::readServerOperationTypeByte + cmdStr;
    }

    friend std::ostream & operator << (std::ostream &out, SyslogServerInfo &info);
//    friend std::istream & operator >> (std::istream &in, SyslogServerInfo &obj);

    // 解析读取命令得到的返回结果。示例：*.* @192.16.10.254:514，解析失败返回nullptr,注意释放指针。
    static SyslogServerInfo *parseReadCmdResult(const std::string &data);

};


#endif // COMMINFO_H
