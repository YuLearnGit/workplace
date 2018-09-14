#include <linux/kernel.h>
#include <linux/module.h>
#include <linux/ip.h>
#include <linux/version.h>
#include <linux/skbuff.h>
#include <linux/netfilter.h>
#include <linux/netfilter_ipv4.h>
#include <linux/moduleparam.h>
#include <linux/netfilter_ipv4/ip_tables.h>
#include <linux/init.h>
#include <linux/types.h>
#include <linux/inet.h>
#include <linux/in.h>
#include <net/tcp.h>

#include "OPC_decode.h"

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Yu Renfei");
MODULE_DESCRIPTION("OPC DPI");

#define NIPQUAD(addr) \
((unsigned char *)&addr)[0], \
((unsigned char *)&addr)[1], \
((unsigned char *)&addr)[2], \
((unsigned char *)&addr)[3]


//static int pktcnt = 0;

static unsigned int OPC_hook(unsigned int hooknum, struct sk_buff *skb, const struct net_device *in, const struct net_device *out, int (*okfn)(struct sk_buff *))

{
	__be32 sip,dip;// __u32 sport,dport;
	if(skb){
        struct iphdr *iph;
		struct tcphdr *tcph;
		char *data_start;
		char *p_data;
		int app_len = 0;

		iph  = ip_hdr(skb);
		if(iph->protocol == IPPROTO_TCP){
            tcph =  (struct tcphdr *)(skb_network_header(skb) + ip_hdrlen(skb));
            sip = iph->saddr;
            dip = iph->daddr;
           // sport=tcph->source;
           // dport=tcph->dest;

            if(tcph->dest == htons(135)){
                if(skb_is_nonlinear(skb)){
                    if(skb_linearize(skb) != 0)
                        printk(KERN_INFO"skb_linearize failed.\n");
                }

                iph = ip_hdr(skb);
                tcph = (struct tcphdr *)(skb_network_header(skb) + ip_hdrlen(skb));
                data_start = ((char *)tcph) + (tcph->doff << 2);
                printk("\n@@@@@@@@@@@@@@@@@@\n");
                printk("sip: %p\ndip: %p\n ", sip, dip);
                printk("Packet for source address: %d.%d.%d.%d\nPacket for destination address: %d.%d.%d.%d\n ", NIPQUAD(sip), NIPQUAD(dip));
                printk("Packet for source port: %d\nPacket for destination port: %d\n", htons(sport),htons(dport));

                printk("total len: %d    ipheader len:  %d     tcpheader len: %d   \n", (skb->data)[3] , iph->ihl,  tcph->doff);
                printk("sb->data:   ");
                for( p_data = data_start; p_data < (skb->tail);p_data++)
                          {
                				app_len++;
                				printk("%x", (__u8) *p_data);
                          }
                printk("\n***************************\n");
                printk("\n*********OPC dpi********\n");
                if ( OPCDecode(data_start, app_len) == OPC_FAIL)
                	{
                		printk("OPC decode: not OPC proto !!! Failed!!! \n");
                		return NF_DROP;
                	}
                else {
                	printk("OPC decode: OPC proto !!! Success!!! \n");
                	return NF_ACCEPT;
                }
            }
            return NF_ACCEPT;
		}
		return NF_ACCEPT;
    }
	return NF_ACCEPT;
}



static struct nf_hook_ops nfho={

        .hook           = OPC_hook,

        .owner          = THIS_MODULE,

        .pf             = PF_INET,

        .hooknum        = NF_INET_FORWARD, //挂载在本地出口处

        .priority       = 0,  //优先级最高

};



static int __init myhook_init(void)

{
    printk(KERN_ALERT"resgister success\n");
    return nf_register_hook(&nfho);

}



static void __exit myhook_fini(void)

{
    printk(KERN_ALERT"unresgister success\n");
    nf_unregister_hook(&nfho);

}



module_init(myhook_init);

module_exit(myhook_fini);
