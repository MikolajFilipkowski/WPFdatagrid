using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WPFdatagrid.SQLiteHandler;


namespace WPFdatagrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLHandler sqlHandler = new SQLHandler();
        private static readonly Regex _regex = new Regex("[0-9]");

        public MainWindow()
        {
            InitializeComponent();
            sqlHandler.CreateConnection();
            sqlHandler.CreateTable();
            //sqlHandler.InsertData("Stefan", 18);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (reczneId.IsChecked == true) 
                sqlHandler.InsertData(Int32.Parse(idMoje.Text), imie.Text, Int32.Parse(wiek.Text));
            else 
                sqlHandler.InsertData(imie.Text, Int32.Parse(wiek.Text));
            
            sqlHandler.ReadData();
        }

        private void onlyNumbers (object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void noPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!_regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void openDatagrid(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<MyDatagrid>().FirstOrDefault() == null)
            {
                MyDatagrid myDatagrid = new MyDatagrid(sqlHandler);
                myDatagrid.Show();
            }
        }
    }
}
