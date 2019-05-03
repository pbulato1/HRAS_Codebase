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
using Middleware;

namespace HRAS
{
    /// <summary>
    /// Billing.xaml
    /// </summary>
    public partial class Billing : Window
    {
		DateTime entryDate;
		Room room;

        public Billing(string recordSSN, Room recordRoom)
        {
            InitializeComponent();
			tfCheckIn.IsReadOnly = true;
			tfDuration.IsReadOnly = true;
			tfHourlyRate.IsReadOnly = true;
			tfRoom.IsReadOnly = true;
			tfCheckIn.Text = room.getEntryDate().ToString("MM\\dd\\yyyy HH\\mm");
			tfDuration.Text = room.getDuration().ToString();
			tfRoom.Text = room.getRoomNumber();
			entryDate = room.getEntryDate();
			room = recordRoom;

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
