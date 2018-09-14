/*
 * modbus_decode.c
 *
 *  Created on: 2015Äê6ÔÂ15ÈÕ
 *      Author: zwj, dl
 */

#ifndef MODBUS_DECODE_H
#define MODBUS_DECODE_H

#include <linux/types.h>

#define MODBUS_OK 1
#define MODBUS_FAIL (-1)

/* Need 8 bytes for MBAP Header + Function Code */
#define MODBUS_MIN_LEN 8

/* GIDs, SIDs, and Strings */
#define GENERATOR_SPP_MODBUS 144

#define MODBUS_BAD_LENGTH 1
#define MODBUS_BAD_PROTO_ID 2
#define MODBUS_RESERVED_FUNCTION 3

#define MODBUS_BAD_LENGTH_STR "(spp_modbus): Length in Modbus MBAP header does not match the length needed for the given Modbus function."
#define MODBUS_BAD_PROTO_ID_STR "(spp_modbus): Modbus protocol ID is non-zero."
#define MODBUS_RESERVED_FUNCTION_STR "(spp_modbus): Reserved Modbus function code in use."

#define MODBUS_LEN_OK									1
#define MODBUS_LEN_FAIL									0

#define MODBUS_COIL_ADDR_CHECK_OK						1
#define MODBUS_COIL_ADDR_CHECK_FAIL						0

#define FROM_CLIENT_FLAG		0
#define FROM_SERVER_FLAG		1

#define MODBUS_FUNCODE_REJECT       0
#define MODBUS_FUNCODE_RECEIVE	 1

#define THIS_IS_REJECT_FUN_CODE 				1
#define THIS_IS_NOT_REJECT_FUN_CODE 	 0
struct xt_modbusTcp_mtinfo {
	__u16 min_addr;
	__u16 max_addr;
	__u64 lfc_flag;
	__u64 hfc_flag;
	__u8 flags;
};

int ModbusDecode(char *, __u16, __u8, const struct xt_modbusTcp_mtinfo *);
#endif /* MODBUS_DECODE_H */
