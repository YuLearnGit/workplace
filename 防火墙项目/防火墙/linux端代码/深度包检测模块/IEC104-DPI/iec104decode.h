/*
 * iec104decode.h
 *
 *  Created on: 2016Äê12ÔÂ24ÈÕ
 *      Author: gyw
 */

#ifndef IEC104_DECODE_H
#define IEC104_DECODE_H

#define     IEC104_MIN_LEN                  6

#define     IEC104_OK                       1
#define     IEC104_FAIL                     (-1)

#define     IEC104_LEN_OK					1
#define     IEC104_LEN_FAIL					0

#define     IEC104_CONTROL_UNIT_OK			1
#define     IEC104_CONTROL_UNIT_FAIL		0

#define     IEC104_ASDU_OK					1
#define     IEC104_ASDU_FAIL				0


/*
 * U格式函数码 Function
 * */
#define     IEC104_U_FUNC_STARTDT           0x07		//0B00000111
#define     IEC104_U_FUNC_STOPDT            0x13		//0B00010011
#define     IEC104_U_FUNC_TESTER            0x43		//0B01000011

#define     IEC104_U_FUNC_STARTDT_ACK       0x0B		//0B00001011
#define     IEC104_U_FUNC_STOPDT_ACK        0x23		//0B00100011
#define     IEC104_U_FUNC_TESTER_ACK        0x83		//0B10000011


/*
 * ASDU TYPE
 * */
/*TI.*/
#define     IEC10X_TI_FIRM_UPDATE           128
#define     IEC10X_TI_AP_FIRM_BACKOFF       129
#define     IEC10X_TI_AP_BASE_INFO          130

#define		IEC104_C_SC_NA_1				45			//单点命令
#define		IEC104_C_DC_NA_1				46			//双点命令
#define		IEC104_C_RC_NA_1				47			//升降命令
#define		IEC104_C_SE_NA_1				48			//设定值命令，规一化值
#define		IEC104_C_SE_NB_1				49			//设定值命令，标度化值
#define		IEC104_C_SE_NC_1				50			//设定值命令，短浮点数
#define		IEC104_C_BO_NA_1				51			//32比特串
#define		IEC104_C_SC_TA_1				58			//带CP56Time2a时标的单点命令
#define		IEC104_C_DC_TA_1				59			//带CP56Time2a时标的双点命令
#define		IEC104_C_RC_TA_1				60			//带CP56Time2a时标的升降命令
#define		IEC104_C_SE_TA_1				61			//带CP56Time2a时标的设定值命令，规一化
#define		IEC104_C_SE_TB_1				62			//带CP56Time2a时标的设定值命令，标度化值
#define		IEC104_C_SE_TC_1				63			//带CP56Time2a时标的设定值命令，短浮点数
#define		IEC104_C_BO_TA_1				64			//带CP56Time2a时标的32比特串
#define		IEC104_M_EI_NA_1				70			//初始化结束
#define		IEC104_C_IC_NA_1				100 		// 总召唤命令
#define		IEC104_C_CI_NA_1				101 		// 电能脉冲召唤命令
#define		IEC104_C_RD_NA_1				102 		// 读命令
#define		IEC104_C_CS_NA_1				103 		// 时钟同步命令
#define		IEC104_C_RP_NA_1				105 		// 复位进程命令
#define		IEC104_C_TS_TA_1				107 		// 带CP56Time2a时标的测试命令
#define		IEC104_P_ME_NA_1				110 		// 测量值参数，规一化值
#define		IEC104_P_ME_NB_1				111 		// 测量值参数，标度化值
#define		IEC104_P_ME_NC_1				112 		// 测量值参数，短浮点数
#define		IEC104_P_AC_NA_1				113 		// 参数激活
#define		IEC104_F_FR_NA_1				120 		// 文件已准备好
#define		IEC104_F_SR_NA_1				121 		// 节已准备好
#define		IEC104_F_SC_NA_1				122 		// 召唤目录，选择文件，召唤文件，召唤节
#define		IEC104_F_LS_NA_1				123 		// 最后的节，最后的段
#define		IEC104_F_AF_NA_1				124 		// 确认文件，确认节
#define		IEC104_F_SG_NA_1				125 		// 段
#define     IEC104_F_DR_NA_1                126 		// 目录

/*
 * ASDU REASON
 * */
#define		IEC104_REASON_per 			    1 			//周期、循环
#define		IEC104_REASON_Back    			2 			//背景扫描
#define		IEC104_REASON_spont   			3 			//突发
#define		IEC104_REASON_init    			4 			//初始化
#define		IEC104_REASON_req     			5 			//请求或被请求
#define		IEC104_REASON_act     			6 			//激活
#define		IEC104_REASON_actcon  			7 			//激活确认
#define		IEC104_REASON_deact   			8 			//停止激活
#define		IEC104_REASON_deactcon			9 			//停止激活确认
#define		IEC104_REASON_actterm 			10			//激活结束
#define		IEC104_REASON_retrem  			11			//远程命令引起的返送信息
#define		IEC104_REASON_retloc  			12			//当地命令引起的返送信息
#define		IEC104_REASON_file    			13			//文件传送
#define		IEC104_REASON_introgen			20			//响应总召唤
#define		IEC104_REASON_intro1  			21			//响应第1组召唤
#define		IEC104_REASON_intro2  			22			//响应第2组召唤
#define		IEC104_REASON_intro3  			23			//响应第3组召唤
#define		IEC104_REASON_intro4  			24			//响应第4组召唤
#define		IEC104_REASON_intro5  			25			//响应第5组召唤
#define		IEC104_REASON_intro6  			26			//响应第6组召唤
#define		IEC104_REASON_intro7  			27			//响应第7组召唤
#define		IEC104_REASON_intro8  			28			//响应第8组召唤
#define		IEC104_REASON_intro9  			29			//响应第9组召唤
#define		IEC104_REASON_intro10 			30			//响应第10组召唤
#define		IEC104_REASON_intro11 			31			//响应第11组召唤
#define		IEC104_REASON_intro12 			32			//响应第12组召唤
#define		IEC104_REASON_intro13 			33			//响应第13组召唤
#define		IEC104_REASON_intro14 			34			//响应第14组召唤
#define		IEC104_REASON_intro15 			35			//响应第15组召唤
#define		IEC104_REASON_intro16 			36			//响应第16组召唤
#define		IEC104_REASON_reqcogcn			37			//响应计数量总召唤
#define		IEC104_REASON_reqco1  			38			//响应第1组计数量召唤
#define		IEC104_REASON_reqco2  			39			//响应第2组计数量召唤
#define		IEC104_REASON_reqco3  			40			//响应第3组计数量召唤
#define		IEC104_REASON_reqco4  			41			//响应第4组计数量召唤

//iptables传入的参数
struct xt_iec104_mtinfo {
	__u16 min_addr;
	__u16 max_addr;
	__u8 flags;
};

int IEC104Decode(char *app_data, __u16 app_len, __u8 flags, const struct xt_iec104_mtinfo *info);
#endif
