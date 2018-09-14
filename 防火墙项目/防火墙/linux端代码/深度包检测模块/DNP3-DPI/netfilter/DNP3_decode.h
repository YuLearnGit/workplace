/*
 * DNP3_decode.c
 *
 *  Created on: 2015Äê6ÔÂ15ÈÕ
 *      Author: zwj, dl
 */

#ifndef DNP3_DECODE_H
#define DNP3_DECODE_H

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
#define DNP3_OK 1
#define DNP3_FAIL (-1)

/* Need 8 bytes for MBAP Header + Function Code */
#define DNP3_MIN_LEN 5

/* GIDs, SIDs, and Strings */
#define GENERATOR_SPP_DNP3 144

#define DNP3_BAD_LENGTH 1
#define DNP3_BAD_PROTO_ID 2
#define DNP3_RESERVED_FUNCTION 3

#define DNP3_BAD_LENGTH_STR "(spp_DNP3): Length in DNP3 MBAP header does not match the length needed for the given DNP3 function."
#define DNP3_BAD_PROTO_ID_STR "(spp_DNP3): DNP3 protocol ID is non-zero."
#define DNP3_RESERVED_FUNCTION_STR "(spp_DNP3): Reserved DNP3 function code in use."

#define DNP3_DATA_LEN_OK									1
#define DNP3_DATA_LEN_FAIL								0

#define DNP3_PORT_OK									1
#define DNP3_PORT_FAIL								0

#define DNP3_IP_OK									1
#define DNP3_IP_FAIL								0

#define FROM_CLIENT_FLAG		0
#define FROM_SERVER_FLAG		1

#define DNP3_FUNCODE_REJECT      0
#define DNP3_FUNCODE_RECEIVE	 1

#define THIS_IS_REJECT_FUN_CODE 		 1
#define THIS_IS_NOT_REJECT_FUN_CODE 	 0
struct xt_DNP3_mtinfo {
	__u32 dst_port;	
	__u8 flags;
};

int DNP3Decode(char *app_data, __u16 app_len, __u8 flags, const struct xt_DNP3_mtinfo *info, const struct sk_buff *skb);
#endif /* DNP3_DECODE_H */
