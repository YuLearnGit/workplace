#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
//#include <linux/netfilter/xt_EtherCAT.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {
    EtherCAT_SRCPORT = 0x01,
	EtherCAT_DSTPORT = 0x02,

};

struct xt_EtherCAT_mtinfo {
	__u32 src_port;
	__u32 dst_port;	
	__u8 flags;
};


enum {
    O_srcPORT=0,
	O_dstPORT,

};

static void EtherCAT_mt_help(void)
{
	printf(
"EtherCAT match options:\n"
"--src-port src_port    Match source IP\n"
"--dst-port dst_port    Match destination IP\n");

}

static const struct xt_option_entry EtherCAT_mt_opts[] = {

    {.name = "src-port", .id = O_srcPORT, .type = XTTYPE_UINT32, .flags = XTOPT_INVERT},
	{.name = "dst-port", .id = O_dstPORT, .type = XTTYPE_UINT32, .flags = XTOPT_INVERT},
	
	XTOPT_TABLEEND,
};



static void EtherCAT_mt_parse(struct xt_option_call *cb)
{
	struct xt_EtherCAT_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_srcPORT:
	     	info->src_port = atoi(cb->arg);
			 if (cb->invert)
			info->flags |= EtherCAT_SRCPORT;
		break;
	case O_dstPORT:
		info->dst_port = atoi(cb->arg);
		 if (cb->invert)
			info->flags |= EtherCAT_DSTPORT;
		break;

	}
}

static void EtherCAT_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "EtherCAT match: You must specify '--src-port' and '--dst-port'");
}



static void
EtherCAT_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_EtherCAT_mtinfo *info = (const void *)match->data;
    if (info->flags & EtherCAT_SRCPORT) {
		printf(" source port");
	    	if (info->flags & EtherCAT_SRCPORT)
			printf(" !");		
		printf(" %d ", htons(info->src_port));
	}
	if (info->flags & EtherCAT_DSTPORT) {
		printf(" destination port");
         if (info->flags & EtherCAT_DSTPORT)
			printf(" !");		
		printf(" %d ", htons(info->dst_port));
	}

}




static void EtherCAT_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_EtherCAT_mtinfo *info = (const void *)match->data;
    
	  if (info->flags & EtherCAT_SRCPORT) {
		printf(" source port");
		if (info->flags & EtherCAT_SRCPORT)
			printf(" !");		
		printf(" %d ", htons(info->src_port));
	  }
	if (info->flags & EtherCAT_DSTPORT) {
		printf(" destination port:");
		if (info->flags & EtherCAT_DSTPORT)
			printf(" !");		
		printf(" %d ", htons(info->dst_port));
	}

}



static struct xtables_match EtherCAT_mt_reg = {

	
		.version       = XTABLES_VERSION,
		.name          = "EtherCAT",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_EtherCAT_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_EtherCAT_mtinfo)),
	    .help          = EtherCAT_mt_help,
		.x6_parse      = EtherCAT_mt_parse,
		.x6_fcheck     = EtherCAT_mt_check,
		.print         = EtherCAT_mt_print,
		.save          = EtherCAT_mt_save,
		.x6_options    = EtherCAT_mt_opts,  
	
};

void _init(void)
{
	xtables_register_match(&EtherCAT_mt_reg);
}
