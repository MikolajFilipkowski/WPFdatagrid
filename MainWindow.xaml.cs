using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Dziala");
            sqlHandler.CreateConnection();
            sqlHandler.CreateTable();
            //sqlHandler.InsertData("Stefan", 18);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sqlHandler.ReadData();
        }
    }
}
