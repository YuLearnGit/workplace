@Rem switch to disk C
cd\
@Rem establish config file
md c:\CMP\Config
cd c:\CMP\Config
@Rem found rules config file
type nul>modbustcp.config
type nul>opc.config
type nul>dnp3.config
type nul>apc.config
type nul>cnc.config
type nul>dnat.config
type nul>snat.config
type nul>prt.config
type nul>std.config
type nul>whl.config
pause