using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
namespace FireWall
{
    class DB_DataGridbinding
    {
        private string con_str = StaticGlobal.ConnectionString;
        private MySqlConnection conn;
 
        public DB_DataGridbinding()
        {
            try
            {
                StaticGlobal.firewallindex = (from devices in StaticGlobal.FireWalldevices
                                              where devices.getFireWallMAC() == StaticGlobal.firewallmac
                                              select devices.getListindex()).ToList<int>()[0];
                this.conn = new MySqlConnection(con_str);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //属性textbox绑定
        public void PropertyBind()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from fwproperty where 防火墙ID='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getproperty_list().Clear();
                StaticGlobal.property.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    PropertyDataTble datatable = new PropertyDataTble();
                    datatable.fwname = dr[0].ToString();
                    datatable.fwID = dr[1].ToString();
                    datatable.fwIP = dr[2].ToString();
                    datatable.description = dr[3].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getproperty_list().Add(datatable);
                    StaticGlobal.property.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }
        //NATdatagrid绑定
        public void SNATDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from snat where fwmac='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Clear();
                StaticGlobal.SNAToldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    SNATRuleDataTable datatable = new SNATRuleDataTable();
                    datatable.origin_devIP = dr[1].ToString();
                    datatable.EthName = dr[2].ToString();
                    datatable.EthIP = dr[3].ToString();
                    datatable.NATIP = dr[4].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSNATRule_list().Add(datatable);
                    StaticGlobal.SNAToldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }
        public void DNATDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from dnat where fwmac='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Clear();
                StaticGlobal.DNAToldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    DNATRuleDataTable datatable = new DNATRuleDataTable();
                    datatable.origin_dstIP = dr[1].ToString();
                    datatable.origin_dport = dr[2].ToString();
                    datatable.map_IP = dr[3].ToString();
                    datatable.map_port = dr[4].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getDNATRule_list().Add(datatable);
                    StaticGlobal.DNAToldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }
        //WHLdatagrid绑定
        public void WHLDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from whl where fwmac='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Clear();
                StaticGlobal.WHLoldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    WHLRuleDataTable datatable = new WHLRuleDataTable();
                    datatable.log = Convert.ToBoolean(dr[5]);
                    datatable.dst_IP = dr[1].ToString();
                    datatable.src_IP = dr[2].ToString();
                    datatable.dst_port = dr[3].ToString();
                    datatable.src_port = dr[4].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getWHLRule_list().Add(datatable);
                    StaticGlobal.WHLoldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }
   
        //DPIdatagrid绑定
        public void DPIDB_Gridbinding()
        {
            try
            {         
                conn.Open();
                string sqlstring = "select * from modbustcp where fw_mac = '" + StaticGlobal.firewallmac + "'";               
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Clear();
                StaticGlobal.oldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstring, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    FireWallRuleDataTable datatable = new FireWallRuleDataTable();
                    datatable.protocol = dr[1].ToString();
                    datatable.source = dr[2].ToString();
                    datatable.destination = dr[3].ToString();
                    datatable.coiladdressstart = dr[4].ToString();
                    datatable.coiladdressend = dr[5].ToString();
                    datatable.mindata = Convert.ToInt32(dr[6]);
                    datatable.maxdata = Convert.ToInt32(dr[7]);
                    datatable.func = dr[8].ToString();
                    datatable.log = Convert.ToBoolean(dr[9]);
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallRule_list().Add(datatable);
                    StaticGlobal.oldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        //APCdatagrid绑定
        public void APCDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from apc";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getAPCRule_list().Clear();
                StaticGlobal.APColdrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    APCRuleDataTable datatable = new APCRuleDataTable();
                    datatable.protocol = dr[0].ToString();
                    datatable.status = dr[1].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getAPCRule_list().Add(datatable);
                    StaticGlobal.APColdrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        //CNCdatagrid绑定
        public void CNCDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from cnc where fwmac='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Clear();
                StaticGlobal.CNColdrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    CNCRuleDataTable datatable = new CNCRuleDataTable();
                    datatable.log = Convert.ToBoolean(dr[1]);
                    datatable.connlimit = Convert.ToInt16(dr[2]);
                    datatable.srcIP = dr[3].ToString();
                    datatable.dstIP = dr[4].ToString();
                    datatable.sport = dr[5].ToString();
                    datatable.dport = dr[6].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getCNCRule_list().Add(datatable);
                    StaticGlobal.CNColdrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        //PRTdatagrid绑定
        public void PRTDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from prt where fwmac='"+ StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Clear();
                StaticGlobal.PRToldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    PRTRuleDataTable datatable = new PRTRuleDataTable();
                    datatable.route_type = dr[1].ToString();
                    datatable.host_IP = dr[2].ToString();
                    datatable.dstIP = dr[3].ToString();
                    datatable.netmask = dr[4].ToString();
                    datatable.ETH = dr[5].ToString();
                    datatable.log = Convert.ToBoolean(dr[6]);            
                    datatable.Gateway = dr[7].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getFireWallPRTRule_list().Add(datatable);
                    StaticGlobal.PRToldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }
        //STDdatagrid绑定
        public void STDDB_Gridbinding()
        {
            try
            {
                conn.Open();
                string sqlstr = "select * from std where fwmac='" + StaticGlobal.firewallmac + "'";
                StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Clear();
                StaticGlobal.STDoldrules.Clear();
                MySqlCommand cm = new MySqlCommand(sqlstr, conn);
                MySqlDataReader dr = cm.ExecuteReader();
                //绑定
                while (dr.Read())
                {
                    STDRuleDataTable datatable = new STDRuleDataTable();
                    datatable.log = Convert.ToBoolean(dr[1]);
                    datatable.protocol = dr[2].ToString();
                    datatable.srcIP = dr[3].ToString();
                    datatable.dstIP = dr[4].ToString();
                    datatable.sport = dr[5].ToString();
                    datatable.dport = dr[6].ToString();
                    StaticGlobal.FireWalldevices[StaticGlobal.firewallindex].getSTDRule_list().Add(datatable);
                    StaticGlobal.STDoldrules.Add(datatable);
                }
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("exceptionInfo {0}", e);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
