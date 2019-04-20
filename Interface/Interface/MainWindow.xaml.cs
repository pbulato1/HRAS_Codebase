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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Middleware;
using System.Security.Cryptography;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public MainWindow()
        {
            InitializeComponent();
        }
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			byte[] passwordBytes = Encoding.ASCII.GetBytes(password.Password);
			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			byte[] hashedBytes = sha.ComputeHash(passwordBytes);
			string hashedPassword = Convert.ToBase64String(hashedBytes);

			Session currentSession;

			try
			{
				currentSession = Session.establishSession(Account.Text, hashedPassword);
				MainMenu m = new MainMenu();
				m.Show();
				this.Close();
			}
			catch (Exception ex)
			{
				if (ex == User.failedLoginException)
				{
					int failedAttempts = User.getFailedAttempts(Account.Text);
					string plural = (failedAttempts == 1) ? "" : "s";
					lockedInfo.Content = "Your account currently has " + failedAttempts + " strike" + plural + ". It will be locked when you reach 5 strikes.";
				}
				else if (ex == User.noAccountException)
				{
					lockedInfo.Content = "The specified user does not exist.";
				}
				else if (ex == Session.failedConnectionException)
				{
					lockedInfo.Content = "A connection to the database could not be established.";
				}
				else if (ex == User.accountLockedException)
				{
					MessageBox.Show(this, "Your account is locked!", "Locked", MessageBoxButton.OK, MessageBoxImage.Stop);
				}
			}


            
            /// I found this is very important in here the lock-out feature is not actually acknowledge which user_ID
            /// They will increase false attempt when every the USER_ID and PASSWORD are not both matched the data base
            /// and then when we enter the right one => we suppose to get into the system.
        }
	}
}
