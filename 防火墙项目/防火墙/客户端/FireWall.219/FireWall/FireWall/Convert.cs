using System;
using System.Collections.Generic;
using System.Globalization;
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
    class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {
            string strValue = value.ToString();
            if (strValue != null)
            {

                if (strValue == "172.16.10.40")
                    return "Red";

            }
            return "";
        }
        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            return "";
        }
    }
}
