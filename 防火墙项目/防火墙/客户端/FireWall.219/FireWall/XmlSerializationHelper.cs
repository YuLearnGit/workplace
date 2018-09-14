using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FireWall
{
    public class XmlSerializationHelper
    {
        //
        private readonly string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

        //获取配置文件夹名
        public XmlSerializationHelper(string configName)
        {
            configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configName);
        }

        //反序列化目标类
        public T Get<T>(string index = null) where T : class, new()
        {
            var result = new T();
            result = GetConfigFile<T>();
            return result;
        }
        private T GetConfigFile<T>() where T : class, new()
        {
            var result = new T();
            var fileName = GetConfigFileName<T>();
            var content = GetConfig(fileName);
            if (content == null)
            {
                SaveConfig(fileName, string.Empty);
            }
            else if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    result = (T)XmlDeserialize(typeof(T), content);
                }
                catch
                {
                    result = new T();
                }
            }
            return result;
        }

        //序列化目标类
        public void Save<T>(T configFile) where T : class
        {
            var fileName = GetConfigFileName<T>();
            SaveConfig(fileName, XmlSerialize(configFile));
        }

        //读取配置
        private string GetConfig(string fileName)
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            var configPath = GetFilePath(fileName);
            if (!File.Exists(configPath))
                return null;
            else
                return File.ReadAllText(configPath);
        }

        //保存配置
        private void SaveConfig(string fileName, string content)
        {
            var configPath = GetFilePath(fileName);
            File.WriteAllText(configPath, content);
        }

        //xml反序列化
        private static object XmlDeserialize(Type type, string xmlStr)
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(type);
            StringReader sWriter = new StringReader(xmlStr);
            return ser.Deserialize(sWriter);
        }

        //xml序列化
        private static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StringWriter sWriter = new StringWriter();
            ser.Serialize(sWriter, obj);
            return sWriter.ToString();
        }

        //获取类名
        private static string GetConfigFileName<T>()
        {
            return typeof(T).Name;
        }

        //获取文件路径名
        private string GetFilePath(string fileName)
        {
            var configPath = string.Format(@"{0}\{1}.xml", configFolder, fileName);
            return configPath;
        }
    }
}
