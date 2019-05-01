using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Security.Cryptography;
using System.Threading;

namespace Middleware
{
	public enum PrivilegeLevels
	{
		NONE = 0,
		R = 1,
		A = 2,
	}

	public enum ImportType
	{
		MEDICAL = 0,
		INVENTORY = 1,
		ROOM = 2,
		USERS = 4
	}

    public enum DataLength
    {
        //INVENTORY
        STOCKID = 5,
        QUANTITY = 5,
        DESCRIPTION = 35,
        SIZE = 3,
        COST = 8,
        //MEDICAL RECORD
        LASTNAME = 50,
        FIRSTNAME = 25,
        MIDDLEINITIAL = 1,
        GENDER = 1,
        SSN = 9,
        BIRTHDATE = 8,
        ENTRYDATETIME = 12,
        EXITDATETIME = 12,
		ATTENDINGPHY = 5,
        ROOMNO = 9,
        SYMPTOM1 = 25,
        SYMPTOM2 = 25,
        SYMPTOM3 = 25,
        SYMPTOM4 = 25,
        SYMPTOM5 = 25,
        SYMPTOM6 = 25,
        DIAGNOSIS = 75,
        NOTES = 100,
        INSURER = 5,
        ADDRESSLINE1 = 35,
        ADDRESSLINE2 = 35,
        ADDRESSCITY = 25,
        ADDRESSSTATE = 2,
        ADDRESSZIP = 5,
        DNRSTATUS = 1,
        ORGANDONOR = 1,
		//ROOM
		ROOMNUMBER = 9,
		HOURLYRATE = 5,
		EFFECTIVEDATE = 8,
		//USERS
		USERNAME = 25,
		PASSWORD = 50,
		USERTYPE = 1
    }

    public enum DataStart
    {
        //INVENTORY
        STOCKID = 0,
        QUANTITY = DataLength.STOCKID,
        DESCRIPTION = DataLength.STOCKID + DataLength.QUANTITY,
        SIZE = DataLength.STOCKID + DataLength.QUANTITY + DataLength.DESCRIPTION,
        COST = DataLength.STOCKID + DataLength.QUANTITY + DataLength.DESCRIPTION + DataLength.SIZE,
        //MEDICAL RECORD
        LASTNAME = 0,
        FIRSTNAME = DataLength.LASTNAME,
        MIDDLEINITIAL = FIRSTNAME + DataLength.FIRSTNAME,
        GENDER = MIDDLEINITIAL + DataLength.MIDDLEINITIAL,
        SSN = GENDER + DataLength.GENDER,
        BIRTHDATE = SSN + DataLength.SSN,
        ENTRYDATETIME = BIRTHDATE + DataLength.BIRTHDATE,
        EXITDATETIME = ENTRYDATETIME + DataLength.ENTRYDATETIME,
        ATTENDINGPHY = EXITDATETIME + DataLength.EXITDATETIME,
        ROOMNO = ATTENDINGPHY + DataLength.ATTENDINGPHY,
        SYMPTOM1 = ROOMNO + DataLength.ROOMNO,
        SYMPTOM2 = SYMPTOM1 + DataLength.SYMPTOM1,
        SYMPTOM3 = SYMPTOM2 + DataLength.SYMPTOM2,
        SYMPTOM4 = SYMPTOM3 + DataLength.SYMPTOM3,
        SYMPTOM5 = SYMPTOM4 + DataLength.SYMPTOM4,
        SYMPTOM6 = SYMPTOM5 + DataLength.SYMPTOM5,
        DIAGNOSIS = SYMPTOM6 + DataLength.SYMPTOM6,
        NOTES = DIAGNOSIS + DataLength.DIAGNOSIS,
        INSURER = NOTES + DataLength.NOTES,
        ADDRESSLINE1 = INSURER + DataLength.INSURER,
        ADDRESSLINE2 = ADDRESSLINE1 + DataLength.ADDRESSLINE1,
        ADDRESSCITY = ADDRESSLINE2 + DataLength.ADDRESSLINE2,
        ADDRESSSTATE = ADDRESSCITY + DataLength.ADDRESSCITY,
        ADDRESSZIP = ADDRESSSTATE + DataLength.ADDRESSSTATE,
        DNRSTATUS = ADDRESSZIP + DataLength.ADDRESSZIP,
        ORGANDONOR = DNRSTATUS + DataLength.DNRSTATUS,
		//ROOM
		ROOMNUMBER = 0,
		HOURLYRATE = ROOMNUMBER + DataLength.ROOMNUMBER,
		EFFECTIVEDATE = HOURLYRATE + DataLength.HOURLYRATE,
		//USERS
		USERNAME = 0,
		PASSWORD = DataLength.USERNAME + USERNAME,
		USERTYPE = DataLength.PASSWORD + PASSWORD
	}

