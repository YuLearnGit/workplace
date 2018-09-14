/*
 * general_api.h
 *
 *  Created on: 2015年6月15日
 *      Author: zwj
 */

#ifndef GENERAL_API_H_
#define GENERAL_API_H_

#include <linux/module.h>
#include <linux/skbuff.h>
#include <linux/ip.h>
#include <linux/tcp.h>
#include <linux/ipv6.h>
#include <linux/netfilter/x_tables.h>
#include <linux/types.h>
#include <linux/netfilter.h>
#include <linux/kernel.h>
#include <linux/version.h>
#include <linux/netfilter_ipv4.h>
#include <linux/moduleparam.h>
#include <linux/netfilter_ipv4/ip_tables.h>
#include <linux/init.h>
#include <linux/inet.h>
#include <linux/in.h>

#define FLAG_SET	1
#define FLAG_UNSET	0

//判断是否为TCP数据包
bool IsTcpPacket(const struct sk_buff *);

//判断是否为UDP数据包
bool IsUdpPackst(const struct sk_buff *);

//判断是否为ICMP数据包
bool IsIcmpPacket(const struct sk_buff *);

//获取源IP
__u32 GetSrcIp(const struct sk_buff *);

//获取目的IP
__u32 GetDstIp(const struct sk_buff *);

//获取源端口
__u32 GetSrcPort(const struct sk_buff *);

//获取目的端口
__u32 GetDstPort(const struct sk_buff *);

//获取应用层数据
__u8* GetAppData(__u8* , const struct sk_buff *);

//获取应用层数据长度
__u16 GetAppDataLength(const struct sk_buff *);

//获取指定位置两个字节的数据
__u16 GetData_U16(__u8*, __u8);

//获取指定位置四个字节的数据
__u32 GetData_U32(__u8*, __u8);

//获取TCP数据包Fin状态
bool IsTcpFlagFinSet(const struct sk_buff *);

//功能：获取TCP数据包Syn状态
bool IsTcpFlagSynSet(const struct sk_buff *);

//功能：获取TCP数据包Rst状态
bool IsTcpFlagRstSet(const struct sk_buff *);

//功能：获取TCP数据包Psh状态
bool IsTcpFlagPshSet(const struct sk_buff *);

//功能：获取TCP数据包Ack状态
bool IsTcpFlagAckSet(const struct sk_buff *);



#endif /* GENERAL_API_H_ */
