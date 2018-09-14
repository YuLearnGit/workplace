/*----------------------------------------------------------------
// Copyright (C) 
// 版权所有。
//
// 文件名：UserMessageBox
// 文件功能描述：消息框
//----------------------------------------------------------------*/
using System.Windows;

namespace FireWall
{
    /// <summary>
    /// UserMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UserMessageBox 
    {
        public UserMessageBox()
        {
            InitializeComponent();
        }

        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string Message
        {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>

        /*----------------------------------------------------------------
       //函数说明：根据不同的用户权限加载不同的权限选择框//
       //输入：string类型标题，string类型信息//
       //输出：无//
       //----------------------------------------------------------------*/
        public static bool? Show(string title, string msg)
        {
            var msgBox = new UserMessageBox();
            msgBox.Title = title;
            msgBox.Message = msg;
            if (title == "提示" || title == "错误")
            {
                msgBox.Tip.Visibility = Visibility.Visible;
                msgBox.Confirm.Visibility = Visibility.Collapsed;
            }
            else
            {
                msgBox.Tip.Visibility = Visibility.Collapsed;
                msgBox.Confirm.Visibility = Visibility.Visible;
            }
            msgBox.Focus();
            return msgBox.ShowDialog();
        }       

        /*----------------------------------------------------------------
       //函数说明：确认按键//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /*----------------------------------------------------------------
       //函数说明：取消按键//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
