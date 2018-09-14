using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace FireWall
{
    /// <summary>
    /// AddUser.xaml 的交互逻辑
    /// </summary>
    public partial class AddUser 
    {
        public AddUser()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------
       //函数说明：根据不同的用户权限加载不同的权限选择框//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] SUPER = new string[] { "操作员", "游客" };
            string[] OPER = new string[] { "游客" };
            if (StaticGlobal.UserRole.ToUpper() == "SUPER")
            {
                comboBox.ItemsSource = SUPER;
                comboBox.SelectedIndex = 0;
            }
            else
            {
                comboBox.ItemsSource = OPER;
                comboBox.SelectedIndex = 0;
            }
        }

        /*----------------------------------------------------------------
       //函数说明：关闭页面//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /*----------------------------------------------------------------
       //函数说明：确认按键//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                //MessageBox.Show("用户名不能为空");
                UserMessageBox.Show("提示", "用户名不能为空！");
            }
            else
            {
                if (passwordBox.Password == "")
                {
                    //MessageBox.Show("密码不能为空！");
                    UserMessageBox.Show("提示", "密码不能为空！");
                }
                else
                {
                    if (passwordBox1.Password == "")
                    {
                        //MessageBox.Show("请确认密码！");
                        UserMessageBox.Show("提示", "请确认密码！");
                    }
                    else
                    {
                        if (passwordBox.Password != passwordBox1.Password)
                        {
                            //MessageBox.Show("两次密码输入不一致！请重新输入！");
                            UserMessageBox.Show("提示", "两次密码输入不一致！请重新输入！");
                        }
                        else
                        {
                            Search();
                        }
                    }
                }
            }
        }

        /*----------------------------------------------------------------
       //函数说明：查询函数，检测添加用户的合理性//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void Search()
        {
            string CommandText = "select * from useraccount where UserName='" + textBox.Text + "'";
            try
            {
                DataSet result = new DataSet();
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                MySqlDataAdapter s = new MySqlDataAdapter(CommandText, conn);
                s.Fill(result);
                if (result.Tables[0].Rows.Count > 0)
                {
                    //MessageBox.Show("存在同名用户，请更改后重试！");
                    UserMessageBox.Show("提示", "存在同名用户，请更改后重试！");
                }
                else
                {
                    Add();
                }
                conn.Close();
                conn.Dispose();
            }
            catch (Exception)
            {
                //MessageBox.Show("数据库连接出错！");
                UserMessageBox.Show("提示", "数据库连接出错！");
            }
        }

        /*----------------------------------------------------------------
        //函数说明：增加用户函数//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        private void Add()
        {
            string action;
            string userRole;
            switch (comboBox.Text)
            {
                case "操作员":
                    userRole = "OPER";
                    break;
                case "游客":
                    userRole = "GUEST";
                    break;
                default:
                    userRole = "OPER";
                    break;
            }
            string CommandText = "insert into useraccount (UserName,Password,Purview) values('" + textBox.Text + "','" + passwordBox.Password + "','" + userRole + "');";
            try
            {
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                MySqlCommand comm = new MySqlCommand(CommandText, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Close();
                conn.Dispose();
                //MessageBox.Show("添加用户成功！");
                UserMessageBox.Show("提示", "添加用户" + textBox.Text + "成功！");
                action = "添加用户" + textBox.Text;
                Log.addlog(StaticGlobal.UserID, action, 0);
                this.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("添加用户失败，请重试！");
                UserMessageBox.Show("提示", "添加用户失败，请重试！");
            }
        }
    }
}
