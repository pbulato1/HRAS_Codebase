using System;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for InventoryWithdraw.xaml
    /// </summary>
    public partial class InventoryWithdraw : Window
    {
        DataGrid grid;

		public InventoryWithdraw(DataGrid previousPageDataGrid)
		{
            grid = previousPageDataGrid;
            InitializeComponent();
            Date.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            Date.IsEnabled = false;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            if (ItemName.Text.Equals("") || ItemID.Text.Equals("") || Quantity.Text.Equals("") || PatientID.Text.Equals("")) MessageBox.Show(this, "You have to fill all required fields", "Fill required fields", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                bool success = false;
                try
                {
                    if (InventoryItem.withdrawItem(ItemID.Text, Quantity.Text, PatientID.Text)) success = true;
                    else MessageBox.Show(this, "An error occured.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if (grid != null && success == true)
                {
                    DataTable inventory = InventoryItem.searchInventory(ItemID.Text, "", "");
                    grid.ItemsSource = inventory.DefaultView;
                    grid.AutoGenerateColumns = true;
                    grid.CanUserAddRows = false;
                    this.Close();
                }
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (ItemID.Text.Equals("") && Quantity.Text.Equals("") && PatientID.Text.Equals("")) this.Close();
            else
            {
                MessageBoxResult result = MessageBox.Show(this, "There are unsave contents, do you still want to exit?", "Unsave Content", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}