    public class Session
	{
		SqlConnection conn;
		User currentUser;
        static Session theSession;
		public static Exception failedConnectionException = new Exception("A connection to the database could not be established.");


		private Session(string username, string password)
		{
			try
			{
                string connectionString = Properties.Settings1.Default.CONNECTIONSTRING;
                conn = new SqlConnection(connectionString);
				conn.Open();
				currentUser = new User();
				if (!currentUser.login(username, password, conn))
				{
					throw User.failedLoginException;
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public static Session establishSession(string username, string password)
        {
            if (theSession == null)
            {
				try
				{
					theSession = new Session(username, password);
				}
				catch (Exception e)
				{
					throw e;
				}
			}
            return theSession;
        }

        public static Session getCurrentSession()
        {
            return theSession;
        }

        public SqlConnection getConnection()
		{
			return conn;
		}

		public void closeConnection()
		{
			conn.Close();
            theSession = null;
		}

		public bool verifySession()
		{
			return currentUser.isLoggedIn();
		}

		public User getCurrentUser()
		{
			return currentUser;
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

	public class Diagnosis
	{
		string diagnosisName;
		List<string> symptoms;
	}

	public class PasswordHasher
	{
		public static string hashPassword(string pass)
		{
			byte[] passwordBytes = Encoding.ASCII.GetBytes(pass);
			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			byte[] hashedBytes = sha.ComputeHash(passwordBytes);
			return Convert.ToBase64String(hashedBytes);
		}
	}

	public class User
	{
		string username;
		string password;
		int privilegeLevel;
		bool loggedIn = false;
		public static Exception failedLoginException = new Exception("Incorrect username or password.");
		public static Exception noAccountException = new Exception("The username given does not exist.");
		public static Exception accountLockedException = new Exception("The specified account is locked.");
		int loginAttemptThreshold = 5;

		public bool login(string enteredUsername, string enteredPassword, SqlConnection connection)
		{
			bool matchFound = false;
			string queryString = "Verify_Username";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@userName", enteredUsername));
			SqlDataReader dataReader = command.ExecuteReader();
			bool userExists = false;
			while (dataReader.Read())
			{
				userExists = true;
			}
			if (!userExists) throw noAccountException;
			else
			{
				int loginAttempts = getFailedAttempts(enteredUsername);
				if (loginAttempts >= loginAttemptThreshold) throw accountLockedException;
			}
			queryString = "Verify_Login";
			command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@username", enteredUsername));
			command.Parameters.Add(new SqlParameter("@password", enteredPassword));
			dataReader.Close();
			dataReader = command.ExecuteReader();
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
					loggedIn = true;
				}
				else throw failedLoginException;
			}
			dataReader.Close();
			command.Dispose();

			return matchFound;
		}

		public static int getFailedAttempts(string userName)
		{
			string connectionString = Properties.Settings1.Default.CONNECTIONSTRING;
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			string queryString = "Get_Failed_Attempts";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@userName", userName));
			SqlDataReader dataReader = command.ExecuteReader();
			int indexAttempts = dataReader.GetOrdinal("Failed_Login");
			int fails = 5;
			while (dataReader.Read())
			{
				fails = dataReader.GetInt32(indexAttempts);
			}
			connection.Close();
			return fails;
		}



		public bool logout()
		{
			try
			{
				username = "";
				password = "";
				privilegeLevel = (int)PrivilegeLevels.NONE; // this is ok becuase PrivilegeLevels is an enumeration thus the values are actually ints
				//logoffTimer = null;
				loggedIn = false;
				Session currentSession = Session.getCurrentSession();
				currentSession.closeConnection();
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

        public string getUsername()
        {
            return username;
        }

		public int getPrivilegeLevel()
		{
			return privilegeLevel;
		}
	}

	public class BasicAddress
	{
		private string addressLineOne;
		private string addressLineTwo;
		private string city;
		private string state;
		private string zip;

        public BasicAddress(string line1, string line2, string theCity, string theState, string theZip)
        {
            // this should connect the datebase directly, 
        }

	}

	public class Patient
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

        public Patient(string theLastName, string theFirstName, char middle, char theGender, string SSN, DateTime birthdate, BasicAddress theAddress, bool theDnrStatus, bool Donor)
        {
            
        }
	}

	public class MedicalRecord
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

		public static DataTable getMedicalRecords()
		{
			DataTable records = new DataTable();
			SqlConnection connection = Session.getCurrentSession().getConnection();
			string queryString = "Get_Medical_Records_Top";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			SqlDataReader reader = command.ExecuteReader();
			records.Load(reader);
			return records;
		}

		public static DataTable searchMedicalRecords(string input)
		{
			DataTable records = new DataTable();
			SqlConnection connection = Session.getCurrentSession().getConnection();
			string queryString = "Search_Medical_Records";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@input", input));
			SqlDataReader reader = command.ExecuteReader();
			records.Load(reader);
			return records;
		}

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

			//connection.Open();
			//SqlCommand command = new SqlCommand(commandString, connection);
			//command.ExecuteNonQuery();
			//command.Dispose();
			//connection.Close();
		}

		public void addRoom(Room theRoom)
		{

		}

		public void removeRoom(Room theRoom)
		{

		}
	}

	public class InventoryItem
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

        public static DataTable  getInventory()
        {
            DataTable inventory = new DataTable();
            SqlConnection connection = Session.getCurrentSession().getConnection();
			string queryString = "Get_Items_Top";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			SqlDataReader reader = command.ExecuteReader();
			inventory.Load(reader);
            return inventory;
        }

        public static DataTable searchInventory(string id, string description, string size)
        {
            DataTable inventory = new DataTable();
            SqlConnection connection = Session.getCurrentSession().getConnection();
			string queryString = "Search_Items";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@stockID", id));
			command.Parameters.Add(new SqlParameter("@description", description));
			command.Parameters.Add(new SqlParameter("@size", size));
			SqlDataReader reader = command.ExecuteReader();
            inventory.Load(reader);
            return inventory;
        }

		public static bool addInventory(string description, string stockID, string size, string quantity, string price)
		{
			int numQuantity = 0;
			int numPrice = 0;
			bool priceEntered = false;
			bool quantityEntered = false;
			if (quantity != "")
			{
				try
				{
					numQuantity = Int32.Parse(quantity);
					quantityEntered = true;
				}
				catch (Exception e) { return false; }
			}
			if (price != "")
			{
				try
				{
					numPrice = Int32.Parse(price);
					priceEntered = true;
				}
				catch (Exception e) { return false; }
			}
			SqlConnection connection = Session.getCurrentSession().getConnection();
			bool alreadyExists = false;
			string queryString = "Retrieve_Item";
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@stockID", stockID));
			SqlDataReader dataReader = command.ExecuteReader();
			while (dataReader.Read())
			{
				alreadyExists = true;
			}
			dataReader.Close();

			if (!alreadyExists)
			{
				queryString = "Import_Item";
				command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@stockID", stockID));
				command.Parameters.Add(new SqlParameter("@description", description));
				command.Parameters.Add(new SqlParameter("@size", size));
				if (priceEntered) command.Parameters.Add(new SqlParameter("@cost", numPrice));
				if (quantityEntered) command.Parameters.Add(new SqlParameter("@quantity", numQuantity));
				try
				{
					command.ExecuteNonQuery();
					return true;
				}
				catch (Exception) { return false; }
			}
			else
			{
				queryString = "Add_To_Existing_Inventory";
				command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@stockID", stockID));
				command.Parameters.Add(new SqlParameter("@quantity", numQuantity));
				try
				{
					command.ExecuteNonQuery();
					return true;
				}
				catch (Exception) { return false; }
			}
		}
    }

