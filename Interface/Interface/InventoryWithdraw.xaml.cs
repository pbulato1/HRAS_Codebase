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
			Price.IsEnabled = false;
			Price.Background = Brushes.Gray;
			ItemName.IsEnabled = false;
			ItemName.Background = Brushes.Gray;
			Date.Text = DateTime.Today.ToString("MM/dd/yyyy");
			Date.IsEnabled = false;
		}

		public InventoryWithdraw()
        {
            InitializeComponent();
			Price.IsEnabled = false;
			Price.Background = Brushes.Gray;
			ItemName.IsEnabled = false;
			ItemName.Background = Brushes.Gray;
			Date.Text = DateTime.Today.ToString("MM/dd/yyyy");
			Date.IsEnabled = false;
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

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            //Need warning sign here
            this.Close();
        }
    }
}
