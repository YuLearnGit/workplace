#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {
	IEC104_MIN = 0x01,
	IEC104_MAX = 0x02,
};

struct xt_iec104_mtinfo {
	__u16 min_addr;
	__u16 max_addr;
	__u8 flags;

};


enum {
	O_MIN = 0,
	O_MAX,
};

static void iec104_mt_help(void)
{
	printf(
"iec104 match options:\n"
"--min-addr addr[-addr]    Match common addr in the specified range\n"
"--max-addr addr[-addr]    Match common addr in the specified range\n");
}

static const struct xt_option_entry iec104_mt_opts[] = {
	{.name = "min-addr", .id = O_MIN, .type = XTTYPE_UINT16, .flags = XTOPT_INVERT},
	{.name = "max-addr", .id = O_MAX, .type = XTTYPE_UINT16, .flags = XTOPT_INVERT},
	XTOPT_TABLEEND,
};

static void iec104_mt_parse(struct xt_option_call *cb)
{
	struct xt_iec104_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_MIN:
		info->min_addr = atoi(cb->arg);
		if (cb->invert)
			info->flags |= IEC104_MIN;
		break;
	case O_MAX:
		info->max_addr = atoi(cb->arg);
		if (cb->invert)
			info->flags |= IEC104_MAX;
		break;
	}
}

static void iec104_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "iec104 match: You must specify `--min-addr' and '--max-addr'");
}

static void
iec104_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_iec104_mtinfo *info = (const void *)match->data;

	if (info->flags & IEC104_MIN) {
		printf(" common_addr min range");
		if (info->flags & IEC104_MIN)
			printf(" !");
		/*
		 * ipaddr_to_numeric() uses a static buffer, so cannot
		 * combine the printf() calls.
		 */
		printf(" %x", (info->min_addr));
	}
	if (info->flags & IEC104_MAX) {
		printf(" common_addr min range");
		if (info->flags & IEC104_MAX)
			printf(" !");
		printf(" %x", (info->max_addr));
	}
}

static void iec104_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_iec104_mtinfo *info = (const void *)match->data;

	if (info->flags & IEC104_MIN) {
		printf(" common_addr min range");
		if (info->flags & IEC104_MIN)
			printf(" !");
		/*
		 * ipaddr_to_numeric() uses a static buffer, so cannot
		 * combine the printf() calls.
		 */
		printf(" %x", (info->min_addr));
	}
	if (info->flags & IEC104_MAX) {
		printf(" common_addr min range");
		if (info->flags & IEC104_MAX)
			printf(" !");
		printf(" %x", (info->max_addr));
	}
}



static struct xtables_match iec104_mt_reg = {


		.version       = XTABLES_VERSION,
		.name          = "iec104",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_iec104_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_iec104_mtinfo)),
		.help          = iec104_mt_help,
		.x6_parse      = iec104_mt_parse,
		.x6_fcheck     = iec104_mt_check,
		.print         = iec104_mt_print,
		.save          = iec104_mt_save,
		.x6_options    = iec104_mt_opts,


};

void _init(void)
{
	xtables_register_match(&iec104_mt_reg);
}
