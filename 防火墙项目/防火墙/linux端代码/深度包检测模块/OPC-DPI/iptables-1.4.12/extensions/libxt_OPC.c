#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
//#include <linux/netfilter/xt_OPC.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {

	OPC_DSTPORT = 0x01,

};

struct xt_OPC_mtinfo {
	__u32 dst_port;	
	__u8 flags;
};


enum {

	O_dstPORT=0,

};

static void OPC_mt_help(void)
{
	printf(
"OPC match options:\n"
"--dst-port dst_port    Match source IP\n");

}

static const struct xt_option_entry OPC_mt_opts[] = {

	{.name = "dst-port", .id = O_dstPORT, .type = XTTYPE_UINT32, .flags = XTOPT_INVERT},
	
	XTOPT_TABLEEND,
};



static void OPC_mt_parse(struct xt_option_call *cb)
{
	struct xt_OPC_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_dstPORT:
		info->dst_port = atoi(cb->arg);
		if (cb->invert)
			info->flags |= OPC_DSTPORT;
		break;

	}
}

static void OPC_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "OPC match: You must specify '--dst-port'");
}



static void
OPC_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_OPC_mtinfo *info = (const void *)match->data;

	if (info->dst_port !=0) {
		printf(" OPC destination port %d",info->dst_port);

	}

}




static void OPC_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_OPC_mtinfo *info = (const void *)match->data;


	if (info->dst_port !=0) {
		printf(" OPC destination port %d",info->dst_port);
	}
}



static struct xtables_match OPC_mt_reg = {

	
		.version       = XTABLES_VERSION,
		.name          = "OPC",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_OPC_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_OPC_mtinfo)),
	    .help          = OPC_mt_help,
		.x6_parse      = OPC_mt_parse,
		.x6_fcheck     = OPC_mt_check,
		.print         = OPC_mt_print,
		.save          = OPC_mt_save,
		.x6_options    = OPC_mt_opts,  
	
};

void _init(void)
{
	xtables_register_match(&OPC_mt_reg);
}
