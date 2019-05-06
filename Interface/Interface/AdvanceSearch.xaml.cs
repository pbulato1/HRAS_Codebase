using System;
using Interface;
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
using System.Windows.Shapes;
using Middleware;
using HRAS;

namespace HRAS
{

    public partial class AdvanceSearch : Window
    {
        DataGrid grid;
        public AdvanceSearch(DataGrid previousPageDataGrid)
        {
            grid = previousPageDataGrid;
            InitializeComponent();
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            DataTable records = Middleware.MedicalRecord.searchAdvanceMedicalRecords(FirstName.Text, LastName.Text, SSN.Text, Room.Text);
            grid.ItemsSource = records.DefaultView;
            grid.AutoGenerateColumns = true;
            grid.CanUserAddRows = false;
            this.Close();
        }
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
        }

        private void Button_Click_BackMenu(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            menu.Show();
            this.Close();
        }

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show(this, "Do you want to exit?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }
    }
}
