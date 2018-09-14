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

enum {
	MODBUSTCP_ADDR = 0x01,
	MODBUSTCP_FUNC = 0x02,
	MODBUSTCP_DATA = 0x04,
		
};

#define MODBUS_LEN_OK									1
#define MODBUS_LEN_FAIL									0

#define MODBUS_COIL_ADDR_CHECK_OK						1
#define MODBUS_COIL_ADDR_CHECK_FAIL						0

#define MODBUS_DATA_ADDR_CHECK_OK						1
#define MODBUS_DATA_ADDR_CHECK_FAIL						0

#define MODBUS_DATA_CHECKED_OK  1
#define MODBUS_DATA_CHECKED_FAIL  0

#define FROM_CLIENT_FLAG		0
#define FROM_SERVER_FLAG		1

#define MODBUS_FUNCODE_REJECT       0
#define MODBUS_FUNCODE_RECEIVE	 1

#define MODBUS_FUNCODE_NOT_MATCHED  0
#define MODBUS_FUNCODE_MATCHED      1

#define THIS_IS_MATCHED_FUN_CODE 				1
#define THIS_IS_NOT_MATCHED_FUN_CODE 	 0

struct xt_modbusTcp_mtinfo {
	__u16 data_addr[2];   /* Modbus data address.  */
	__u8 modbus_func[2];  /* Modbus function code. */
	__u16 con_data[2];    /* Control data range.   */     
	__u8 flags;
	
};

int ModbusDecode(char *, __u16, __u8, const struct xt_modbusTcp_mtinfo *);
#endif /* MODBUS_DECODE_H */
