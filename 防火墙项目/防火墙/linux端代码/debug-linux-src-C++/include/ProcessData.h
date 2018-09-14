#ifndef PROCESSDATA_H
#define PROCESSDATA_H


#include <memory>
#include <linux/netlink.h>
#include <libnetfilter_log/libnetfilter_log.h>
#include <libnetfilter_log/linux_nfnetlink_log.h>
#include <libnfnetlink/libnfnetlink.h>

#include "SecurityDev.h"
#include "ClientDev.h"
#include "ProcessRules.h"
#include "ProtectedDev.h"

#define MAX_MSG_SIZE 1024 /*接收缓冲区大小*/
#define LOG_CP_RANGE 1024 /*拷贝的数据包范围*/
#define NETLINK_GROUP 10
#define DEBUG

class ProcessData {
 public:
  ProcessData(std::shared_ptr<SecurityDev> s_ptr,
              std::shared_ptr<ClientDev> c_ptr,
              std::shared_ptr<ProcessRules> p_ptr);
  void processData();

 private:
  static int packet_callback(struct nflog_g_handle *gh, struct nfgenmsg *nfmsg,
                      struct nflog_data *nfa, void *data);
  static void addProtecdev(const std::string &dev_ip, std::shared_ptr<ProtectedDev> protec_ptr);

  static std::shared_ptr<SecurityDev> securdev_ptr;
  static std::shared_ptr<ClientDev> clidev_ptr;
  static std::shared_ptr<ProcessRules> prorule_ptr;
  static std::map<std::string, std::shared_ptr<ProtectedDev>> protec_devs;
};

#endif // PROCESSDATA_H
