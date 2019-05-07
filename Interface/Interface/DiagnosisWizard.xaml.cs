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
    /// Interaction logic for DiagnosisWizard.xaml
    /// </summary>
    public partial class DiagnosisWizard : Window
    {
        private bool answer = false;
        public DiagnosisWizard()
        {
            InitializeComponent();
            DG1.IsReadOnly = true;
            Middleware.DiagnosisWizardMid wizard = new Middleware.DiagnosisWizardMid();
            wizard.RunDiagnosisWizard();
            question.Content = wizard.askQuestion(wizard.getSymptomName(wizard.getSymptomList()));
        }

        private void Button_Click_Restart(object sender, RoutedEventArgs e)
        {
            MainMenu newWindow = new MainMenu();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            // check for yes/no buttons then put that data in class varibal answer
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

        private void DiagnosisPercent_Change(object sender, SelectionChangedEventArgs e)
        {
            // need to call getDiagnosisPercentages
        }

        private void NoButton_Checked(object sender, RoutedEventArgs e)
        {
            answer = false;
        }

        private void YesButton_Checked(object sender, RoutedEventArgs e)
        {
            answer = false;
        }
    }
}
