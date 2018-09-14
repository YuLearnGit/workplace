#ifndef SENDINFO_H_INCLUDED
#define SENDINFO_H_INCLUDED

#include <string>
#include <sys/types.h>

#define DEBUG

int sendinfo(std::string dst_ip, std::string src_ip, std::string dst_mac, std::string src_mac,
             u_int16_t src_port, u_int16_t dst_port, std::string content);
int mac_str_to_bin(u_char *str, u_char *mac);

#endif // SENDINFO_H_INCLUDED
