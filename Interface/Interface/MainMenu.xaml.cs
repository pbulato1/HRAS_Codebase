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
using Middleware;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            
			if (Session.getCurrentSession().getCurrentUser().getPrivilegeLevel() != (int)PrivilegeLevels.A)
			{
				ImportFiles.IsEnabled = false;
				ImportFiles.Visibility = Visibility.Hidden;
                AccountInfo.IsEnabled = false;
                AccountInfo.Visibility = Visibility.Hidden;
			}
		}

        private void Button_Click_InvertoryRecords(object sender, RoutedEventArgs e)
        {
            InventoryRecord inventory = new InventoryRecord();
            inventory.Show();
            this.Close();
        }
        private void Button_Click_CheckIn(object sender, RoutedEventArgs e)
        {
            CheckIn checkIn = new CheckIn();
            checkIn.Show();
            this.Close();
        }
        private void Button_Click_DiagnosisWizard(object sender, RoutedEventArgs e)
        {
            DiagnosisWizard dw = new DiagnosisWizard();
            dw.Show();
            this.Close();
        }
        private void Button_Click_MedicalRecords(object sender, RoutedEventArgs e)
        {
            MedicalRecord medicalRecord = new MedicalRecord();
            medicalRecord.Show();
            this.Close();
        }
        private void Button_Click_ImportFile(object sender, RoutedEventArgs e)
        {
            ImportFile importFile = new ImportFile();
            importFile.Show();
            this.Close();
        }
        private void Button_Click_AccountInformation(object sender, RoutedEventArgs e)
        {
            AccountInformation accountInformation = new AccountInformation();
            accountInformation.Show();
            this.Close();
        }
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show(this, "Do you want to exit?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
				Session.getCurrentSession().getCurrentUser().logout();
				MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }
    }
}
