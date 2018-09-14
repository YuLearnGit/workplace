/*
 * EtherCAT_decode.c
 *
 *  Created on: 2015Äê6ÔÂ15ÈÕ
 *      Author: zwj, dl
 */

#ifndef EtherCAT_DECODE_H
#define EtherCAT_DECODE_H

#include <linux/types.h>


#include <linux/skbuff.h>
#include <linux/ip.h>
#include <linux/tcp.h>
#include <linux/ipv6.h>
#include <linux/netfilter/x_tables.h>
#include <linux/types.h>
#include <linux/netfilter.h>
#include <linux/version.h>
#include <linux/netfilter_ipv4.h>
#include <linux/moduleparam.h>
#include <linux/netfilter_ipv4/ip_tables.h>
#include <linux/init.h>
#include <linux/inet.h>
#define EtherCAT_OK 1
#define EtherCAT_FAIL (-1)

/* Need 8 bytes for MBAP Header + Function Code */
#define EtherCAT_MIN_LEN 5

/* GIDs, SIDs, and Strings */
#define GENERATOR_SPP_EtherCAT 144

#define EtherCAT_BAD_LENGTH 1
#define EtherCAT_BAD_PROTO_ID 2
#define EtherCAT_RESERVED_FUNCTION 3

#define EtherCAT_BAD_LENGTH_STR "(spp_EtherCAT): Length in EtherCAT MBAP header does not match the length needed for the given EtherCAT function."
#define EtherCAT_BAD_PROTO_ID_STR "(spp_EtherCAT): EtherCAT protocol ID is non-zero."
#define EtherCAT_RESERVED_FUNCTION_STR "(spp_EtherCAT): Reserved EtherCAT function code in use."

#define EtherCAT_DATA_LEN_OK									1
#define EtherCAT_DATA_LEN_FAIL								0

#define EtherCAT_PORT_OK									1
#define EtherCAT_PORT_FAIL								0

#define EtherCAT_IP_OK									1
#define EtherCAT_IP_FAIL								0

#define FROM_CLIENT_FLAG		0
#define FROM_SERVER_FLAG		1

#define EtherCAT_FUNCODE_REJECT      0
#define EtherCAT_FUNCODE_RECEIVE	 1

#define THIS_IS_REJECT_FUN_CODE 		 1
#define THIS_IS_NOT_REJECT_FUN_CODE 	 0
struct xt_EtherCAT_mtinfo {
	__u32 src_port;
	__u32 dst_port;	
    __u8 flags;
};

int EtherCATDecode(char *app_data, __u16 app_len, __u8 flags, const struct xt_EtherCAT_mtinfo *info, const struct sk_buff *skb);
#endif /* EtherCAT_DECODE_H */
