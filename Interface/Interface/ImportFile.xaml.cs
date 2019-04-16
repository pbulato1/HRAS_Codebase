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
using Microsoft.Win32;
using Middleware;

namespace Interface
{
    /// <summary>
    /// Interaction logic for ImportFile.xaml
    /// </summary>
    public partial class ImportFile : Window
    {
		ImportType type;

        public ImportFile()
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
				Session.getCurrentSession().getCurrentUser().logout();
				MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }

        private void Button_Click_tbtnInventory(object sender, RoutedEventArgs e)
        {
			type = ImportType.INVENTORY;
        }

		private void Button_Click_tbtnMedicalRecords(object sender, RoutedEventArgs e)
		{
			type = ImportType.MEDICAL;
		}

		private void Button_Click_tbtnRooms(object sender, RoutedEventArgs e)
		{
			type = ImportType.ROOM;
		}

		private void Button_Click_btnImport(object sender, RoutedEventArgs e)
		{
			ImportData.import(tfFilePath.Text, type, Session.getCurrentSession());
		}

		private void Button_Click_btnBrowse(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
				tfFilePath.Text = openFileDialog.FileName;
		}
	}
}
