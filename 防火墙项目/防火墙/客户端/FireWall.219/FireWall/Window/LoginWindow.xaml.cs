using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FireWall
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool flag;
        public string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Settings.ini";//ini文件路径
        Thread t;

        public LoginWindow()
        {
            InitializeComponent();
            //判断标志符，防止配置文件重复加载报错       
            if (StaticGlobal.flag == true)
            {
                //配置文件读取
                GlobalXmlConfig.ReadXmlConfig();
            }
            string[] points = (string[])ReadKeys("UserLog").ToArray(typeof(string));
            if (points.Length != 0)
            {
                userBox.Text = points[0];
                passwordBox.Focus();
            }
            else
            {
                userBox.Focus();
            }
        }

        [DllImport("Kernel32")]
        private extern static int GetPrivateProfileStringA(string strAppName, string strKeyName, string sDefault, byte[] buffer, int nSize, string strFileName);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /*----------------------------------------------------------------
        //函数说明：写入ini文件里的数据//
        //输入：节，键，值//
        //输出：无//
        //----------------------------------------------------------------*/
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, iniPath);
        }

        /*----------------------------------------------------------------
       //函数说明：读取ini文件//
       //输入：节名//
       //输出：无//
       //----------------------------------------------------------------*/
        public ArrayList ReadKeys(string sectionName)
        {
            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileStringA(sectionName, null, "", buffer, buffer.GetUpperBound(0), iniPath);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }

        /*----------------------------------------------------------------
       //函数说明：删除ini文件里的数据//
       //输入：节，键//
       //输出：无//
       //----------------------------------------------------------------*/
        public bool DeleteIniKey(string section, string key)
        {
            try
            {
                if (section.Trim().Length <= 0 || key.Trim().Length <= 0)
                {
                    flag = false;
                }
                else
                {
                    if (WritePrivateProfileString(section, key, null, iniPath) == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        /*----------------------------------------------------------------
       //函数说明：记录上一次登录的用户名//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        public void WriteUserLog()
        {
            string[] oldPoints;
            oldPoints = (string[])ReadKeys("UserLog").ToArray(typeof(string));
            foreach (string o in oldPoints)
            {
                DeleteIniKey("UserLog", o);
            }
            string user = userBox.Text;
            IniWriteValue("UserLog", user, "1");
        }

        /*----------------------------------------------------------------
       //函数说明：登录按键//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void loginbutton_Click(object sender, RoutedEventArgs e)
        {
            WriteUserLog();
            userBox.IsEnabled = false;
            passwordBox.IsEnabled = false;
            if ((userBox.Text == "") || (passwordBox.Password == ""))
            {
                userBox.IsEnabled = true;
                passwordBox.IsEnabled = true;
                //MessageBox.Show("用户名密码不能为空！");
                UserMessageBox.Show("提示", "用户名和密码不能为空！");
            }
            else
            {
                bool loginflag = false;
                if (StaticGlobal.firstloginflag == true)
                {
                    if ((userBox.Text == "admin") && (passwordBox.Password == "admin"))
                    {
                        loginflag = true;
                        StaticGlobal.UserName = "admin";
                        StaticGlobal.UserID = 0;
                        StaticGlobal.UserRole = "SUPER";
                    }
                    else
                    {
                        userBox.IsEnabled = true;
                        passwordBox.IsEnabled = true;
                        //MessageBox.Show("用户名密码错误！");
                        UserMessageBox.Show("提示", "用户名或密码错误！");
                        passwordBox.Clear();
                        loginflag = false;
                    }
                }
                else
                {
                    string CommandText = "select * from useraccount where UserName = '" + userBox.Text + "'and Password = '" + passwordBox.Password + "'";
                    DataSet result = new DataSet();
                    try
                    {
                        MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                        conn.Open();
                        MySqlDataAdapter s = new MySqlDataAdapter(CommandText, conn);
                        s.Fill(result);
                        conn.Close();
                        if (result.Tables[0].Rows.Count == 0)
                        {
                            userBox.IsEnabled = true;
                            passwordBox.IsEnabled = true;
                            //MessageBox.Show("用户名密码错误！");
                            UserMessageBox.Show("提示", "用户名或密码错误！");
                            passwordBox.Clear();
                            loginflag = false;
                        }
                        else
                        {
                            StaticGlobal.UserName = userBox.Text;
                            StaticGlobal.UserID = Convert.ToInt16(result.Tables[0].Rows[0][0]);
                            StaticGlobal.UserRole = result.Tables[0].Rows[0][3].ToString();
                            loginflag = true;
                        }
                    }
                    catch (Exception)
                    {
                        userBox.IsEnabled = true;
                        passwordBox.IsEnabled = true;
                        //MessageBox.Show("数据库未连接");
                        UserMessageBox.Show("提示", "数据库未连接！");
                    }
                }
                if (loginflag == true)
                {
                    LoginSuccess.Visibility = Visibility.Visible;
                    LoginGrid.Visibility = Visibility.Collapsed;
                    Welcome.Content = userBox.Text;
                    t = new Thread(new ThreadStart(() =>
                    {
                        Thread.Sleep(100);
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Log.addlog(StaticGlobal.UserID, "用户登录", 0);
                          
                            StaticGlobal.LoginTime =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (StaticGlobal.UserRole == "SUPER")
                            {
                                UserMessageBox.Show("提示", "您当前身份为:超级用户！");
                            }
                            else if(StaticGlobal.UserRole == "OPER")
                            {
                                UserMessageBox.Show("提示", "您当前身份为:操作员！");
                            }
                            else
                            {
                                UserMessageBox.Show("提示", "您当前身份为:游客！");
                            }
                            
                            if (StaticGlobal.firstloginflag == true)
                            {
                                DataBaseSettingWindow dbsettingwindow = new DataBaseSettingWindow();
                                dbsettingwindow.Show();
                            }
                            MainWindow mainwindow = new MainWindow();
                            mainwindow.Show();
                            this.Close();
                        }));
                        t.Abort();
                    }));
                    t.Start();
                }
            }
        }

        /*----------------------------------------------------------------
       //函数说明：关闭按键//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /*----------------------------------------------------------------
       //函数说明：可拖动//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }
    }
}