	public class Room
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
		public static int progress = 0;

		public static int getProgress()
		{
			return progress;
		}

		public static void import(string filePath, ImportType type, Session session)
		{
			Thread thread;
			long fileSize = new System.IO.FileInfo(filePath).Length;
			System.IO.StreamReader file = new System.IO.StreamReader(@filePath);
			switch (type)
			{
				case ImportType.INVENTORY:
					thread = new Thread(() => importInventory(file, session.getConnection(), fileSize));
					thread.Start();
					break;
				case ImportType.MEDICAL:
					thread = new Thread(() => importMedical(file, session.getConnection(), fileSize));
					thread.Start();
					break;
				case ImportType.ROOM:
					thread = new Thread(() => importRoom(file, session.getConnection(), fileSize));
					thread.Start();
					break;
				case ImportType.USERS:
					thread = new Thread(() => importUser(file, session.getConnection(), fileSize));
					thread.Start();
					break;
			}
		}

		private static void importInventory(System.IO.StreamReader file, SqlConnection connection, long fileSize)
		{
			int bytesRead = 0;
			string line;
			int quantity = 0;
			bool hasQuantity = false;
			while ((line = file.ReadLine()) != null) // all casts are just integer enumerations to make it more readable
			{
				bytesRead += line.Length;
                string id = line.Substring((int)DataStart.STOCKID, (int)DataLength.STOCKID);
				try
				{
					quantity = Int32.Parse(line.Substring((int)DataStart.QUANTITY, (int)DataLength.QUANTITY));
					hasQuantity = true;
				}
				catch (Exception)
				{
					hasQuantity = false;
				}
                string description = line.Substring((int)DataStart.DESCRIPTION, (int)DataLength.DESCRIPTION);
                string size = line.Substring((int)DataStart.SIZE, (int)DataLength.SIZE);
                int cost = Int32.Parse(line.Substring((int)DataStart.COST, (int)DataLength.COST));

				string queryString = "Import_Inventory";
				SqlCommand command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@stockID", id));
				command.Parameters.Add(new SqlParameter("@quantity", quantity));
				command.Parameters.Add(new SqlParameter("@description", description));
				command.Parameters.Add(new SqlParameter("@size", size));
				command.Parameters.Add(new SqlParameter("@cost", cost));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur
                command.Dispose();
				double updatedProgress = (bytesRead * 100) / fileSize;
				if (updatedProgress < 100) progress = (int)updatedProgress; // ok because it will always be less than a hundred
			}
			progress = 100;
			file.Close();
		}

