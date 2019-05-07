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
    /// Interaction logic for CheckIn.xaml
    /// </summary>
    public partial class CheckIn : Window
    {
        int SSNStandardLength = 9;
        public CheckIn()
        {
            InitializeComponent();

        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            bool patientExist = Patient.patientExists(SSN.Text);

            if (FirstName.Text.Equals("") || LastName.Text.Equals("") || SSN.Text.Equals("") || Birthdate.Text.Equals("") || Address.Text.Equals("") || City.Text.Equals("") || State.Text.Equals("") || Zip.Text.Equals(""))
                MessageBox.Show(this, "You have to fill all required fields", "Fill required fields", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            else
            {
                MessageBoxResult result = MessageBox.Show(this, "Do you want to submit?", "Submit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (patientExist)
                    {
                        if (!Patient.checkInExistedPatient(SSN.Text))
                        {
                            MessageBox.Show(this, "An error occurred while adding to the database, make sure that the data format " +
                                "is correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    else
                    {
                        if (SSN.Text.Length != SSNStandardLength)
                        {
                            MessageBox.Show(this, "The Patient must be of length " + SSNStandardLength, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        else
                        {
                            try
                            {
                                if (!Patient.checkInPatient(FirstName.Text, LastName.Text, MiddleInitial.Text, SSN.Text, Birthdate.Text, Gender.Text, Address.Text, City.Text, State.Text, Zip.Text))
                                {
                                    MessageBox.Show(this, "An error occurred while adding to the database, make sure that the data format " +
                                        "is correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex == InventoryItem.invalidInput)
                                {
                                    MessageBox.Show(this, "The input format you entered was invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text.Equals("") && MiddleInitial.Text.Equals("") && LastName.Text.Equals("") && SSN.Text.Equals("") && Birthdate.Text.Equals("") && Gender.Text.Equals("") && Address.Text.Equals("") && City.Text.Equals("") && State.Text.Equals("") && Zip.Text.Equals("")) InitializeComponent();
            else
            {
                MessageBoxResult result = MessageBox.Show(this, "There are unsave contents, do you still want to restart?", "Unsave Content", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    InitializeComponent();
                }
            }
        }

        private void Button_Click_BackMenu(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text.Equals("") && LastName.Text.Equals("") && SSN.Text.Equals("") && Birthdate.Text.Equals("") && Gender.Text.Equals("") && Address.Text.Equals("") && City.Text.Equals("") && State.Text.Equals("") && Zip.Text.Equals("")) this.Close();
            else
            {
                MessageBoxResult result = MessageBox.Show(this, "There are unsave contents, do you still want to go back?", "Unsave Content", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MainMenu menu = new MainMenu();
                    menu.Show();
                    this.Close();
                }
            }
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
