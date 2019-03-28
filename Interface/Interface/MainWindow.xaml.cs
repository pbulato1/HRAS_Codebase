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

			Session currentSession = new Session(Account.Text, hashedPassword);

			if (currentSession.verifySession())
			{
				MainMenu m = new MainMenu();
				m.Show();
				this.Close();
				failedAttempts = 0;
			}
			else
			{
				failedAttempts++;
				if (failedAttempts == attemptThreshold)
				{
					MessageBox.Show(this, "Your account is locked!", "Locked", MessageBoxButton.OK, MessageBoxImage.Stop);
				}
				else
				{
					lockedInfo.Content = "Your account currently has " + failedAttempts + " strike. It will be locked when you reach " + attemptThreshold + " strikes.";
				}
			}
        }

		private void Account_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
}
