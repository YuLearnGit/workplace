#include <sys/types.h>
#include <libnet.h>
#include <string>
#include <iostream>

#include "sendinfo.h"

using namespace std;

int sendinfo(string dst_ip, string src_ip, string dst_mac, string src_mac,
             u_int16_t src_port, u_int16_t dst_port, string content) {
    libnet_t *handle; /* Libnet句柄 */
	//char *device = "br0"; /* 设备名字,也支持点十进制的IP地址,会自己找到匹配的设备 */
	std::string dev_str("br0");
	char *device = const_cast<char*>(dev_str.c_str());
    char *src_ip_str = const_cast<char*>(src_ip.c_str());
	char *dst_ip_str = const_cast<char*>(dst_ip.c_str());
    char *src_mac_str = const_cast<char*>(src_mac.c_str());
    char *dst_mac_str = const_cast<char*>(dst_mac.c_str());
    char *cont_str = const_cast<char*>(content.c_str());

    u_char srcmac[6], dstmac[6];
    mac_str_to_bin((u_char*)src_mac_str, srcmac);
    mac_str_to_bin((u_char*)dst_mac_str, dstmac);

    u_long dstip, srcip; /* 网路序的目的IP和源IP */
	char error[LIBNET_ERRBUF_SIZE]; /* 出错信息 */
	libnet_ptag_t eth_tag, ip_tag, udp_tag; /* 各层build函数返回值 */
	u_short proto = IPPROTO_UDP; /* 传输层协议 */
	u_char payload[400] = {0}; /* 承载数据的数组，初值为空 */
	u_long payload_s = 0; /* 承载数据的长度，初值为0 */

  /* 把目的IP地址字符串转化成网络序 */
	dstip = libnet_name2addr4(handle, dst_ip_str, LIBNET_RESOLVE);
	/* 把源IP地址字符串转化成网络序 */
	srcip = libnet_name2addr4(handle, src_ip_str, LIBNET_RESOLVE);

  /* 初始化Libnet */
  if((handle = libnet_init(LIBNET_LINK, device, error)) == NULL)
    return (-1);

  strncpy((char*)payload, cont_str, strlen(cont_str)+1); /* 构造负载的内容 */
	payload_s = strlen((char*)payload); /* 计算负载内容的长度 */

  udp_tag = libnet_build_udp(
			src_port, /* 源端口 */
			dst_port, /* 目的端口 */
			LIBNET_UDP_H + payload_s, /* 长度 */
			0, /* 校验和,0为libnet自动计算 */
			payload, /* 负载内容 */
			payload_s, /* 负载内容长度 */
			handle, /* libnet句柄 */
			0 /* 新建包 */
			);
  if(udp_tag == -1)
    return(-3);

  /* 构造IP协议块，返回值是新生成的IP协议快的一个标记 */
	ip_tag = libnet_build_ipv4(
			LIBNET_IPV4_H + LIBNET_UDP_H + payload_s, /* IP协议块的总长*/
			0, /* tos */
			(u_short) libnet_get_prand(LIBNET_PRu16), /* id,随机产生0~65535 */
			0, /* frag 片偏移 */
			(u_int8_t)libnet_get_prand(LIBNET_PR8), /* ttl,随机产生0~255 */
			proto, /* 上层协议 */
			0, /* 校验和，此时为0，表示由Libnet自动计算 */
			srcip, /* 源IP地址,网络序 */
			dstip, /* 目标IP地址,网络序 */
			NULL, /* 负载内容或为NULL */
			0, /* 负载内容的大小*/
			handle, /* Libnet句柄 */
			0 /* 协议块标记可修改或创建,0表示构造一个新的*/
			);
  if(ip_tag == -1)
    return (-4);

  /* 构造一个以太网协议块,只能用于LIBNET_LINK */
	eth_tag = libnet_build_ethernet(
			dstmac, /* 以太网目的地址 */
			srcmac, /* 以太网源地址 */
			ETHERTYPE_IP, /* 以太网上层协议类型，此时为IP类型 */
			NULL, /* 负载，这里为空 */
			0, /* 负载大小 */
			handle, /* Libnet句柄 */
			0 /* 协议块标记，0表示构造一个新的 */
			);
  if(eth_tag == -1)
    return (-5);

  int packet_size = libnet_write(handle); /* 发送已经构造的数据包*/
	/*
#ifdef DEBUG
     cout << "Send successfully!!!" << endl;
#endif
*/
  libnet_destroy(handle); /* 释放句柄 */
  return (0);
}

int mac_str_to_bin(u_char *str, u_char *mac) {
    int i;
    u_char *s, *e;

    if((mac==NULL) || (str==NULL))
        return -1;
    s = (u_char*)str;
    for(i = 0; i < 6; ++i) {
        mac[i] = s ? strtoul((const char*)s, (char**)&e, 16) : 0;
        if(s)
            s = (*e) ? e + 1 :e;
    }
    return 0;
}
