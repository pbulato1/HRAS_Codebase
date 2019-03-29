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
using System.Data;

namespace Interface
{
    /// <summary>
    /// Interaction logic for InventoryRecord.xaml
    /// </summary>
    public partial class InventoryRecord : Window
    {
        public InventoryRecord()
        {
            InitializeComponent();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            AddInventory addInventory = new AddInventory();
            addInventory.Show();
        }

        private void Button_Click_Withdraw(object sender, RoutedEventArgs e)
        {
            InventoryWithdraw inventoryWithdraw = new InventoryWithdraw();
            inventoryWithdraw.Show();
        }

        private void Button_Click_History(object sender, RoutedEventArgs e)
        {
            InventoryHistory history = new InventoryHistory();
            history.Show();
            this.Close();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {

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

        private void IR_DataChange(object sender, SelectionChangedEventArgs e)
        {
        }
        
    }
}
