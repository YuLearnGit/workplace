using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// SystemSettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSettingControl : UserControl
    {
        public SystemSettingControl()
        {
            InitializeComponent();
        }

        private void SaveXmlButton_Click(object sender, RoutedEventArgs e)
        {           
            if(UserMessageBox.Show("消息提示", "确认修改数据库信息?") == true)
            {
                string ConnectionString = "Data Source = " + DataSourceBox.Text + "; User Id = " + UserIDBox.Text + "; Password =" + PassWordBox.Text;
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                //创建数据库及必须的表
                string CommandText = " CREATE database if not exists " + DataBaseBox.Text + ";DROP TABLE IF EXISTS " + DataBaseBox.Text + ".`firewallip`;" +
                                     " CREATE TABLE " + DataBaseBox.Text + ".`firewallip` (fw_ip VARCHAR(255) DEFAULT NULL, `fw_mac` varchar(255) DEFAULT NULL,"+
                                     " dev_ip VARCHAR(255) DEFAULT NULL, dev_mac VARCHAR(255) DEFAULT NULL, dev_type VARCHAR(255) DEFAULT NULL)"+
                                     " ENGINE = InnoDB DEFAULT CHARSET = utf8;" +
                                     " DROP TABLE IF EXISTS " + DataBaseBox.Text + ".`firewallrules`;" +
                                     " CREATE TABLE " + DataBaseBox.Text + ".`firewallrules` (`fw_mac` varchar(255) DEFAULT NULL,`protocol` varchar(255) DEFAULT NULL," +
                                     " `source` varchar(255) DEFAULT NULL,`destination` varchar(255) DEFAULT NULL,`coiladdressstart` varchar(255) DEFAULT NULL," +
                                     " `coiladdressend` varchar(255) DEFAULT NULL, minspeed varchar(255) DEFAULT NULL, maxspeed varchar(255) DEFAULT NULL," +
                                     " `functioncode` varchar(255) DEFAULT NULL,`log` varchar(255) DEFAULT NULL" +
                                     " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;" +
                                     " DROP TABLE IF EXISTS " + DataBaseBox.Text + ".`useraccount`;" +
                                     " CREATE TABLE " + DataBaseBox.Text + ".`useraccount` (`UserID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT," +
                                     " `UserName` char(16) NOT NULL,`Password` char(8) DEFAULT NULL,`Purview` char(32) DEFAULT NULL," +
                                     " PRIMARY KEY(`UserID`),UNIQUE KEY `useName` (`UserName`)) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8;" +
                                     " INSERT INTO " + DataBaseBox.Text + ".`useraccount` (`UserID`, `UserName`, `Password`, `Purview`) VALUES('1', 'admin', 'admin', 'SUPER');";
                try
                {
                    //Open DataBase
                    //打开数据库
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(CommandText, conn);
                    cmd.ExecuteNonQuery();
                    //读取全局配置信息
                    XmlSerializationHelper configContext = new XmlSerializationHelper("Config");
                    GlobalConfig globalconfig = configContext.Get<GlobalConfig>();

                    globalconfig.DataBaseConfigs[0].DataBase = DataBaseBox.Text;
                    globalconfig.DataBaseConfigs[0].DataSource = DataSourceBox.Text;
                    globalconfig.DataBaseConfigs[0].UserId = UserIDBox.Text;
                    globalconfig.DataBaseConfigs[0].Password = PassWordBox.Text;
                    configContext.Save(globalconfig);

                    StaticGlobal.database = DataBaseBox.Text;
                    StaticGlobal.datasource = DataSourceBox.Text;
                    StaticGlobal.userid = UserIDBox.Text;
                    StaticGlobal.password = PassWordBox.Text;
                    StaticGlobal.ConnectionString = "Database = " + StaticGlobal.database + ";Data Source = " + StaticGlobal.datasource + "; User Id = " + StaticGlobal.userid + "; Password =" + StaticGlobal.password;
                }
                catch
                {
                    //Can not Open DataBase
                    //打开不成功 则连接不成功
                    UserMessageBox.Show("提示", "输入信息有误或Mysql服务未打开，无法连接数据库！");
                }
                finally
                {
                    //Close DataBase
                    //关闭数据库连接
                    conn.Close();
                }
            }
           
        }

        private void ResetXmlButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseBox.Text = StaticGlobal.database;
            DataSourceBox.Text = StaticGlobal.datasource;
            UserIDBox.Text = StaticGlobal.userid;
            PassWordBox.Text = StaticGlobal.password;
        }

        //判断输入是否改变
        private void textchanged(object sender, TextChangedEventArgs e)
        {
            if (DataBaseBox.Text != StaticGlobal.database || DataSourceBox.Text != StaticGlobal.datasource || UserIDBox.Text != StaticGlobal.userid || PassWordBox.Text != StaticGlobal.password )
            {
                SaveXmlButton.IsEnabled = true;
                ResetXmlButton.IsEnabled = true;
            }
            else
            {
                SaveXmlButton.IsEnabled = false;
                ResetXmlButton.IsEnabled = false;
            }
        }
    }
}
