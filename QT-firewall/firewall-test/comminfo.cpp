#include "comminfo.h"

unsigned long MyRandom::random_max = 0x7FFFFFFF;
unsigned long MyRandom::next_seed = 1;

long MyRandom::doRand(unsigned long *value)
{
    long quotient, remainder, t;

    quotient = *value / 127773L;
    remainder = *value % 127773L;
    t = 16807L * remainder - 2836L * quotient;
    if (t <= 0)
        t += 0x7FFFFFFFL;
    return ((*value = t) % ((unsigned long)random_max + 1));
}

int MyRandom::rand()
{
    return doRand(&MyRandom::next_seed);
}

void MyRandom::srand(unsigned int seed)
{
    next_seed = seed;
}

const char *CommInfo::headBytes = "\x0f\x0e\x0d\x0c\x0b\x0a";
const char *CommInfo::versionByte = "\x02";
const char *CommInfo::endByte = "\x18";
const int CommInfo::headLength = strlen(CommInfo::headBytes);
const int CommInfo::versionLength = strlen(CommInfo::versionByte);
const int CommInfo::checkLength = 4;
const int CommInfo::validNumLength = 2;
const int CommInfo::endLength = strlen(CommInfo::endByte);

const char *CommInfo::iptablesCmdTypeByte = "\x01";
const char *CommInfo::syslogServerCmdTypeByte = "\x02";
const char *CommInfo::scanDeviceCmdTypeByte = "\x03";
const char *CommInfo::commonShellCmdTypeByte = "\x04";
const char *CommInfo::returnShellCmdTypeByte = "\x05";
const int CommInfo::commandTypeLength = 1;

char *CommInfo::getTimeBytes()
{
    char * tbs = new char[5];
    time_t t = time(nullptr);
//    std::cout << t << std::endl;
    tbs[0] = ((t >> 24) & 0xFF);
    tbs[1] = ((t >> 16) & 0xFF);
    tbs[2] = ((t >> 8 ) & 0xFF);
    tbs[3] = ( t        & 0xFF);
    tbs[4] = '\0';
    return tbs;
}

CommInfo::CommInfo()
{

}

CommInfo::~CommInfo()
{

}

char *CommInfo::parseUintToBytes(unsigned int num)
{
    char *bs = new char[5];
    bs[0] = ((num >> 24) & 0xFF);
    bs[1] = ((num >> 16) & 0xFF);
    bs[2] = ((num >> 8 ) & 0xFF);
    bs[3] = ( num        & 0xFF);
    bs[4] = '\0';
    return bs;
}

unsigned int CommInfo::parseBytesToUint(const char *tbs)
{
    unsigned int ts = ((tbs[0] & 0xFF) << 24) | ((tbs[1] & 0xFF) << 16) | ((tbs[2] & 0xFF) << 8) | (tbs[3] & 0xFF);
//    std::cout << ts << std::endl;
    return ts;
}

char *CommInfo::parseUsintToBytes(unsigned short num)
{
    char *bs = new char[3];
    bs[0] = ((num >> 8) & 0xFF);
    bs[1] = ((num     ) & 0xFF);
    bs[2] = '\0';
    return bs;
}

unsigned short CommInfo::parseBytesToUsint(const char *tbs)
{
    unsigned int ts = ((tbs[0] & 0xFF) << 8) | (tbs[1] & 0xFF);
    return ts;
}

unsigned int CommInfo::getRandomNum(unsigned int seed)
{
    MyRandom::srand(seed);
    return MyRandom::rand();
}

char *CommInfo::timeBytesToRandomBytes(const char *tb)
{
    unsigned int time = CommInfo::parseBytesToUint(tb);
    unsigned int random = CommInfo::getRandomNum(time);
    return CommInfo::parseUintToBytes(random);
}

void CommInfo::copyBytes(char *dst, const char *src, int len){
    assert (len >= 1);
    memcpy(dst,src,len);
}

