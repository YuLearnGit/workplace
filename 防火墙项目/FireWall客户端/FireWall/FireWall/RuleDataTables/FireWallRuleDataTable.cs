using System;
using System.Collections.Generic;
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
    public class FireWallRuleDataTable : INotifyPropertyChanged
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

        public string protocol { get; set; }

        public string source { get; set; }

        public string destination { get; set; }

        public string coiladdressstart { get; set; }

        public string coiladdressend { get; set; }

        public string func { get; set; }
        public int mindata { get; set; }
        public int maxdata { get; set; }

        public bool log { get; set; }
        //public bool log
        //{
        //    get { return _check; }
        //    set
        //    {
        //        if (_check != value)
        //        {
        //            _check = value;
        //            NotifyPropertyChanged("log");
        //        }
        //    }
        //}
    }
}
