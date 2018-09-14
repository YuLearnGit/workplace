using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace FireWall
{
  public  class SNATRuleDataTable: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string origin_devIP { get; set; }
        public string EthName { get; set; }
        public string EthIP { get; set; }
        public string NATIP { get; set; }

    }
}
