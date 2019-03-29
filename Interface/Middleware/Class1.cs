using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Middleware
{
	enum PrivilegeLevels
	{
		NONE = 0,
		R = 1,
		A = 2,
	}

	public enum ImportType
	{
		MEDICAL = 0,
		INVENTORY = 1,
		ROOM = 2
	}

	public enum DataLength
	{
		STOCKID = 5,
		QUANTITY = 5,
		DESCRIPTION = 35,
		SIZE = 3,
		COST = 8
	}

	public enum DataStart
	{
		STOCKID = 0,
		QUANTITY = DataLength.STOCKID,
		DESCRIPTION = DataLength.STOCKID + DataLength.QUANTITY,
		SIZE = DataLength.STOCKID + DataLength.QUANTITY + DataLength.DESCRIPTION,
		COST = DataLength.STOCKID + DataLength.QUANTITY + DataLength.DESCRIPTION + DataLength.SIZE
	}

	public class Session
	{
		SqlConnection conn;
		User currentUser;
        public Session(string username, string password)
		{
			try
			{
				string connectionString = "Data Source=localhost;Initial Catalog=DummyDatabase;Integrated Security=true;";// User ID=UserName;Password=Password";
				SqlConnection connection = new SqlConnection(connectionString);
				connection.Open();
				currentUser = new User();
				currentUser.login(username, password, connection);
			}
			catch (Exception e)
			{
				throw new Exception("A connection to the database could not be established.");
			}
		}

		public SqlConnection getConnection()
		{
			return conn;
		}

		public void closeConnection()
		{
			currentUser.logout();
			conn.Close();
		}

		public bool verifySession()
		{
			return currentUser.isLoggedIn();
		}
	}

	class DiagnosisWizard
	{
		Dictionary<Diagnosis, double> diagnosisList;
		string diagnosisQuery;
		string symptomQuery;

		public void eliminateSymptom(string symptom, bool has)
		{
			string operand = (has) ? "=" : "<>";
			diagnosisQuery = diagnosisQuery + " AND Symptom " + operand + " '" + symptom + "'";
		}

		public string getNextSymptom(string symptomQuery)
		{
			return "";
		}

		private Dictionary<Diagnosis, double> calculateProbabilities()
		{
			return new Dictionary<Diagnosis, double>();
		}

		public Dictionary<Diagnosis, double> getProbableDiagnosis(string diagnosisQuery)
		{
			return new Dictionary<Diagnosis, double>();
		}
	}

	class Diagnosis
	{
		string diagnosisName;
		List<string> symptoms;
	}

	class User
	{
		string username;
		string password;
		int privilegeLevel;
		Timer logoffTimer; // This needs to be moved to the parent of the front end
		int AFKTime;
		int timerThreshold = 1000 * 60; // 1000 = 1 second, 60 seconds = 1 minute
		int logoffThreshold = 15;
		int warningThreshold = 10;
		bool loggedIn = false;

		public bool login(string enteredUsername, string enteredPassword, SqlConnection connection)
		{
			bool matchFound = false;
			string queryString = "Verify_Login";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@username", enteredUsername));
			command.Parameters.Add(new SqlParameter("@password", enteredPassword));
			SqlDataReader dataReader = command.ExecuteReader();
			while (dataReader.Read())
			{
				int indexUsername = dataReader.GetOrdinal("User_Name");
				string retrievedUsername = dataReader.GetString(indexUsername);
				if (retrievedUsername == enteredUsername) // not necessary, but in case of hacker mischief
				{
					matchFound = true;
					username = enteredUsername;
					password = enteredPassword;
					int index = dataReader.GetOrdinal("User_Type");
					string privilege = dataReader.GetString(index);
					privilegeLevel = (int)(Enum.Parse(typeof(PrivilegeLevels), privilege));
					logoffTimer = new Timer();
					AFKTime = 0;
					logoffTimer.Interval = timerThreshold;
					logoffTimer.Elapsed += OnTimedEvent;
					logoffTimer.Enabled = true;
					loggedIn = true;
				}
				else throw new Exception("An error occurred while logging the user in.");
			}
			dataReader.Close();
			command.Dispose();

			return matchFound;
		}

		private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			AFKTime++;
			if (AFKTime == warningThreshold)
			{
				// code to display a warning here
			}
			if (AFKTime == logoffThreshold)
			{
				logout();
			}
		}

		public bool logout()
		{
			try
			{
				username = "";
				password = "";
				privilegeLevel = (int)PrivilegeLevels.NONE; // this is ok becuase PrivilegeLevels is an enumeration thus the values are actually ints
				logoffTimer = null;
				AFKTime = 0;
				loggedIn = false;
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool isLoggedIn()
		{
			return loggedIn;
		}

		public void resetTimer()
		{
			AFKTime = 0;
			logoffTimer.Stop();
			logoffTimer.Start();
		}
	}

	class BasicAddress
	{
		private string addressLineOne;
		private string addressLineTwo;
		private string city;
		private string state;
		private string zip;

        public BasicAddress(string line1, string line2, string theCity, string theState, string theZip)
        {
            addressLineOne = line1;
            addressLineTwo = line2;
            city = theCity;
            state = theState;
            zip = theZip;
        }

	}

	class Patient
	{
		string lastName;
		string firstName;
		char middleInitial;
		char gender;
		string ssn;
		DateTime birthDate;
		BasicAddress address;
		bool dnrStatus;
		bool organDonor;
	}

	class MedicalRecord
	{
		Patient patient;
		DateTime entryDate;
		DateTime exitDate;
		string attendingPhysician;
		Dictionary<Room, double> previousRooms;
		Room currentRoom;
		Diagnosis diagnosis;
		string notes;
		string insurer;
		List<InventoryItem> suppliesUsed;

		public void addSupply(InventoryItem item)
		{
			suppliesUsed.Add(item);
			// update the database here
		}

		public void removeSupply(InventoryItem item)
		{

		}

		public void checkIn(DateTime date)
		{
			entryDate = date;

		}

		public void checkOut(DateTime date)
		{

		}

		public void generateBill()
		{

		}

		public void addNote(string note)
		{

		}

		public void diagnose(Diagnosis theDiagnosis)
		{
			diagnosis = theDiagnosis;

			string connectionString = "Data Source=localhost;Initial Catalog=DummyDatabase;Integrated Security=true;";// User ID=UserName;Password=Password";
			string commandString = "UPDATE Staff SET Password = 'HellsYeah' WHERE User_Name = sami; ";
			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand(commandString, connection);
				command.ExecuteNonQuery();
				command.Dispose();
				connection.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("A connection to the database could not be established.");
			}
		}

		public void addRoom(Room theRoom)
		{

		}

		public void removeRoom(Room theRoom)
		{

		}
	}

	class InventoryItem
	{
		string stockID;
		int quantity;
		string description;
		int size;
		double cost;

		public double getTotalCost()
		{
			return 0;
		}
	}

	class Room
	{
		int roomNumber;
		double hourlyRate;
		Dictionary<Patient, DateTime> occupants;

		public void checkIn(DateTime date)
		{

		}

		public double checkOut(DateTime date)
		{
			return 0;
		}

		public void adjustRate(double newRate)
		{

		}

		public double calculateCost(Patient thePatient)
		{
			return 0;
		}
	}

	public class ImportData
	{
		public static void import(string filePath, ImportType type, Session session)
		{
			System.IO.StreamReader file = new System.IO.StreamReader(@filePath);
			switch (type)
			{
				case ImportType.INVENTORY:
					importInventory(file, session.getConnection());
					break;
				case ImportType.MEDICAL:
					importMedical(file, session.getConnection());
					break;
				case ImportType.ROOM:
					importRoom(file, session.getConnection());
					break;
			}
			file.Close();
		}

		private static void importInventory(System.IO.StreamReader file, SqlConnection connection)
		{
			string line;
			while ((line = file.ReadLine()) != null)
			{
				int id = Int32.Parse(line.Substring(0, 5));
				int quantity = Int32.Parse(line.Substring(5, 5));
				string description = line.Substring(10, 35);
				int size = Int32.Parse(line.Substring(45, 3));
				int cost = Int32.Parse(line.Substring(48, 8));
				string commandString = "UPDATE Staff SET Password = 'HellsYeah' WHERE User_Name = sami; ";
				SqlCommand command = new SqlCommand(commandString, connection);
				command.ExecuteNonQuery();
				command.Dispose();
				System.Console.WriteLine(line);
			}
		}

		private static void importMedical(System.IO.StreamReader file, SqlConnection connection)
		{
			string line;
			while ((line = file.ReadLine()) != null)
			{

			}
		}

		private static void importRoom(System.IO.StreamReader file, SqlConnection connection)
		{
			string line;
			while ((line = file.ReadLine()) != null)
			{
				System.Console.WriteLine(line);
			}
		}
	}
}
