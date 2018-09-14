using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FireWall
{
    [Serializable]
    public class GlobalConfig
    {
        public List<DataBaseConfig> DataBaseConfigs;

        public List<Parameter> Parameters;

        public List<LoginSetting> LoginSettings;

        public List<ScanIPConfig> ScanIPConfig;
    }

    [Serializable]
    public class DataBaseConfig
    {
        [XmlAttribute("database")]
        public string DataBase { get; set; }

        [XmlAttribute("datasource")]
        public string DataSource { get; set; }

        [XmlAttribute("userid")]
        public string UserId { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }
    }

    [Serializable]
    public class Parameter
    {
        [XmlAttribute("functioncodenum")]
        public int FunctionCodeNum { get; set; }
    }

    [Serializable]
    public class LoginSetting
    {
        [XmlAttribute("firstloginflag")]
        public Boolean firstloginflag { get; set; }
    }

    [Serializable]
    public class ScanIPConfig
    {
        [XmlAttribute("scanip")]
        public string scanip { get; set; }
    }
}
