#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
//#include <linux/netfilter/xt_modbusTcp.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {
	MODBUSTCP_MIN = 0x01,
	MODBUSTCP_MAX = 0x02,
	MODBUSTCP_LFC = 0x04,
	MODBUSTCP_HFC = 0x08,
};

struct xt_modbusTcp_mtinfo {
	__u16 min_addr;
	__u16 max_addr;
	__u64 lfc_flag;
	__u64 hfc_flag;
	__u8 flags;
	
};


enum {
	O_MIN = 0,
	O_MAX,
	O_LFC,
	O_HFC,
};

static void modbusTcp_mt_help(void)
{
	printf(
"modbusTcp match options:\n"
"--min-addr addr[-addr]    Match coil addr in the specified range\n"
"--max-addr addr[-addr]    Match coil addr in the specified range\n");
}

static const struct xt_option_entry modbusTcp_mt_opts[] = {
	{.name = "min-addr", .id = O_MIN, .type = XTTYPE_UINT16, .flags = XTOPT_INVERT},
	{.name = "max-addr", .id = O_MAX, .type = XTTYPE_UINT16, .flags = XTOPT_INVERT},
	{.name = "lfc-flag", .id = O_LFC, .type = XTTYPE_UINT64, .flags = XTOPT_INVERT},
	{.name = "hfc-flag", .id = O_HFC, .type = XTTYPE_UINT64, .flags = XTOPT_INVERT},
	XTOPT_TABLEEND,
};







static void modbusTcp_mt_parse(struct xt_option_call *cb)
{
	struct xt_modbusTcp_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_MIN:
		info->min_addr = atoi(cb->arg);
		if (cb->invert)
			info->flags |= MODBUSTCP_MIN;
		break;
	case O_MAX:
		info->max_addr = atoi(cb->arg);
		if (cb->invert)
			info->flags |= MODBUSTCP_MAX;
		break;
	case O_LFC :
		info->lfc_flag = atoi(cb->arg);
		if (cb->invert)
			info->flags |= MODBUSTCP_LFC;
		break;
	case O_HFC :
		info->hfc_flag = atoi(cb->arg);
		if (cb->invert)
			info->flags |= MODBUSTCP_HFC;
		break;
	}
}

static void modbusTcp_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "modbusTcp match: You must specify `--min-addr' and '--max-addr'and '--lfc-flag'and 'hfc-flag' ");
}




static void
modbusTcp_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_modbusTcp_mtinfo *info = (const void *)match->data;

	if (info->flags & MODBUSTCP_MIN) {
		printf(" coil_addr min range");
		if (info->flags & MODBUSTCP_MIN)
			printf(" !");
		/*
		 * ipaddr_to_numeric() uses a static buffer, so cannot
		 * combine the printf() calls.
		 */		
		printf(" %x", (info->min_addr));
	}
	if (info->flags & MODBUSTCP_MAX) {
		printf(" coil_addr min range");
		if (info->flags & MODBUSTCP_MAX)
			printf(" !");		
		printf(" %x", (info->max_addr));
	}
	if (info->flags & MODBUSTCP_LFC) {
		printf("function code low 64");
		if (info->flags & MODBUSTCP_LFC)
			printf(" !");		
		printf(" %x", (info->lfc_flag));
	}
	if (info->flags & MODBUSTCP_HFC) {
		printf("function code height 64");
		if (info->flags & MODBUSTCP_HFC)
			printf(" !");		
		printf(" %x", (info->hfc_flag));
	}
}





static void modbusTcp_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_modbusTcp_mtinfo *info = (const void *)match->data;

	if (info->flags & MODBUSTCP_MIN) {
		printf(" coil_addr min range");
		if (info->flags & MODBUSTCP_MIN)
			printf(" !");
		/*
		 * ipaddr_to_numeric() uses a static buffer, so cannot
		 * combine the printf() calls.
		 */		
		printf(" %x", (info->min_addr));
	}
	if (info->flags & MODBUSTCP_MAX) {
		printf(" coil_addr min range");
		if (info->flags & MODBUSTCP_MAX)
			printf(" !");		
		printf(" %x", (info->max_addr));
	}
	if (info->flags & MODBUSTCP_LFC) {
		printf("function code low 64");
		if (info->flags & MODBUSTCP_LFC)
			printf(" !");		
		printf(" %x", (info->lfc_flag));
	}
	if (info->flags & MODBUSTCP_HFC) {
		printf("function code height 64");
		if (info->flags & MODBUSTCP_HFC)
			printf(" !");		
		printf(" %x", (info->hfc_flag));
	}
}



static struct xtables_match modbusTcp_mt_reg = {

	
		.version       = XTABLES_VERSION,
		.name          = "modbusTcp",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_modbusTcp_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_modbusTcp_mtinfo)),
		.help          = modbusTcp_mt_help,
		.x6_parse      = modbusTcp_mt_parse,
		.x6_fcheck     = modbusTcp_mt_check,
		.print         = modbusTcp_mt_print,
		.save          = modbusTcp_mt_save,
		.x6_options    = modbusTcp_mt_opts,
	

};

void _init(void)
{
	xtables_register_match(&modbusTcp_mt_reg);
}