char *CommInfo::composeUdpData(const char *checkBytes, const char *commandTypeByte, const char *commandBytes, int *dataLength)
{
    // commandBytes 不含0x00，所以可以这样计算长度
    int commandLength = strlen(commandBytes);

    int validDataLength = commandTypeLength + commandLength;

    char *validNumBytes = CommInfo::parseUsintToBytes(validDataLength);

    int plainTextLength = checkLength + validNumLength + commandTypeLength + commandLength;

    char *plainTextBytes = new char[plainTextLength + 1];
    copyBytes(plainTextBytes,checkBytes,checkLength);
    copyBytes(plainTextBytes + checkLength,validNumBytes,validNumLength);
    copyBytes(plainTextBytes + checkLength + validNumLength,commandTypeByte,commandTypeLength);
    copyBytes(plainTextBytes + checkLength + validNumLength + commandTypeLength,commandBytes,commandLength);
    plainTextBytes[plainTextLength] = '\0';

    DELETE(validNumBytes);

    // 此处进行加密
    char * encryptedBytes = plainTextBytes;

    //        int encryptedLength = strlen(encryptedBytes);
    int encryptedLength = checkLength + validNumLength + commandTypeLength + commandLength;

    int udpDataLength =  headLength + versionLength + encryptedLength + endLength;
    char *udpData = new char[udpDataLength + 1];
    copyBytes(udpData,headBytes,headLength);
    copyBytes(udpData + headLength,versionByte,versionLength);
    copyBytes(udpData + headLength + versionLength,encryptedBytes,encryptedLength);
    copyBytes(udpData + headLength + versionLength + encryptedLength,endByte,endLength);
    udpData[udpDataLength] = '\0';

//    std::cout << "udpData:" << std::endl;
//    for (int i=0;i<udpDataLength;++i)
//    {
//        printf("%x ",(unsigned char)udpData[i]);
//    }
//    std::cout << std::endl;

    DELETE(plainTextBytes);
    //加密数据也可能需要delete
    if (dataLength != nullptr)
    {
        *dataLength = udpDataLength;
    }

    return udpData;
}

bool CommInfo::discomposeUdpData(const char *udpData, const int udpDataLength, char **checkBytes, char **commandTypeByte, char **commandBytes, int *commandLength)
{
    // 先检查头部
    const char *head = udpData;
    if (!CommInfo::checkHeadBytes(head))
    {
        return false;
    }
    // 检查结尾字符
    if (udpData[udpDataLength - 1] != *endByte)
    {
        printf("end byte error:%x\n",(unsigned char)udpData[udpDataLength-1]);
        return false;
    }
    // 检查协议版本
    const char *version = udpData + headLength;
    if (*version != *versionByte)
    {
        printf("version byte error:%x\n",(unsigned char)*version);
        return false;
    }
    // 上述检查都通过了，认为是本协议数据，开始解析。
    const char *encryptedData = udpData + headLength + versionLength;
//    int encryptedLength = udpDataLength - headLength - versionLength - endLength;
    // 此处应解密
    char *plainTextBytes = (char *)encryptedData;

    // 由于明文中易含\x00，所以，如何正确统计出明文长度？
    //        int plainTextLength = strlen(plainTextBytes);
    *checkBytes = plainTextBytes;
    const char *validNumBytes = (plainTextBytes + checkLength);
    *commandTypeByte = (plainTextBytes + checkLength + validNumLength);
    *commandBytes = (plainTextBytes + checkLength + validNumLength + commandTypeLength);

    if (commandLength != nullptr)
    {
        *commandLength = CommInfo::parseBytesToUsint(validNumBytes) - commandTypeLength;
    }

    return true;
}

bool CommInfo::checkHeadBytes(const char *head)
{
    for (int i=0;i < headLength; ++i)
    {
        if (head[i] != headBytes[i])
        {
            printf("head bytes error:%x\n",(unsigned char)head[i]);
            return false;
        }
    }
    return true;
}

