using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WPFdatagrid.SQLiteHandler;

namespace WPFdatagrid
{
    /// <summary>
    /// Logika interakcji dla klasy MyDatagrid.xaml
    /// </summary>
    public partial class MyDatagrid : Window
    {
        SQLHandler sqlHandler;
        List<MyUser> users { get; set; }
        public MyDatagrid(SQLHandler handler)
        {
            InitializeComponent();
            sqlHandler = handler;
            users = sqlHandler.databaseRepresentation;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            myData.ItemsSource = users;
            //sqlHandler.InsertData(sqlHandler.databaseRepresentation);
        }

        void myData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = ((Binding)column.Binding).Path.Path;
                    if (bindingPath == "Id")
                    {
                       
                        int rowIndex = e.Row.GetIndex();
                        var el = (TextBox)e.EditingElement;
                        // rowIndex has the row index
                        // bindingPath has the column's binding
                        // el.Text has the new, user-entered value

                        try
                        {
                            foreach (var user in users)
                            {
                                if (user.Id == Int32.Parse(el.Text))
                                {
                                    el.Text = users[rowIndex].Id.ToString();
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.Message);
                        }
                        finally
                        {
                            Trace.WriteLine(users[0]);
                        }
                    }
                }
            }
            if (!e.Row.IsEditing)
            {
                this.myData.Items.Refresh();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            sqlHandler.TruncateData();
            sqlHandler.InsertData(users);
            users = sqlHandler.ReadData();
            myData.Items.Refresh();
        }
    }
}
