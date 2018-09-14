using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace FireWall
{
   public class PropertyDataTble : INotifyPropertyChanged
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
        public string fwname{ get; set; }
        public string fwID { get; set; }
        public string fwIP { get; set; }
        public string description { get; set; }
    }
}
