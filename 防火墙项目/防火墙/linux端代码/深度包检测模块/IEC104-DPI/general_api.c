/*
 * general_api.c
 *
 *  Created on: 2015年6月15日
 *      Author: zwj
 */

#include "general_api.h"

//****************************************************************************
//函数名：IsTcpPacket(const struct sk_buff *skb)
//功能：判断是否为TCP数据包
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpPacket(const struct sk_buff *skb)
{
	struct iphdr *iph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_TCP)
		return true;
	else return false;
}

//****************************************************************************
//函数名：IsUdpPacket(const struct sk_buff *skb)
//功能：判断是否为UDP数据包
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsUdpPacket(const struct sk_buff *skb)
{
	struct iphdr *iph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_UDP)
		return true;
	else return false;
}

//****************************************************************************
//函数名：IsIcmpPacket(const struct sk_buff *skb)
//功能：判断是否为ICMP数据包
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsIcmpPacket(const struct sk_buff *skb)
{
	struct iphdr *iph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_ICMP)
		return true;
	else return false;
}

//****************************************************************************
//函数名：GetSrcIp(const struct sk_buff *skb)
//功能：获取源IP
//参数：struct sk_buff *skb
//返回值：__u32
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
__u32 GetSrcIp(const struct sk_buff *skb)
{
	__be32 sip;
	struct iphdr *iph;
	iph  = ip_hdr(skb);
	sip = iph->saddr;
	return sip;
}

//****************************************************************************
//函数名：GetDstIp(const struct sk_buff *skb)
//功能：获取目的IP
//参数：struct sk_buff *skb
//返回值：__u32
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
__u32 GetDstIp(const struct sk_buff *skb)
{
	__be32 dip;
	struct iphdr *iph;
	iph  = ip_hdr(skb);
	dip = iph->daddr;
	return dip;
}

//****************************************************************************
//函数名：GetSrcPort(const struct sk_buff *skb)
//功能：获取源端口
//参数：struct sk_buff *skb
//返回值：__u32
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
__u32 GetSrcPort(const struct sk_buff *skb)
{
	__u32 sport;
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		sport = tcph->source;
	}
	else sport = 0;

	return sport;
}

//****************************************************************************
//函数名：GetDstPort(const struct sk_buff *skb)
//功能：获取目的端口
//参数：struct sk_buff *skb
//返回值：__u32
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
__u32 GetDstPort(const struct sk_buff *skb)
{
	__u32 dport;
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		dport = tcph->dest;
	}
	else dport = 0;

	return dport;
}

//****************************************************************************
//函数名：GetAppData(__u8* app_data, const struct sk_buff *skb)
//功能：获取目的端口
//参数：__u8* app_data, const struct sk_buff *skb
//返回值：__u8*
//作者：zwj
//日期：2015-6-5
//备注：__u8* app_data应用层数据指针；此函数使用： app_data = GetAppData(app_data, skb)
//****************************************************************************
__u8* GetAppData(__u8* app_data, const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph  = ip_hdr(skb);
	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		app_data = ((char *)tcph) + (tcph->doff << 2);
	}
	else app_data = NULL;

	return app_data;
}

//****************************************************************************
//函数名：GetAppDataLength(const struct sk_buff *skb)
//功能：获取应用层数据长度
//参数：struct sk_buff *skb
//返回值：__u16
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
__u16 GetAppDataLength(const struct sk_buff *skb)
{
	__u16 app_len;
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		app_len = (skb->len) - (skb->data_len) - (tcph->doff << 2);
		//printk("\n----(skb->len)=%d ,(skb->data_len)=%d ,(iph->ihl<<2)=%d, (tcph->doff << 2)=%d----\n",(skb->len),(skb->data_len),(iph->ihl<<2),(tcph->doff << 2));
	}
	else app_len = 0;

	return app_len;

}

//****************************************************************************
//函数名：GetData_U16(__u8* app_data, __u8 position)
//功能：获取指定起始位置两个字节的数据
//参数：__u8* app_data, __u8 position
//返回值：__u16(网络字节序转换后的16位无符号整形数据)
//作者：zwj
//日期：2015-6-5
//备注：app_data为应用层数据起始地址；position指出需要数据的位置；注意需要考虑是否超出应用层数据范围
//****************************************************************************
__u16 GetData_U16(__u8* app_data, __u8 position)
{
	__u16 data;
	data = (__u16) (*(app_data + position));
	data = ntohs(data);

	return data;
}

//****************************************************************************
//函数名：GetData_U32(__u8* app_data, __u8 position)
//功能：获取指定起始位置四个字节的数据
//参数：__u8* app_data, __u8 position
//返回值：__u32(网络字节序转换后的32位无符号整形数据)
//作者：zwj
//日期：2015-6-5
//备注：app_data为应用层数据起始地址；position指出需要数据的位置；注意需要考虑是否超出应用层数据范围
//****************************************************************************
__u32 GetData_U32(__u8* app_data, __u8 position)
{
	__u32 data;
	data = (__u32) (*(app_data + position));
	data = ntohs(data);

	return data;
}

//****************************************************************************
//函数名：IsTcpFlagFinSet(const struct sk_buff *skb)
//功能：获取TCP数据包Fin状态
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpFlagFinSet(const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		if(tcph->fin == FLAG_SET)
				return true;
		else
			return false;
	}
	else
		return false;
}

//****************************************************************************
//函数名：IsTcpFlagSynSet(const struct sk_buff *skb)
//功能：获取TCP数据包Syn状态
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpFlagSynSet(const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		if(tcph->syn == FLAG_SET)
				return true;
		else
			return false;
	}
	else
		return false;
}

//****************************************************************************
//函数名：IsTcpFlagRstSet(const struct sk_buff *skb)
//功能：获取TCP数据包Rst状态
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpFlagRstSet(const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		if(tcph->rst == FLAG_SET)
				return true;
		else
			return false;
	}
	else
		return false;
}

//****************************************************************************
//函数名：IsTcpFlagPshSet(const struct sk_buff *skb)
//功能：获取TCP数据包Psh状态
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpFlagPshSet(const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		if(tcph->psh == FLAG_SET)
				return true;
		else
			return false;
	}
	else
		return false;
}

//****************************************************************************
//函数名：IsTcpFlagAckSet(const struct sk_buff *skb)
//功能：获取TCP数据包Ack状态
//参数：struct sk_buff *skb
//返回值：bool
//作者：zwj
//日期：2015-6-5
//备注：
//****************************************************************************
bool IsTcpFlagAckSet(const struct sk_buff *skb)
{
	struct iphdr *iph;
	struct tcphdr *tcph;
	iph = ip_hdr(skb);

	if(iph->protocol == IPPROTO_TCP)
	{
		tcph = tcp_hdr(skb);
		if(tcph->ack == FLAG_SET)
				return true;
		else
			return false;
	}
	else
		return false;
}
