using DevExpress.Xpf.Charts;
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
    /// StatisticsControl.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticsControl : UserControl
    {
        string Statetext = null;
        DateTime QueryYearMonth;
        DataSet IPresult = new DataSet();
        DataSet Dateresult = new DataSet();
        public bool EnabledDateQueryFlag = false;

        public StatisticsControl()
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
            SelectionBinding();
        }

        //选项绑定
        public void SelectionBinding()
        {
            string[] state = { "ALL", "ACCEPT", "DROP" };
            StateName.ItemsSource = state;
            StateName.SelectedIndex = 0;

            QueryDate.EditValue = DateTime.Now.ToString("yyyy/MM");//默认时间

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

            //    string[] state = { "ALL", "ACCEPT", "DROP" };
            //StateName.ItemsSource = state;
            //StateName.SelectedIndex = 0;

            //QueryDate.EditValue = DateTime.Now.ToString("yyyy/MM");//默认时间

            //FwName.ItemsSource = new string[] { "00:10:F3:5F:F3:8D" };
            //FwName.SelectedIndex = 0;

            //if (FwName.Text != "")
            //{
            //    ProtectedIP.ItemsSource = new string[] { "172.16.10.123" };
            //    ProtectedIP.SelectedIndex = 0;
            //}
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
            if (QueryDate.EditValue != null)
            {
                if (FwName.Text != "" && ProtectedIP.Text != "")
                {
                    IPinitGridControl();
                    IPStatisSearch(Statetext);
                    //UnableToExportImage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    UserMessageBox.Show("提示", "请先进行设备扫描，然后再进行查询！");
                }
            }
            else
            {
                UserMessageBox.Show("提示", "时间不能为空！");
            }
        }

        public void IPinitGridControl()
        {
            StatisData.Columns.Clear();
            SpinEditSettings editSettings1 = new SpinEditSettings()
            {
                HorizontalContentAlignment = EditSettingsHorizontalAlignment.Center
            };
            StatisData.Columns.Add(new GridColumn() { FieldName = "发送数据IP", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            StatisData.Columns.Add(new GridColumn() { FieldName = "次数", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 100 });
            StatisData.Columns["发送数据IP"].EditSettings = editSettings1;
            StatisData.Columns["次数"].EditSettings = editSettings1;
        }

        private void IPStatisSearch(string Statetext)
        {
            IPresult.Clear();
            pie.Points.Clear();
            QueryYearMonth = Convert.ToDateTime(QueryDate.EditValue);
            string mainCommandText = " SELECT SRC as '发送数据IP',count(*) as '次数' FROM iptables_log" + QueryYearMonth.ToString("yyyyMM") +
                                     " WHERE fw_MAC = '" + FwName.Text + "' AND DST = '" + ProtectedIP.Text + "'" + Statetext +
                                     " GROUP BY 1 ORDER BY 1";

            MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
            conn.Open();
            MySqlDataAdapter s = new MySqlDataAdapter(mainCommandText, conn);
            s.Fill(IPresult);
            conn.Close();
            for (int i = 0; i < IPresult.Tables[0].Rows.Count; i++)
            {
                pie.Points.Add(new SeriesPoint(IPresult.Tables[0].Rows[i]["发送数据IP"].ToString(), Convert.ToInt64(IPresult.Tables[0].Rows[i]["次数"])));
            }
            StatisData.ItemsSource = IPresult.Tables[0];
            IPchartControl.Visibility = Visibility.Visible;
            DatechartControl.Visibility = Visibility.Collapsed;
            Returnbutton.Visibility = Visibility.Collapsed;
            ShowBar.Visibility = Visibility.Collapsed;
            ShowLine.Visibility = Visibility.Collapsed;
            EnabledDateQueryFlag = true;
        }

        private void RowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            if (EnabledDateQueryFlag == true)
            {
                DateinitGridControl();
                DateStatisSearch((e.Source.SelectedRows[0] as DataRowView).Row["发送数据IP"].ToString());
            }
        }

        private void DateinitGridControl()
        {
            StatisData.Columns.Clear();
            SpinEditSettings editSettings1 = new SpinEditSettings()
            {
                HorizontalContentAlignment = EditSettingsHorizontalAlignment.Center
            };
            StatisData.Columns.Add(new GridColumn() { FieldName = "日期", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 130 });
            StatisData.Columns.Add(new GridColumn() { FieldName = "次数", HorizontalHeaderContentAlignment = HorizontalAlignment.Center, Width = 100 });
            StatisData.Columns["日期"].EditSettings = editSettings1;
            StatisData.Columns["次数"].EditSettings = editSettings1;
        }

        private void DateStatisSearch(string SRC)
        {
            Dateresult.Clear();
            Dictionary<string, long> DateStatisPoint = new Dictionary<string, long>();
            string mainCommandText = " SELECT EXTRACT(DAY FROM time) as '日期',count(*) as '次数' FROM iptables_log" + Convert.ToDateTime(QueryDate.EditValue).ToString("yyyyMM") +
                                     " WHERE fw_MAC = '" + FwName.Text + "' AND DST = '" + ProtectedIP.Text + "' AND SRC = '" + SRC + "'" + Statetext +
                                     " GROUP BY 1 ORDER BY 1";
            MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
            conn.Open();
            MySqlDataAdapter s = new MySqlDataAdapter(mainCommandText, conn);
            s.Fill(Dateresult);
            conn.Close();
            int days = Convert.ToInt16(QueryYearMonth.AddMonths(1).AddDays(-QueryYearMonth.AddMonths(1).Day).ToString("dd"));
            for (int i = 1; i <= days; i++)
            {
                long cnt = 0;
                for (int j = 0; j < Dateresult.Tables[0].Rows.Count; j++)
                {
                    if (Convert.ToInt16(Dateresult.Tables[0].Rows[j]["日期"].ToString()) == i)
                    {
                        cnt = Convert.ToInt64(Dateresult.Tables[0].Rows[j]["次数"]);
                        break;
                    }
                }
                DateStatisPoint.Add(i.ToString(), cnt);
            }
            StatisData.ItemsSource = Dateresult.Tables[0];
            IPchartControl.Visibility = Visibility.Collapsed;
            DatechartControl.Visibility = Visibility.Visible;
            Returnbutton.Visibility = Visibility.Visible;
            ShowBar.Visibility = Visibility.Visible;
            ShowLine.Visibility = Visibility.Visible;
            barSeries.DataSource = DateStatisPoint;
            line.DataSource = DateStatisPoint;
            EnabledDateQueryFlag = false;
        }
        private void Returnbutton_Click(object sender, RoutedEventArgs e)
        {
            IPinitGridControl();
            StatisData.ItemsSource = IPresult.Tables[0];
            IPchartControl.Visibility = Visibility.Visible;
            DatechartControl.Visibility = Visibility.Collapsed;
            Returnbutton.Visibility = Visibility.Collapsed;
            ShowBar.Visibility = Visibility.Collapsed;
            ShowLine.Visibility = Visibility.Collapsed;
            EnabledDateQueryFlag = true;
            //for (int j =0;j<10;j++)
            //{
            //    for (int i = 1; i < 31; i++)
            //    {
            //        Random a = new Random();
            //        Random b = new Random();
            //        string mainCommandText = "INSERT INTO `iptables_log201703` VALUES ('2017-03-" + i + " 14:27:20', 'hehe-desktop', '00:10:F3:5F:F3:8D', 'br0 ', 'eth0 ', 'br0 ', 'eth1 ', '24:fd:52:43:b0:c5', '6c:f0:49:07:9c:56', '172.16.10." + new Random().Next(195, 200) + "', '172.16.10.123 ', '52 ', '0x00 ', '0x00 ', '64 ', '598 ', 'TCP ', '57078 ', '502 ', 'DROP', 'modbus default log rule');";
            //        MySqlConnection conn = new MySqlConnection(StaticGlobal.ConnectionString);
            //        conn.Open();
            //        MySqlCommand cmd = new MySqlCommand(mainCommandText, conn);
            //        cmd.ExecuteNonQuery();
            //        conn.Close();
            //    }
            //}

            //MessageBox.Show("已完成");
        }

        //*----------------------------------------------------------------
        //函数说明：显示柱状图
        //输入：无
        //输出：无
        //----------------------------------------------------------------*//
        private void ShowBar_Checked(object sender, RoutedEventArgs e)
        {
            barSeries.Visible = true;
        }
        private void ShowBar_Unchecked(object sender, RoutedEventArgs e)
        {
            barSeries.Visible = false;
        }

        //*----------------------------------------------------------------
        //函数说明：显示曲线图
        //输入：无
        //输出：无
        //----------------------------------------------------------------*//
        private void ShowLine_Checked(object sender, RoutedEventArgs e)
        {
            line.Visible = true;
        }
        private void ShowLine_Unchecked(object sender, RoutedEventArgs e)
        {
            line.Visible = false;
        }

        /*----------------------------------------------------------------
       //函数说明：点击下拉框会清空gridcontrol//
       //输入：无//
       //输出：无//
       //----------------------------------------------------------------*/
        private void clear(object sender, RoutedEventArgs e)
        {
            IPinitGridControl();
            IPchartControl.Visibility = Visibility.Collapsed;
            DatechartControl.Visibility = Visibility.Visible;
            Returnbutton.Visibility = Visibility.Collapsed;
            ShowBar.Visibility = Visibility.Collapsed;
            ShowLine.Visibility = Visibility.Collapsed;
            EnabledDateQueryFlag = false;
            StatisData.ItemsSource = null;
            barSeries.DataSource = null;
            line.DataSource = null;
            pie.Points.Clear();
            //UnableToExportImage.Visibility = Visibility.Visible;
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
            //saveDialog.FileName = Convert.ToDateTime(StartDate.EditValue).ToString("yyyy年MM月dd日HH时mm分ss秒") + "—" + Convert.ToDateTime(EndDate.EditValue).ToString("yyyy年MM月dd日HH时mm分ss秒") + HostName.SelectedItem.ToString() + "日志";
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
