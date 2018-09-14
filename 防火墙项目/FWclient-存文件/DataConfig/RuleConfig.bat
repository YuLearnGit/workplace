@Rem switch to disk C
@cd\
@Rem establish config file
@Rem if exist c:\CMP\Config exit
md c:\CMP\Config
md c:\CMP\log
cd c:\CMP\Config
@Rem establish rules config file
@type nul>modbustcp
@echo ^#devIP-dst-src-addrStart-addrEnd-minSpeed-maxSpeed-func-method-logFlag>>"modbustcp"

@type nul>opc
@echo ^#devIP-dstIP-srcIP-logFlag>>"opc"

@type nul>dnp3
@echo ^#devIP-dstIP-srcIP-logFlag>>"dnp3"

@type nul>apc
@echo ^#protocol-method>>"apc"
@echo http allow>>"apc"
@echo ftp allow>>"apc"
@echo smtp allow>>"apc"
@echo pop3 allow>>"apc"
@echo telnet allow>>"apc"
@echo ssh allow>>"apc"

@type nul>arp
@echo ^#devIP-gateIP-gateMAC>>"arp"

@type nul>cnc
@echo ^#devIP-logFlag-connlimit-srcIP-dstIP-sport-dport>>"cnc"

@type nul>dnat
@echo ^#fwIP-originDIP-originDport-mapIP-mapPort>>"dnat"

@type nul>snat
@echo ^#fwIP-devIP-ethName-ethIP>>"snat"

@type nul>prt
@echo ^#default devIP-iface-gateway>>"prt"
@echo ^#host devIP-host-iface-gateway>>"prt"
@echo ^#net devIP-net-netmask-iface-gateway>>"prt"

@type nul>std
@echo ^#devIP-protocol-srcIP-dstIP-sport-dport-logFlag>>"std"

@type nul>whl
@echo ^#devIP-dstIP-srcIP-dport-sport>>"whl"

@type nul>firewallip
@echo ^#fwIP-fwMAC-devIP-devMAC-devType>>"firewallip"

@Rem establish useraccount file
@type nul>useraccount
@echo admin admin SUPER >>"useraccount"
@echo Engineer 33333 OPER>>"useraccount"
@echo Guest 111 GUEST>>"useraccount"

@Rem establish device type file
@type nul>macs
exit