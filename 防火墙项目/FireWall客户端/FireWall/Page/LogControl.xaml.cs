using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// ReportControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        string Statetext = null; 
        public LogControl()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------
       //函数说明：加载窗口//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitGridControl();
            SelectionBinding();
        }

        public void SelectionBinding()
        {
            string[] state = { "ALL", "ACCEPT", "DROP" };
            StateName.ItemsSource = state;
            StateName.SelectedIndex = 0;

            StartDate.EditValue = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
            EndDate.EditValue = DateTime.Now.ToString();

            FwName.ItemsSource = (from devices in StaticGlobal.fwdev_list
                                  select devices.getDev_MAC()).ToList<string>().ToArray();
            FwName.SelectedIndex = 0;

            if (FwName.Text != "")
            {
                ProtectedIP.ItemsSource = (from devices in StaticGlobal.fwdev_list
                                           where devices.getDev_MAC() == FwName.Text
                                           select devices.getProtecDevIP_list()).ToList()[0].ToArray();
                ProtectedIP.SelectedIndex = 0;
            }

            //string[] state = { "ALL", "ACCEPT", "DROP" };
            //StateName.ItemsSource = state;
            //StateName.SelectedIndex = 0;

            //StartDate.EditValue = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
            //EndDate.EditValue = DateTime.Now.ToString();

            //FwName.ItemsSource = new string[] { "00:10:F3:5F:F3:8D" };
            //FwName.SelectedIndex = 0;

            //if (FwName.Text != "")
            //{
            //    ProtectedIP.ItemsSource = new string[] { "172.16.10.123" };
            //    ProtectedIP.SelectedIndex = 0;
            //}
        }

        public void InitGridControl()
        {
            LogData.Columns.Clear();
            SpinEditSettings editSettings1 = new SpinEditSettings()
            {
                DisplayFormat = "yyyy-MM-dd HH:mm:ss",
                HorizontalContentAlignment = EditSettingsHorizontalAlignment.Center
            };
            SpinEditSettings editSettings2 = new SpinEditSettings()
            {
                HorizontalContentAlignment = EditSettingsHorizontalAlignment.Center
            };
            LogData.Columns.Add(new GridColumn() { FieldName = "时间", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "防火墙MAC", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 180 });
            LogData.Columns.Add(new GridColumn() { FieldName = "源IP地址", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "目标IP地址", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "承载数据的总长度", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "IP包头内的服务类型", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "服务类型的优先级", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "IP数据包标示", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "传输层协议类型", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "源端口号", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "目标端口号", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns.Add(new GridColumn() { FieldName = "处理状态", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            LogData.Columns["时间"].EditSettings = editSettings1;
            LogData.Columns["防火墙MAC"].EditSettings = editSettings2;
            LogData.Columns["源IP地址"].EditSettings = editSettings2;
            LogData.Columns["目标IP地址"].EditSettings = editSettings2;
            LogData.Columns["承载数据的总长度"].EditSettings = editSettings2;
            LogData.Columns["IP包头内的服务类型"].EditSettings = editSettings2;
            LogData.Columns["服务类型的优先级"].EditSettings = editSettings2;
            LogData.Columns["IP数据包标示"].EditSettings = editSettings2;
            LogData.Columns["传输层协议类型"].EditSettings = editSettings2;
            LogData.Columns["源端口号"].EditSettings = editSettings2;
            LogData.Columns["目标端口号"].EditSettings = editSettings2;
            LogData.Columns["处理状态"].EditSettings = editSettings2;
        }

        private void Querybutton_Click(object sender, RoutedEventArgs e)
        {
            if (StateName.SelectedItem.ToString() == "ALL")
            {
                Statetext = "";
            }
            else
            {
                Statetext = "AND handle_result = '" + StateName.SelectedItem.ToString() + "'";
            }
            DateTime startDate = Convert.ToDateTime(StartDate.EditValue);
            DateTime endDate = Convert.ToDateTime(EndDate.EditValue);
            TimeSpan monthSpan = endDate.Subtract(startDate);
            //时间不能为空且不超过3个月
            if ((StartDate.EditValue != null) && (EndDate.EditValue != null))
            {
                int startyear = Convert.ToInt16(Convert.ToDateTime(StartDate.EditValue).ToString("yyyy"));
                int endyear = Convert.ToInt16(Convert.ToDateTime(EndDate.EditValue).ToString("yyyy"));
                int startmonth = Convert.ToInt16(Convert.ToDateTime(StartDate.EditValue).ToString("MM"));
                int endmonth = Convert.ToInt16(Convert.ToDateTime(EndDate.EditValue).ToString("MM"));
                if (monthSpan.TotalDays > 0)
                {
                    if ((endyear - startyear) * 12 + endmonth - startmonth <= 2) //判断是否超过3个月
                    {
                        string[] selectedmonth = new string[(endyear - startyear) * 12 + endmonth - startmonth + 1];
                        if (startyear == endyear) //判断选择时间是否跨年
                        {
                            for (int i = 0; i <= endmonth - startmonth; i++)
                            {
                                selectedmonth[i] = startyear.ToString() + (i + startmonth).ToString().PadLeft(2, '0');
                            }
                        }
                        else
                        {
                            for (int i = startmonth; i <= 12; i++)
                            {
                                selectedmonth[i - startmonth] = startyear.ToString() + i.ToString().PadLeft(2, '0');
                            }
                            for (int i = 1; i <= endmonth; i++)
                            {
                                selectedmonth[12 - startmonth + i] = endyear.ToString() + i.ToString().PadLeft(2, '0');
                            }
                        }
                        InitGridControl();

                        if (FwName.Text != "" && ProtectedIP.Text != "")
                        {
                            InitGridControl();
                            Search(selectedmonth);
                            //UnableToExportImage.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        UserMessageBox.Show("提示", "查询时间不能超过三个月！");
                    }
                }
                else
                {
                    //MessageBox.Show("时间不符合规则!");
                    UserMessageBox.Show("提示", "时间不符合规则！");
                }
            }
            else
            {
                //MessageBox.Show("时间不能为空!");
                UserMessageBox.Show("提示", "时间不能为空！");
            }
        }

        private void Search(string[] month)
        {
            int startyear = Convert.ToInt16(Convert.ToDateTime(StartDate.EditValue).ToString("yyyy"));
            string LogTableNamesql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='firewallclient' and TABLE_NAME like '%"+"iptables_log"+ startyear + "%';";
            List<string> LogTableName = new List<string>();
            string mainCommandText = "select time as'时间',host_name as'主机名称',fw_MAC as '防火墙MAC',SRC as'源IP地址',DST as'目标IP地址'," +
                      "LEN as'承载数据的总长度',TOS as'IP包头内的服务类型',PREC as'服务类型的优先级',ID as'IP数据包标示'," +
                      "PROTO as'传输层协议类型',SPT as'源端口号', DPT as'目标端口号',handle_result as '处理状态' FROM iptables_log" + month[0] +
                      " WHERE fw_MAC = '" + FwName.Text + "' AND DST = '" + ProtectedIP.Text + "'" + Statetext +
                      "AND time BETWEEN '" + StartDate.EditValue + "' AND '" + EndDate.EditValue + "';";
            for (int i = 1; i < month.Count(); i++)
            {
                mainCommandText += "select time as'时间',host_name as'主机名称',fw_MAC as '防火墙MAC',SRC as'源IP地址',DST as'目标IP地址'," +
                      "LEN as'承载数据的总长度',TOS as'IP包头内的服务类型',PREC as'服务类型的优先级',ID as'IP数据包标示'," +
                      "PROTO as'传输层协议类型',SPT as'源端口号', DPT as'目标端口号',handle_result as '处理状态' FROM iptables_log" + month[i] +
                      " WHERE fw_MAC = '" + FwName.Text + "' AND DST = '" + ProtectedIP.Text + "'" + Statetext +
                      "AND time BETWEEN '" + StartDate.EditValue + "' AND '" + EndDate.EditValue + "';";
            }
          
            DataSet result = new DataSet();
            MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
            conn.Open();
                       
            MySqlCommand cm = new MySqlCommand(LogTableNamesql, conn);
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                LogTableName.Add(dr[0].ToString());
            }
            dr.Close();
            MySqlDataAdapter s = new MySqlDataAdapter(mainCommandText, conn);
            s.Fill(result);
            conn.Close();
            LogData.ItemsSource = result.Tables[0];       

        }

        /*----------------------------------------------------------------
       //函数说明：点击下拉框会清空gridcontrol//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void clear(object sender, RoutedEventArgs e)
        {
            LogData.ItemsSource = null;
            InitGridControl();
        }

        //*----------------------------------------------------------------
        //函数说明：导出数据表按钮事件，可导出xls，xlsx，rtf，pdf，scv，txt，html格式
        //输入：无
        //输出：无
        //----------------------------------------------------------------*//
        private void Exportbutton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel (2003) (*.xls)|*.xls|Excel (2010) (*.xlsx)|*.xlsx|Rtf文件 (*.rtf)|*.rtf|Pdf文件 (*.pdf)|*.pdf|Csv文件 (*.csv)|*.csv|文本文件 (*.txt)|*.txt|网页 (*.html)|*.html";
            saveDialog.FileName = Convert.ToDateTime(StartDate.EditValue).ToString("yyyy年MM月dd日HH时mm分ss秒") + "—" + Convert.ToDateTime(EndDate.EditValue).ToString("yyyy年MM月dd日HH时mm分ss秒") +"被保护设备"+ ProtectedIP.Text + "的日志";
            if (saveDialog.ShowDialog() != false)
            {
                string exportFilePath = saveDialog.FileName;
                string fileExtenstion = new FileInfo(exportFilePath).Extension;
                switch (fileExtenstion)
                {
                    case ".xls":
                        new PrintableControlLink(view).ExportToXls(exportFilePath);
                        break;
                    case ".xlsx":
                        new PrintableControlLink(view).ExportToXlsx(exportFilePath);
                        break;
                    case ".rtf":
                        view.ExportToRtf(exportFilePath);
                        break;
                    case ".pdf":
                        view.ExportToPdf(exportFilePath);
                        break;
                    case ".csv":
                        view.ExportToCsv(exportFilePath);
                        break;
                    case ".txt":
                        view.ExportToText(exportFilePath);
                        break;
                    case ".html":
                        view.ExportToHtml(exportFilePath);
                        break;
                    default:
                        break;
                }

                if (File.Exists(exportFilePath))
                {
                    try
                    {
                        if (UserMessageBox.Show("导出", "文件已成功导出，是否打开文件?") == true)
                        {
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                    }
                    catch
                    {
                        String msg = "无法打开文件!" + Environment.NewLine + Environment.NewLine + "路径:" + exportFilePath;
                        UserMessageBox.Show("错误", msg);
                    }
                }
                else
                {
                    String msg = "无法保存文件!" + Environment.NewLine + Environment.NewLine + "路径:" + exportFilePath;
                    UserMessageBox.Show("错误", msg);
                }
            }
        }
    }
}
