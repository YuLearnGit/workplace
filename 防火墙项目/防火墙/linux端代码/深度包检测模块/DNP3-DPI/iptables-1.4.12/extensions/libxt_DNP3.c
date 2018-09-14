#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
//#include <linux/netfilter/xt_DNP3.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {

	DNP3_DSTPORT = 0x01,

};

struct xt_DNP3_mtinfo {
	__u32 dst_port;	
	__u8 flags;
};


enum {

	O_dstPORT=0,

};

static void DNP3_mt_help(void)
{
	printf(
"DNP3 match options:\n"
"--dst-port dst_port    Match source IP\n");

}

static const struct xt_option_entry DNP3_mt_opts[] = {

	{.name = "dst-port", .id = O_dstPORT, .type = XTTYPE_UINT32RC, .flags = XTOPT_INVERT},
	
	XTOPT_TABLEEND,
};



static void DNP3_mt_parse(struct xt_option_call *cb)
{
	struct xt_DNP3_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_dstPORT:
		info->dst_port = atoi(cb->arg);
		if (cb->invert)
			info->flags |= DNP3_DSTPORT;
		break;

	}
}

static void DNP3_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "DNP3 match: You must specify '--dst-port'");
}



static void
DNP3_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_DNP3_mtinfo *info = (const void *)match->data;

	if (info->dst_port !=0) {
		printf(" DNP3 destination port %d",info->dst_port);
	}

}




static void DNP3_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_DNP3_mtinfo *info = (const void *)match->data;


	if (info->dst_port !=0) {
		printf(" DNP3 destination port %d",info->dst_port);

	}

}



static struct xtables_match DNP3_mt_reg = {

	
		.version       = XTABLES_VERSION,
		.name          = "DNP3",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_DNP3_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_DNP3_mtinfo)),
	    .help          = DNP3_mt_help,
		.x6_parse      = DNP3_mt_parse,
		.x6_fcheck     = DNP3_mt_check,
		.print         = DNP3_mt_print,
		.save          = DNP3_mt_save,
		.x6_options    = DNP3_mt_opts,  
	
};

void _init(void)
{
	xtables_register_match(&DNP3_mt_reg);
}
