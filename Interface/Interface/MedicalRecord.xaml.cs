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

namespace Interface
{
    /// <summary>
    /// Interaction logic for MedicalRecord.xaml
    /// </summary>
    public partial class MedicalRecord : Window
    {
        public MedicalRecord()
        {
            InitializeComponent();
            Loaded += MedicalRecord_Loaded;
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
				Session.getCurrentSession().getCurrentUser().logout();
				MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }


        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {

        }

      

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void MedicalRecord_Loaded(object sender, EventArgs e)
        {
            //DataTable medical = MedicalRecord.getMedical();
            //M1. = medical.DefaultView;
            M1.AutoGenerateColumns = true;
            M1.CanUserAddRows = false;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //DataTable medical = MedicalRecord.searchMedical(Account.Text);
            //M1.ItemsSource = medical.DefaultView;
            M1.AutoGenerateColumns = true;
            M1.CanUserAddRows = false;
        }

		private void IR_DataChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_AdvanceSearch(object sender, RoutedEventArgs e)
        {
            AdvanceSearch advanceSearch = new AdvanceSearch();
            advanceSearch.Show();
            this.Close();
        }
    }
}
