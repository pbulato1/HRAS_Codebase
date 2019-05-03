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
    /// Interaction logic for CheckIn.xaml
    /// </summary>
    public partial class CheckIn : Window
    {
        public CheckIn()
        {
            InitializeComponent();
        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FirstName.Text = "";
        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SSN_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Birthdate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Address_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void City_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Zip_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            //Need connection here => save item
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text.Equals("") && LastName.Text.Equals("") && SSN.Text.Equals("") && Birthdate.Text.Equals("") && Phone.Text.Equals("") && Address.Text.Equals("") && City.Text.Equals("") && State.Text.Equals("") && Zip.Text.Equals("")) this.Close();
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
            if (FirstName.Text.Equals("") && LastName.Text.Equals("") && SSN.Text.Equals("") && Birthdate.Text.Equals("") && Phone.Text.Equals("") && Address.Text.Equals("") && City.Text.Equals("") && State.Text.Equals("") && Zip.Text.Equals("")) this.Close();
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
