using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireWall
{
    class XmlHelper
    {
        //读XML
        public static void ReadXmlConfig()
        {
            XmlSerializationHelper xmlSerialization = new XmlSerializationHelper("Config");

        }

        //写XML
        public static bool WriteXmlConfig<T>(T configFile) where T : class
        {
            XmlSerializationHelper xmlSerialization = new XmlSerializationHelper("Config");
            xmlSerialization.Save<T>(configFile);
            return true;
        }
    }
}
