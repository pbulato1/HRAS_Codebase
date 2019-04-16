using Interface;
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

namespace HRAS
{
    /// <summary>
    /// Billing.xaml
    /// </summary>
    public partial class Billing : Window
    {
        public Billing()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            MessageBox.Show(this, "Successfully checked！", " Check out at " + " " + currentTime, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();

        }
    }
}
