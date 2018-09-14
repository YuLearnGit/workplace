#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xtables.h>
#include <linux/netfilter.h>
//#include <linux/netfilter/xt_Profinet.h>
#include <linux/types.h>
#include <linux/netfilter.h>

enum {
    Profinet_SRCPORT = 0x01,
	Profinet_DSTPORT = 0x02,

};

struct xt_Profinet_mtinfo {
	__u32 src_port;
	__u32 dst_port;	
	__u8 flags;
};


enum {
    O_srcPORT=0,
	O_dstPORT,

};

static void Profinet_mt_help(void)
{
	printf(
"Profinet match options:\n"
"--src-port src_port    Match source IP\n"
"--dst-port dst_port    Match destination IP\n");

}

static const struct xt_option_entry Profinet_mt_opts[] = {

    {.name = "src-port", .id = O_srcPORT, .type = XTTYPE_UINT32, .flags = XTOPT_INVERT},
	{.name = "dst-port", .id = O_dstPORT, .type = XTTYPE_UINT32, .flags = XTOPT_INVERT},
	
	XTOPT_TABLEEND,
};



static void Profinet_mt_parse(struct xt_option_call *cb)
{
	struct xt_Profinet_mtinfo *info = cb->data;

	xtables_option_parse(cb);
	switch (cb->entry->id) {
	case O_srcPORT:
	     	info->src_port = atoi(cb->arg);
			 if (cb->invert)
			info->flags |= Profinet_SRCPORT;
		break;
	case O_dstPORT:
		info->dst_port = atoi(cb->arg);
		 if (cb->invert)
			info->flags |= Profinet_DSTPORT;
		break;

	}
}

static void Profinet_mt_check(struct xt_fcheck_call *cb)
{
	if (cb->xflags == 0)
		xtables_error(PARAMETER_PROBLEM,
			   "Profinet match: You must specify '--src-port' and '--dst-port'");
}



static void
Profinet_mt_print(const void *ip, const struct xt_entry_match *match,
                  int numeric)
{
	const struct xt_Profinet_mtinfo *info = (const void *)match->data;
    if (info->flags & Profinet_SRCPORT) {
		printf(" source port");
	    	if (info->flags & Profinet_SRCPORT)
			printf(" !");		
		printf(" %d ", htons(info->src_port));
	}
	if (info->flags & Profinet_DSTPORT) {
		printf(" destination port");
         if (info->flags & Profinet_DSTPORT)
			printf(" !");		
		printf(" %d ", htons(info->dst_port));
	}

}




static void Profinet_mt_save(const void *ip, const struct xt_entry_match *match)
{
	const struct xt_Profinet_mtinfo *info = (const void *)match->data;
    
	  if (info->flags & Profinet_SRCPORT) {
		printf(" source port");
		if (info->flags & Profinet_SRCPORT)
			printf(" !");		
		printf(" %d ", htons(info->src_port));
	  }
	if (info->flags & Profinet_DSTPORT) {
		printf(" destination port:");
		if (info->flags & Profinet_DSTPORT)
			printf(" !");		
		printf(" %d ", htons(info->dst_port));
	}

}



static struct xtables_match Profinet_mt_reg = {

	
		.version       = XTABLES_VERSION,
		.name          = "Profinet",
		.revision      = 1,
		.family        = NFPROTO_IPV4,
		.size          = XT_ALIGN(sizeof(struct xt_Profinet_mtinfo)),
		.userspacesize = XT_ALIGN(sizeof(struct xt_Profinet_mtinfo)),
	    .help          = Profinet_mt_help,
		.x6_parse      = Profinet_mt_parse,
		.x6_fcheck     = Profinet_mt_check,
		.print         = Profinet_mt_print,
		.save          = Profinet_mt_save,
		.x6_options    = Profinet_mt_opts,  
	
};

void _init(void)
{
	xtables_register_match(&Profinet_mt_reg);
}
