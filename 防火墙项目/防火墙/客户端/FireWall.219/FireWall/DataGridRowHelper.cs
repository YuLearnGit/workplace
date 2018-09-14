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
    public static class DataGridRowHelper
    {
        #region ShowRowIndex
        public static readonly DependencyProperty ShowRowIndexProperty = DependencyProperty.RegisterAttached("ShowRowIndex", typeof(bool), typeof(DataGridRowHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnShowRowIndexPropertyChanged)));
        private static void OnShowRowIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is DataGrid)
        {
            DataGrid _dataGrid = sender as DataGrid;
            _dataGrid.LoadingRow -= _dataGrid_LoadingRow;
            _dataGrid.UnloadingRow -= _dataGrid_UnloadingRow;
            bool newValue = (bool)e.NewValue;
            if (newValue)
            {
                _dataGrid.LoadingRow += _dataGrid_LoadingRow;
                _dataGrid.UnloadingRow += _dataGrid_UnloadingRow;
            }
        }
    }
    static void _dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
    {
        List<DataGridRow> rows = GetRowsProperty(sender as DataGrid);
        if (rows.Contains(e.Row))
            rows.Remove(e.Row);
        foreach (DataGridRow row in rows)
            row.Header = row.GetIndex() + 1;
    }
    static void _dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        List<DataGridRow> rows = GetRowsProperty(sender as DataGrid);
        if (!rows.Contains(e.Row))
            rows.Add(e.Row);
        foreach (DataGridRow row in rows)
            row.Header = row.GetIndex() + 1;
    }
    public static bool GetShowRowIndexProperty(DependencyObject obj)
    {
        return (bool)obj.GetValue(ShowRowIndexProperty);
    }
    public static void SetShowRowIndexProperty(DependencyObject obj, bool value)
    {
        obj.SetValue(ShowRowIndexProperty, value);
    }
    #endregion

    #region Rows
    public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached("Rows", typeof(List<DataGridRow>), typeof(DataGridRowHelper), new PropertyMetadata(new List<DataGridRow>()));

    private static List<DataGridRow> GetRowsProperty(DependencyObject obj)
    {
        return (List<DataGridRow>)obj.GetValue(RowsProperty);
    }
    #endregion
  }
}
