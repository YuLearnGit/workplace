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
using System.Windows.Shapes;

namespace FireWall
{
    /// <summary>
    /// SetPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetPasswordWindow 
    {
        public SetPasswordWindow()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------
       //函数说明：取消按键//
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
                        update();
                    }
                }
            }
        }

        /*----------------------------------------------------------------
       //函数说明：更新指定用户密码的数据库数据//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void update()
        {
            string action;
            string CommandText = "update useraccount set Password ='" + passwordBox.Password + "' where UserName='" + textBox.Text + "'";
            try
            {
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                MySqlCommand comm = new MySqlCommand(CommandText, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Close();
                conn.Dispose();
                //MessageBox.Show("设置密码成功！");
                UserMessageBox.Show("提示", "修改用户" + textBox.Text + "的密码成功！");
                action = "修改用户" + textBox.Text + "密码";
                Log.addlog(StaticGlobal.UserID, action, 0);
                this.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("设置密码失败，请重试！");
                UserMessageBox.Show("提示", "修改密码失败，请重试！");
            }
        }
    }
}
