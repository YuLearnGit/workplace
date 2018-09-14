using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireWall
{
    public class FireWallDevices
    {
        private int listindex;
        private string FireWallIP;
        private string FireWallMAC;
        private ObservableCollection<FireWallRuleDataTable> FireWallRuleLists = new ObservableCollection<FireWallRuleDataTable>();

        private ObservableCollection<PropertyDataTble> propertyLists = new ObservableCollection<PropertyDataTble>();

        private ObservableCollection<SNATRuleDataTable> SNATRuleLists = new ObservableCollection<SNATRuleDataTable>();

        private ObservableCollection<DNATRuleDataTable> DNATRuleLists = new ObservableCollection<DNATRuleDataTable>();

        private ObservableCollection<WHLRuleDataTable> WHLRuleLists = new ObservableCollection<WHLRuleDataTable>();

        private ObservableCollection<APCRuleDataTable> APCRuleLists = new ObservableCollection<APCRuleDataTable>();

        private ObservableCollection<CNCRuleDataTable> CNCRuleLists = new ObservableCollection<CNCRuleDataTable>();

        private ObservableCollection<PRTRuleDataTable> FireWallPRTLists = new ObservableCollection<PRTRuleDataTable>();

        private ObservableCollection<STDRuleDataTable> STDRuleLists = new ObservableCollection<STDRuleDataTable>();

        public FireWallDevices(int listindex, string FireWallIP, string FireWallMAC)
        {
            this.listindex = listindex;
            this.FireWallIP = FireWallIP;
            this.FireWallMAC = FireWallMAC;
        }

        public int getListindex()
        {
            return listindex;
        }

        public void setListindex(int listindex)
        {
            this.listindex = listindex;
        }

        public string getFireWallIP()
        {
            return FireWallIP;
        }

        public void setFireWallIP(string FireWallIP)
        {
            this.FireWallIP = FireWallIP;
        }

        public string getFireWallMAC()
        {
            return FireWallMAC;
        }

        public void setFireWallMAC(string FireWallMAC)
        {
            this.FireWallMAC = FireWallMAC;
        }

        public void addFireWallRule(FireWallRuleDataTable fwrdt)
        {
            FireWallRuleLists.Add(fwrdt);
        }

        public ObservableCollection<FireWallRuleDataTable> getFireWallRule_list()
        {
            return FireWallRuleLists;
        }

        public ObservableCollection<PropertyDataTble> getproperty_list()
        {
            return propertyLists;
        }
        public void addproperty_list(PropertyDataTble fwrdt)
        {
            propertyLists.Add(fwrdt);
        }

        public void addSNATRule(SNATRuleDataTable fwrdt)
        {
            SNATRuleLists.Add(fwrdt);
        }

        public ObservableCollection<SNATRuleDataTable> getSNATRule_list()
        {
            return SNATRuleLists;
        }
        public void addDNATRule(DNATRuleDataTable fwrdt)
        {
            DNATRuleLists.Add(fwrdt);
        }

        public ObservableCollection<DNATRuleDataTable> getDNATRule_list()
        {
            return DNATRuleLists;
        }

        public ObservableCollection<WHLRuleDataTable> getWHLRule_list()
        {
            return WHLRuleLists;
        }
        public void addWHLRule(WHLRuleDataTable fwrdt)
        {
            WHLRuleLists.Add(fwrdt);
        }

        public ObservableCollection<APCRuleDataTable> getAPCRule_list()
        {
            return APCRuleLists;
        }

        public ObservableCollection<CNCRuleDataTable> getCNCRule_list()
        {
            return CNCRuleLists;            
        }
        public void addCNCRule(CNCRuleDataTable fwrdt)
        {
            CNCRuleLists.Add(fwrdt);
        }
        public void addFireWallPRTRule(PRTRuleDataTable fwrdt)
        {
            FireWallPRTLists.Add(fwrdt);
        }
        public ObservableCollection<PRTRuleDataTable> getFireWallPRTRule_list()
        {
            return FireWallPRTLists;
        }

        public void addSTDrule(STDRuleDataTable fwrdt)
        {
            STDRuleLists.Add(fwrdt);
        }
        public ObservableCollection<STDRuleDataTable> getSTDRule_list()
        {
            return STDRuleLists;
        }
    }
}
