using HRAS;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for PatientInfo.xaml
    /// </summary>
    public partial class PatientInfo : Window
    {
        public PatientInfo(Middleware.MedicalRecord record)
        {
            InitializeComponent();
            Loaded += Patient_Loaded;
			Name.Text = record.getLastName() + ", " + record.getFirstName() + ", " + record.getMiddleInitial();
			ssn.Text = record.getPatient().getSSN();
			gender.Text = "" + record.getPatient().getGender();
			birthday.Text = record.getPatient().getBirthDate().ToShortDateString();
			string addressLineOne = record.getPatient().getAddress().getAddressLineOne();
			string addressLineTwo = record.getPatient().getAddress().getAddressLineTwo();
			string addressCity = record.getPatient().getAddress().getCity();
			string addressState = record.getPatient().getAddress().getState();
			string addressZip = record.getPatient().getAddress().getZip();
			address.Text = addressLineOne + " " + addressLineTwo + " " + addressCity + ", " + addressState + " " + addressZip;
		}

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Button_Click_Checkout(object sender, RoutedEventArgs e)
        {
            Billing billing = new Billing(ssn.Text, new Room(DateTime.Parse(entryDate.Text), roomNum.Text, Room.getHourlyRate(roomNum.Text)));
            billing.Show();
            this.Close();

        }

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();

        }
        private void Patient_Loaded(object sender, EventArgs e)
        {

        }


        private void IR_DataChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_BackMenu(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
