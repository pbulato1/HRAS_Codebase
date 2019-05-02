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
using System.Timers;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public static Timer logoffTimer = new Timer();
		int AFKTime = 0; // in minutes
		int warningThreshold = 500;
		int logoffThreshold = 1000;

		public MainWindow()
        {
            InitializeComponent();
			logoffTimer.Interval = 60 * 1000; // set for 60 seconds, aka one minute
			logoffTimer.Elapsed += OnTimedEvent;
			logoffTimer.AutoReset = true;
			logoffTimer.Enabled = false;
			btnHiddenLogoff.Visibility = Visibility.Hidden;
		}

		private void LogIn_Click(object sender, RoutedEventArgs e)
		{

			byte[] passwordBytes = Encoding.ASCII.GetBytes(password.Password);
			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			byte[] hashedBytes = sha.ComputeHash(passwordBytes);
			string hashedPassword = Convert.ToBase64String(hashedBytes);
			Session currentSession;
			try
			{
				currentSession = Session.establishSession(Account.Text, hashedPassword);
				logoffTimer.Enabled = true;
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
		}
		private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			AFKTime++;
			if (AFKTime == warningThreshold)
			{
				btnHiddenLogoff.Dispatcher.Invoke(() =>
				{
					MessageBox.Show(this, "No operation for 10 minutes, do you want to exit?", "System Idle", MessageBoxButton.OK, MessageBoxImage.Stop);
				});
			}
			if (AFKTime == logoffThreshold)
			{
				btnHiddenLogoff.Dispatcher.Invoke(() =>
				{
					Session.getCurrentSession().getCurrentUser().logout();
					MainWindow m = new MainWindow();
					m.lockedInfo.Content = "Your session timed out.";
					m.Show();
					foreach (Window w in App.Current.Windows)
					{
						if (w != m) w.Close();
					}
				});
			}
		}


    }
}
