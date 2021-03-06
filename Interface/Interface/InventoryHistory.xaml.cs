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
    /// Interaction logic for InventoryHistory.xaml
    /// </summary>
    public partial class InventoryHistory : Window
    {
        public InventoryHistory()
        {
            InitializeComponent();
			D1.IsReadOnly = true;
			Loaded += InventoryHistory_Loaded;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            InventoryRecord inventory = new InventoryRecord();
            inventory.Show();
            this.Close();
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


        private void InventoryHistory_Loaded(object sender, EventArgs e)
        {
			DataTable inventory = InventoryItem.getInventoryHistory();
			D1.ItemsSource = inventory.DefaultView;
			D1.AutoGenerateColumns = true;
			D1.CanUserAddRows = false;
		}

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //DataTable inventoryHistory = ;
            //D1.ItemsSource = inventoryHistory.DefaultView;
            D1.AutoGenerateColumns = true;
            D1.CanUserAddRows = false;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IR_DataChange(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
