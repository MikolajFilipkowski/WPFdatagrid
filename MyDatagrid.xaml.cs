using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<MyUser> users;
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
    }
}
