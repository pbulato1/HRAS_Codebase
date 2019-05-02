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
        private bool isTextBoxEmpty()
        {
            if (ItemName.Text.Equals("") || ItemID.Text.Equals("") || Quantity.Text.Equals("") || DayPicker.SelectedDate.Equals("")/*this is not right yet*/ || PatientID.Text.Equals("")) return true;
            return false;
        }

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
            if (!isTextBoxEmpty())
            {
                MessageBoxResult result = MessageBox.Show(this, "Do you want to submit?", "Submit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                    InventoryItem.addInventory(ItemName.Text, ItemID.Text, Quantity.Text, Price.Text);
                }
                InventoryItem.addInventory(ItemName.Text, ItemID.Text, Quantity.Text, Price.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "You have to fill all required fields", "Fill required fields", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (!isTextBoxEmpty())
            {
                MessageBoxResult result = MessageBox.Show(this, "There are unsave contents, do you still want to exit?", "Unsave Content", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else this.Close();
        }
    }
}
