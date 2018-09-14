#include <iostream>
#include <string>
#include <memory>

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <arpa/inet.h>
#include <net/if.h>
#include <string.h>
#include <signal.h>
#include <sys/wait.h>
#include <sys/ioctl.h>
#include <errno.h>
#include <sys/socket.h>
#include <net/if_arp.h>
#include <netinet/in.h>
#include <linux/sockios.h>

#include "SecurityDev.h"


using namespace std;

shared_ptr<SecurityDev> SecurityDev::self_ptr = nullptr;

SecurityDev::SecurityDev() {
  IP_addr = getLocalIP();
  MAC_addr = getLocalMAC();
}

shared_ptr<SecurityDev> SecurityDev::getInstance() {
  if(self_ptr == nullptr)
    self_ptr = shared_ptr<SecurityDev>(new SecurityDev);
  return self_ptr;
}

std::string SecurityDev::getLocalIP() {
  struct sockaddr_in sin;
  struct ifreq ifr;
  char *temp_ip = NULL;

  int sock = socket(AF_INET, SOCK_DGRAM, 0);
  if(sock == -1) {
    perror("socket");
    return string();
  }

  strncpy(ifr.ifr_name, "br0", IFNAMSIZ-1);
  ifr.ifr_name[IFNAMSIZ-1] = 0;
  if(ioctl(sock, SIOCGIFADDR, &ifr) < 0) {
    perror("ioctl");
    return string();
  }

  memcpy(&sin, &ifr.ifr_addr, sizeof(sin));
  temp_ip = inet_ntoa(sin.sin_addr);
  /*
#ifdef DEBUG
  fprintf(stdout, "br0: %s\n", temp_ip);
#endif
*/
  close(sock);
  return string(temp_ip);
}

std::string SecurityDev::getLocalMAC() {
  char buf[20];
  string cmd = "cat /sys/class/net/br0/address";

  FILE *stream = popen(cmd.c_str(), "r");
  fread(buf, sizeof(char), sizeof(buf), stream);
  buf[17] = '\0';
  /*
#ifdef DEBUG
  cout << "localmac : " << buf << endl;
#endif
*/
  return string(buf);
}
