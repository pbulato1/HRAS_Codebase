using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Timers;

namespace Interface
{
    /// <summary>
    /// Interaction logic for ImportFile.xaml
    /// </summary>
    public partial class ImportFile : Window
    {
		ImportType type;
		System.Timers.Timer timer = new System.Timers.Timer();

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
			pbImport.Value = 0;
			lblStatus.Content = "Import Status: In Progress";
			ImportData.import(tfFilePath.Text, type, Session.getCurrentSession());
			timer.Interval = 250;
			timer.Elapsed += OnTimedEvent;
			timer.Enabled = true;
		}

		private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			pbImport.Dispatcher.Invoke(() =>
			{
				pbImport.Value = ImportData.getProgress();
				if (pbImport.Value == 100)
				{
					timer.Enabled = false;
					lblStatus.Content = "Import Status: Complete";
				}
			});
		}

		private void Button_Click_btnBrowse(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
				tfFilePath.Text = openFileDialog.FileName;
		}
	}
}
