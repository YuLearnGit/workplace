using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace FireWall
{
  public  class WHLRuleDataTable : INotifyPropertyChanged
    {
        //private bool _check = false;
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string dst_IP { get; set; }
        public string src_IP { get; set; }
        public string dst_port { get; set; }
        public string src_port { get; set; }
        public bool log { get; set; }
    
    }
}
