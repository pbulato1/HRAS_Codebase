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
		int failedAttempts = 0;
		int attemptThreshold = 5;

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

			Session currentSession = Session.establishSession(Account.Text, hashedPassword);

			if (currentSession.verifySession())
			{
				MainMenu m = new MainMenu();
				m.Show();
				this.Close();
				failedAttempts = 0;
			}
            failedAttempts++;

            if (failedAttempts < attemptThreshold)
            {
                lockedInfo.Content = "Your account currently has " + failedAttempts + " strike. It will be locked when you reach " + attemptThreshold + " strikes.";
            }
            else
			{
                lockedInfo.Content = "Your account currently has " + failedAttempts + " strike. It will be locked when you reach " + attemptThreshold + " strikes.";
                MessageBox.Show(this, "Your account is locked!", "Locked", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
            
            /// I found this is very important in here the lock-out feature is not actually acknowledge which user_ID
            /// They will increase false attempt when every the USER_ID and PASSWORD are not both matched the data base
            /// and then when we enter the right one => we suppose to get into the system.
        }
	}
}
