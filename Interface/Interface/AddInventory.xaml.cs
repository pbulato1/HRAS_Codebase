using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for AddInventory.xaml
    /// </summary>


    public partial class AddInventory : Window
    {
        private bool isTextBoxEmpty()
        {
            if (ItemName.Text.Equals("") || ItemID.Text.Equals("") || Quantity.Text.Equals("") || DayPicker.SelectedDate.Equals("")/*this is not right yet*/) return true;
            return false;
        }

		DataGrid grid;
		int ItemIDStandardLength = 5;

		public AddInventory(DataGrid previousPageDataGrid)
		{
			grid = previousPageDataGrid;
			InitializeComponent();
		}


        public AddInventory()
        {
            InitializeComponent();
		}

        private void ItemName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // check if the format of the box is correct or not for every text box.
        }

        private void ItemID_TextChanged(object sender, TextChangedEventArgs e)
        {
			bool currentAddExists = InventoryItem.itemExists(ItemID.Text);
			if (currentAddExists)
			{
				Price.IsEnabled = false;
				Price.Background = Brushes.Gray;
				ItemName.IsEnabled = false;
				ItemName.Background = Brushes.Gray;
				Size.IsEnabled = false;
				Size.Background = Brushes.Gray;

			}
			else
			{
				Price.IsEnabled = true;
				Price.Background = Brushes.White;
				ItemName.IsEnabled = true;
				ItemName.Background = Brushes.White;
				Size.IsEnabled = true;
				Size.Background = Brushes.White;
			}
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

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            bool success = false;
            bool currentAddExists = InventoryItem.itemExists(ItemID.Text);

            if (!isTextBoxEmpty())
            {
                MessageBoxResult result = MessageBox.Show(this, "Do you want to submit?", "Submit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (currentAddExists)
                    {
                        if (!InventoryItem.addInventory(ItemID.Text, Quantity.Text))
                        {
                            MessageBox.Show(this, "An error occurred while adding to the database, make sure that the data format " +
                                "is correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        else { success = true; };
                    }
                    else
                    {
                        if (ItemID.Text.Length != ItemIDStandardLength)
                        {
                            MessageBox.Show(this, "The Item ID must be of length " + ItemIDStandardLength, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        else
                        {
                            try
                            {
                                if (!InventoryItem.addInventory(ItemName.Text, ItemID.Text, Size.Text, Quantity.Text, Price.Text))
                                {
                                    MessageBox.Show(this, "An error occurred while adding to the database, make sure that the data format " +
                                        "is correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                                }
                                else { success = true; }
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
