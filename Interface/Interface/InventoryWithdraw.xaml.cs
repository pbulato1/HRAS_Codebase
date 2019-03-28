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
    /// Interaction logic for InventoryWithdraw.xaml
    /// </summary>
    public partial class InventoryWithdraw : Window
    {
        public InventoryWithdraw()
        {
            InitializeComponent();
        }

        private void ItemName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ItemID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Date_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            //Need connection here
            this.Close();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            //Need warning sign here
            this.Close();
        }
    }
}
