using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace FireWall
{
  public  class STDRuleDataTable : INotifyPropertyChanged
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
        public bool log { get; set; }
        public string protocol { get; set; }
        public string srcIP { get; set; }
        public string dstIP { get; set; }
        public string sport { get; set; }
        public string dport { get; set; }

    }
    
    
}