		private static void importMedical(System.IO.StreamReader file, SqlConnection connection, long fileSize)
		{
			int bytesRead = 0;
			string line;
			while ((line = file.ReadLine()) != null)
			{
				bytesRead += line.Length;
				string lastName = line.Substring((int)DataStart.LASTNAME, (int)DataLength.LASTNAME);
				lastName = lastName.Trim();
				string firstName = line.Substring((int)DataStart.FIRSTNAME, (int)DataLength.FIRSTNAME);
				firstName = firstName.Trim();
				string middleInitial = line.Substring((int)DataStart.MIDDLEINITIAL, (int)DataLength.MIDDLEINITIAL);
				string gender = line.Substring((int)DataStart.GENDER, (int)DataLength.GENDER);
				string ssn = line.Substring((int)DataStart.SSN, (int)DataLength.SSN);
				string birthDate = line.Substring((int)DataStart.BIRTHDATE, (int)DataLength.BIRTHDATE);
				string entryDateTime = line.Substring((int)DataStart.ENTRYDATETIME, (int)DataLength.ENTRYDATETIME);
				DateTime dtEntryDateTime = DateTime.ParseExact(entryDateTime, "MMddyyyyHHmm", null);
				string exitDateTime = line.Substring((int)DataStart.EXITDATETIME, (int)DataLength.EXITDATETIME);
				DateTime dtExitDateTime = DateTime.ParseExact(exitDateTime, "MMddyyyyHHmm", null);
				string attendingPhys = line.Substring((int)DataStart.ATTENDINGPHY, (int)DataLength.ATTENDINGPHY);
				string roomNo = line.Substring((int)DataStart.ROOMNO, (int)DataLength.ROOMNO);
				string symptom1 = line.Substring((int)DataStart.SYMPTOM1, (int)DataLength.SYMPTOM1);
				symptom1 = symptom1.Trim();
				string symptom2 = line.Substring((int)DataStart.SYMPTOM2, (int)DataLength.SYMPTOM2);
				symptom2 = symptom2.Trim();
				string symptom3 = line.Substring((int)DataStart.SYMPTOM3, (int)DataLength.SYMPTOM3);
				symptom3 = symptom3.Trim();
				string symptom4 = line.Substring((int)DataStart.SYMPTOM4, (int)DataLength.SYMPTOM4);
				symptom4 = symptom4.Trim();
				string symptom5 = line.Substring((int)DataStart.SYMPTOM5, (int)DataLength.SYMPTOM5);
				symptom5 = symptom5.Trim();
				string symptom6 = line.Substring((int)DataStart.SYMPTOM6, (int)DataLength.SYMPTOM6);
				symptom6 = symptom6.Trim();
				string diagnosis = line.Substring((int)DataStart.DIAGNOSIS, (int)DataLength.DIAGNOSIS);
				diagnosis = diagnosis.Trim();
				string notes = line.Substring((int)DataStart.NOTES, (int)DataLength.NOTES);
				string insurer = line.Substring((int)DataStart.INSURER, (int)DataLength.INSURER);
				string addressLine1 = line.Substring((int)DataStart.ADDRESSLINE1, (int)DataLength.ADDRESSLINE1);
				string addressLine2 = line.Substring((int)DataStart.ADDRESSLINE2, (int)DataLength.ADDRESSLINE2);
				string addressCity = line.Substring((int)DataStart.ADDRESSCITY, (int)DataLength.ADDRESSCITY);
				string addressState = line.Substring((int)DataStart.ADDRESSSTATE, (int)DataLength.ADDRESSSTATE);
				string addressZip = line.Substring((int)DataStart.ADDRESSZIP, (int)DataLength.ADDRESSZIP);
				string dnrStatus = line.Substring((int)DataStart.DNRSTATUS, (int)DataLength.DNRSTATUS);
				string organDonor = line.Substring((int)DataStart.ORGANDONOR, (int)DataLength.ORGANDONOR);

				string queryString = "Import_Patient";
				SqlCommand command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@lastName", lastName));
				command.Parameters.Add(new SqlParameter("@firstName", firstName));
				command.Parameters.Add(new SqlParameter("@middleInitial", middleInitial));
				command.Parameters.Add(new SqlParameter("@gender", gender));
				command.Parameters.Add(new SqlParameter("@ssn", ssn));
				command.Parameters.Add(new SqlParameter("@birthDate", birthDate));
				command.Parameters.Add(new SqlParameter("@addressLine1", addressLine1));
				command.Parameters.Add(new SqlParameter("@addressLine2", addressLine2));
				command.Parameters.Add(new SqlParameter("@addressCity", addressCity));
				command.Parameters.Add(new SqlParameter("@addressState", addressState));
				command.Parameters.Add(new SqlParameter("@addressZip", addressZip));
				command.Parameters.Add(new SqlParameter("@dnrStatus", dnrStatus));
				command.Parameters.Add(new SqlParameter("@organDonor", organDonor));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur

				queryString = "Import_Medical_Record";
				command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@ssn", ssn));
				command.Parameters.Add(new SqlParameter("@entryDateTime", dtEntryDateTime));
				command.Parameters.Add(new SqlParameter("@exitDateTime", dtExitDateTime));
				command.Parameters.Add(new SqlParameter("@diagnosis", diagnosis));
				command.Parameters.Add(new SqlParameter("@insurer", insurer));
				command.Parameters.Add(new SqlParameter("@notes", notes));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur

				string[] symptomList = { symptom1, symptom2, symptom3, symptom4, symptom5, symptom6 };
				foreach (string symptom in symptomList)
				{
					bool alreadyExists = false;
					queryString = "Retrieve_Symptom";
					command = new SqlCommand(queryString, connection);
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@symptomName", symptom));
					SqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						alreadyExists = true;
					}
					dataReader.Close();

					if (!alreadyExists)
					{
						queryString = "Import_Symptom";
						command = new SqlCommand(queryString, connection);
						command.CommandType = System.Data.CommandType.StoredProcedure;
						command.Parameters.Add(new SqlParameter("@symptomName", symptom));
						try
						{
							command.ExecuteNonQuery();
						}
						catch (Exception) { } // This is for the duplictes that occur
					}

					queryString = "Import_Show_Signs";
					command = new SqlCommand(queryString, connection);
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@symptomName", symptom));
					command.Parameters.Add(new SqlParameter("@ssn", ssn));
					command.Parameters.Add(new SqlParameter("@entryDate", dtEntryDateTime));
					try
					{
						command.ExecuteNonQuery();
					}
					catch (Exception) { } // This is for the duplictes that occur
				}

				queryString = "Import_Stayed_In";
				command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@roomNumber", roomNo));
				command.Parameters.Add(new SqlParameter("@ssn", ssn));
				command.Parameters.Add(new SqlParameter("@entryDate", dtEntryDateTime));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur

				// TODO: !!!!!!!!!!!!!!!!!STILL NEED ATTENDING PHYSICIAN!!!!!!!!!!!!!!!!!!!!!!!

				command.Dispose();
				double updatedProgress = (bytesRead * 100) / fileSize;
				if (updatedProgress < 100) progress = (int)updatedProgress; // ok because it will always be less than a hundred
			}
			progress = 100;
			file.Close();
		}

