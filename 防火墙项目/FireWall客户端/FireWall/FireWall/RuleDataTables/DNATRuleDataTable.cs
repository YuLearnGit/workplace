using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FireWall
{
   public class DNATRuleDataTable: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string origin_dstIP { get; set; }
        public string origin_dport { get; set; }
        public string map_IP { get; set; }
        public string map_port { get; set; }
    }
}
