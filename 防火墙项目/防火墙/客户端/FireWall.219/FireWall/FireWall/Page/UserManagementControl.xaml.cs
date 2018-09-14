using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireWall
{
    /// <summary>
    /// UserManagementControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementControl : UserControl
    {
        public UserManagementControl()
        {
            InitializeComponent();
        }

        public void InitGridControl()
        {
            UserData.Columns.Clear();
            SpinEditSettings editSettings1 = new SpinEditSettings()
            {
                HorizontalContentAlignment = EditSettingsHorizontalAlignment.Center
            };
            UserData.Columns.Add(new GridColumn() { FieldName = "用户ID", HorizontalHeaderContentAlignment = HorizontalAlignment.Center });
            UserData.Columns.Add(new GridColumn() { FieldName = "用户名", HorizontalHeaderContentAlignment = HorizontalAlignment.Center });
            UserData.Columns.Add(new GridColumn() { FieldName = "用户权限", HorizontalHeaderContentAlignment = HorizontalAlignment.Center });
            UserData.Columns["用户ID"].EditSettings = editSettings1;
            UserData.Columns["用户名"].EditSettings = editSettings1;
            UserData.Columns["用户权限"].EditSettings = editSettings1;
        }

        /*----------------------------------------------------------------
        //函数说明：显示登录用户权限可见的用户//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        public void InitDateGrid()
        {
            DataSet result = new DataSet();
            string CommandText = "select UserID '用户ID' ,UserName '用户名', Purview '用户权限' from useraccount where UserName ='" + StaticGlobal.UserName + "'";
            if (StaticGlobal.UserRole.ToUpper() == "OPER")
            {
                CommandText += " or Purview = 'GUEST'";
            }
            if (StaticGlobal.UserRole.ToUpper() == "SUPER")
            {
                CommandText += " or Purview = 'GUEST' or Purview = 'OPER'";
            }
            CommandText += " order by 1;";
            try
            {
                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                conn.Open();
                MySqlDataAdapter s = new MySqlDataAdapter(CommandText, conn);
                s.Fill(result);
                conn.Close();
                for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    switch (result.Tables[0].Rows[i][2].ToString().ToUpper())
                    {
                        case "SUPER":
                            result.Tables[0].Rows[i][2] = "超级用户";
                            break;
                        case "OPER":
                            result.Tables[0].Rows[i][2] = "操作员";
                            break;
                        case "GUEST":
                            result.Tables[0].Rows[i][2] = "游客";
                            break;
                        default: break;
                    }
                }
                InitGridControl();
                UserData.ItemsSource = result.Tables[0];

            }
            catch (Exception)
            {
                //MessageBox.Show("服务器连接异常!");
                UserMessageBox.Show("提示", "服务器连接异常！");
            }
        }

        /*----------------------------------------------------------------
       //函数说明：添加用户//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaticGlobal.UserRole.ToUpper() == "SUPER" || StaticGlobal.UserRole.ToUpper() == "OPER")
            {
                AddUser adduser = new AddUser();
                adduser.ShowDialog();
                InitDateGrid();
            }
            else
            {
                UserMessageBox.Show("提示", "您的权限不足，无法操作！");
            }
        }

        /*----------------------------------------------------------------
       //函数说明：删除用户//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaticGlobal.UserRole.ToUpper() == "SUPER" || StaticGlobal.UserRole.ToUpper() == "OPER")
            {
                string action;
                DataRowView SelectedElement = (DataRowView)UserData.SelectedItem;
                if (SelectedElement != null)
                {
                    string username = SelectedElement.Row[1].ToString();
                    if (username != StaticGlobal.UserName)
                    {
                        if (UserMessageBox.Show("确认", "确定要删除用户" + username + "吗?") == true)
                        {
                            string CommandText = "delete from useraccount where UserName = '" + username + "';";
                            try
                            {
                                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                                conn.Open();
                                MySqlCommand comm = new MySqlCommand(CommandText, conn);
                                comm.ExecuteNonQuery();
                                comm.Dispose();
                                conn.Close();
                                conn.Dispose();
                                //MessageBox.Show("成功删除用户！");
                                UserMessageBox.Show("提示", "成功删除用户！");
                                action = "删除用户" + username;
                                Log.addlog(StaticGlobal.UserID, action, 0);
                                InitDateGrid();
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show("删除用户失败，请重试！");
                                UserMessageBox.Show("提示", "删除用户失败，请重试！");
                            }
                        }
                    }
                   else
                    {
                        UserMessageBox.Show("提示", "你不能删除你自己！");
                    } 
                }
                else
                {
                    UserMessageBox.Show("提示", "请选择需要删除的用户！");
                }
            }
            else
            {
                UserMessageBox.Show("提示", "您的权限不足，无法操作！");
            }
        }

        /*----------------------------------------------------------------
       //函数说明：修改选中的用户密码//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView SelectedElement = (DataRowView)UserData.SelectedItem;
            if (SelectedElement != null)
            {
                string username = SelectedElement.Row[1].ToString();
                SetPasswordWindow ss = new SetPasswordWindow();
                ss.textBox.Text = username;
                ss.textBox.IsReadOnly = true;
                ss.ShowDialog();
            }
            else
            {
                UserMessageBox.Show("提示", "请选择需要修改密码的用户！");
            }
        }

        /*----------------------------------------------------------------
        //函数说明：修改选中用户的权限//
        //输入：无//
        //输出：无//
        //----------------------------------------------------------------*/
        private void ChangeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaticGlobal.UserRole.ToUpper() == "SUPER")
            {
                if (UserData.SelectedItem != null)
                {
                    DataRowView SelectedElement = (DataRowView)UserData.SelectedItem;
                    string username = SelectedElement.Row[1].ToString();
                    if (username != "admin")
                    {
                        if (UserMessageBox.Show("确认", "确定要改变用户 " + username + " 的权限？") == true)
                        {
                            string CommandText, action;
                            if (SelectedElement.Row[2].ToString() == "操作员")
                            {
                                CommandText = "update useraccount set Purview = 'GUEST' where UserName='" + username + "'";
                            }
                            else
                            {
                                CommandText = "update useraccount set Purview = 'OPER' where UserName='" + username + "'";
                            }
                            try
                            {
                                MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
                                conn.Open();
                                MySqlCommand comm = new MySqlCommand(CommandText, conn);
                                comm.ExecuteNonQuery();
                                comm.Dispose();
                                conn.Close();
                                conn.Dispose();
                                //MessageBox.Show("更改权限成功！");
                                UserMessageBox.Show("提示", "更改用户" + username + "权限成功！");
                                action = "修改用户" + username + "权限";
                                Log.addlog(StaticGlobal.UserID, action, 0);
                                InitDateGrid();
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show("更改权限失败，请重试！");
                                UserMessageBox.Show("提示", "更改权限失败，请重试！");
                            }
                        }
                    }
                    else
                    {
                            UserMessageBox.Show("提示", "超级用户权限不能修改！");
                    }
                }
                else
                {
                    UserMessageBox.Show("提示", "请选择需要更改权限的用户！");
                }
            }
            else
            {
                UserMessageBox.Show("提示", "您权限不足,无法修改用户权限！");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitDateGrid();
        }
    }
}