		private static void importRoom(System.IO.StreamReader file, SqlConnection connection, long fileSize)
		{
			int bytesRead = 0;
			string line;
			while ((line = file.ReadLine()) != null)
			{
				bytesRead += line.Length;
				string roomNumber = line.Substring((int)DataStart.ROOMNUMBER, (int)DataLength.ROOMNUMBER);
				decimal hourlyRate = Decimal.Parse(line.Substring((int)DataStart.HOURLYRATE, (int)DataLength.HOURLYRATE));
				hourlyRate /= 100;
				string effectiveDate = line.Substring((int)DataStart.EFFECTIVEDATE, (int)DataLength.EFFECTIVEDATE);
				string queryString = "Import_Room";
				SqlCommand command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@roomNumber", roomNumber));
				command.Parameters.Add(new SqlParameter("@hourlyRate", hourlyRate));
				command.Parameters.Add(new SqlParameter("@effectiveDate", effectiveDate));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur

				command.Dispose();
				double updatedProgress = (bytesRead * 100) / fileSize;
				if (updatedProgress < 100) progress = (int)updatedProgress; // ok because it will always be less than a hundred
			}
			progress = 100;
			file.Close();
		}

		private static void importUser(System.IO.StreamReader file, SqlConnection connection, long fileSize)
		{
			int bytesRead = 0;
			string line;
			while ((line = file.ReadLine()) != null)
			{
				bytesRead += line.Length;
				string username = line.Substring((int)DataStart.USERNAME, (int)DataLength.USERNAME);
				username = username.Trim();
				string password = line.Substring((int)DataStart.PASSWORD, (int)DataLength.PASSWORD);
				password = password.Trim();
				string hashedPassword = PasswordHasher.hashPassword(password);
				string userType = line.Substring((int)DataStart.USERTYPE, (int)DataLength.USERTYPE);
				string queryString = "Import_User";
				SqlCommand command = new SqlCommand(queryString, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@username", username));
				command.Parameters.Add(new SqlParameter("@password", hashedPassword));
				command.Parameters.Add(new SqlParameter("@userType", userType));
				command.Parameters.Add(new SqlParameter("@failedLogins", '0'));
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception) { } // This is for the duplictes that occur

				command.Dispose();
				double updatedProgress = (bytesRead * 100) / fileSize;
				if (updatedProgress < 100) progress = (int)updatedProgress; // ok because it will always be less than a hundred
			}
			progress = 100;
			file.Close();
		}
	}
}