const char *SyslogServerInfo::writeServerOperationTypeByte = "1";
const char *SyslogServerInfo::readServerOperationTypeByte = "2";

SyslogServerInfo::SyslogServerInfo(const std::string &protocol,const std::string &ip,const std::string &port)
{
    this->protocol = protocol;
    this->serverIp = ip;
    this->serverPort = port;
}

SyslogServerInfo::SyslogServerInfo()
{

}

const std::string SyslogServerInfo::UDP("UDP");
const std::string SyslogServerInfo::TCP("TCP");

void SyslogServerInfo::display() const
{
    if (this->protocol == SyslogServerInfo::TCP)
    {
        std::cout << "protocol:TCP" << std::endl;
    }
    else if (this->protocol == SyslogServerInfo::UDP)
    {
        std::cout << "protocol:UDP" << std::endl;
    }
    else
    {
        std::cout << "Error protocol!" << std::endl;
    }
    std::cout << "ip:" << this->serverIp << std::endl;
    std::cout << "port:" << this->serverPort << std::endl;
}

std::ostream & operator <<(std::ostream &out, SyslogServerInfo &info)
{
    if (info.protocol == SyslogServerInfo::TCP)
    {
        out << "protocol:TCP";
    }
    else if (info.protocol == SyslogServerInfo::UDP)
    {
        out << "protocol:UDP";
    }
    else
    {
        out << "Error protocol!";
    }
    out << " ip:" << info.serverIp << " port:" << info.serverPort;
    return out;
}

std::string SyslogServerInfo::getProtocol() const
{
    return protocol;
}

void SyslogServerInfo::setProtocol(const std::string &value)
{
    protocol = value;
}

std::string SyslogServerInfo::getIp() const
{
    return serverIp;
}

void SyslogServerInfo::setIp(const std::string &value)
{
    serverIp = value;
}

std::string SyslogServerInfo::getPort() const
{
    return serverPort;
}

void SyslogServerInfo::setPort(const std::string &value)
{
    serverPort = value;
}

std::string SyslogServerInfo::composeWriteServerCmd(const SyslogServerInfo &info)
{
    std::string protocolStr;
    time_t t = time(nullptr);
    char tmp[64];
    strftime(tmp, sizeof(tmp), "%Y-%m-%d %H:%M:%S",localtime(&t));
    std::string timeStr(tmp);
    if (info.getProtocol() == SyslogServerInfo::TCP)
    {
        protocolStr = "@@";
    }
    else if (info.getProtocol() == SyslogServerInfo::UDP)
    {
        protocolStr = "@";
    }
    else
    {
        return "error";
    }
    std::string cmdStr = "sed -i -e '/SERVER_CONFIG/,/END_CONFIG/d' -e '1,1i\\# SERVER_CONFIG " + timeStr + "\\n*.* "+ protocolStr + info.getIp() +":" + info.getPort() + "\\n# END_CONFIG' /etc/rsyslog.conf & service rsyslog restart";
    return SyslogServerInfo::writeServerOperationTypeByte + cmdStr;
}

SyslogServerInfo *SyslogServerInfo::parseReadCmdResult(const std::string &data)
{
    SyslogServerInfo *info = new SyslogServerInfo();
    int length = data.size();
    const char * str = data.c_str();
    int i=0;
    int ip=0;
    while (i < length)
    {
        if (*(str + i) == '@')
        {
            ip=i+1;
            ++i;
            if (*(str + i) == '@')
            {
                info->setProtocol(SyslogServerInfo::TCP);
                ip=i+1;
            }
            else
            {
                info->setProtocol(SyslogServerInfo::UDP);
            }
        }
        else if (*(str +i) == ':')
        {
            info->setIp(std::string(str+ip,i-ip));
            info->setPort(std::string(str+i+1,length-i));
            break;
        }
        ++i;

    }
    if (i >= length)
    {
        return nullptr;
    }
    return info;
}
