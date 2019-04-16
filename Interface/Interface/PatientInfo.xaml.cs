using HRAS;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for PatientInfo.xaml
    /// </summary>
    public partial class PatientInfo : Window
    {
        public PatientInfo()
        {
            InitializeComponent();
            Loaded += Patient_Loaded;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Button_Click_Checkout(object sender, RoutedEventArgs e)
        {
            Billing billing = new Billing();
            billing.Show();
            this.Close();

        }

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();

        }
        private void Patient_Loaded(object sender, EventArgs e)
        {

        }


        private void IR_DataChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_BackMenu(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
