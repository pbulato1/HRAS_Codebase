﻿using System;
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
    /// Interaction logic for InventoryRecord.xaml
    /// </summary>
    public partial class InventoryRecord : Window
    {
        public InventoryRecord()
        {
            InitializeComponent();
			DG1.IsReadOnly = true;
            Loaded += InventoryRecord_Loaded;
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            AddInventory addInventory = new AddInventory(DG1);
            addInventory.Show();
        }

        private void Button_Click_Withdraw(object sender, RoutedEventArgs e)
        {
            InventoryWithdraw inventoryWithdraw = new InventoryWithdraw(DG1);
            inventoryWithdraw.Show();
        }

        private void Button_Click_History(object sender, RoutedEventArgs e)
        {
            InventoryHistory history = new InventoryHistory();
            history.Show();
            this.Close();
        }

        private void InventoryRecord_Loaded(object sender, EventArgs e)
        {
            DataTable inventory = InventoryItem.getInventory();
            DG1.ItemsSource = inventory.DefaultView;
            DG1.AutoGenerateColumns = true;
            DG1.CanUserAddRows = false;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            if (tfStockID != null && tfDescription != null && tfSize != null)
            {
                DataTable inventory = InventoryItem.searchInventory(tfStockID.Text, tfDescription.Text, tfSize.Text);
                DG1.ItemsSource = inventory.DefaultView;
                DG1.AutoGenerateColumns = true;
                DG1.CanUserAddRows = false;
            }
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
       
    }
}
