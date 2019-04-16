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
/*Database code ,, Implemantation code is below this
* 
* 
CREATE TABLE Staff
(
[User_Name] varchar(25),
[Password] varchar(50),
User_Type char,
CONSTRAINT PK_Staff PRIMARY KEY 
([User_Name])
)

CREATE TABLE Patient
(
Last_Name varchar(50),
First_Name varchar(25),
Gender char,
SSN char(9),
Birth_Date char(8),
Address_Line1 varchar(35),
Address_Line2 varchar(35),
Address_City varchar(25),
Address_State char(2),
Address_Zip char(5),
DNR_Status char,
Organ_Donor char,
CONSTRAINT PK_Patient PRIMARY KEY 
(SSN)
)

CREATE TABLE Visited_History
(
Patient_SSN char(9),
Entry_Date char(8),
Exit_Date char(8),
Insurer varchar(5),
Diagnosis varchar(75),
Notes varchar(100),
Bill NUMERIC(7,2),
CONSTRAINT PK_Visited_History PRIMARY KEY 
(Patient_SSN, Entry_Date),
CONSTRAINT FK_Visited_History_Patient FOREIGN KEY 
(Patient_SSN) REFERENCES Patient 
(SSN)
)

CREATE TABLE Room
(
Room_Number NUMERIC(9,0),
Hourly_Rate NUMERIC(5,0),
Effective_Date char(8),
CONSTRAINT PK_Room PRIMARY KEY 
(Room_Number)
)

CREATE TABLE Symptom
(
[Name] varchar(25),
CONSTRAINT PK_Symptom PRIMARY KEY 
([Name])
)

CREATE TABLE Item
(
Stock_ID char(5),
Quantity NUMERIC(5,0),
[Description] varchar(35),
Size varchar(3),
Cost NUMERIC(8,0),
CONSTRAINT PK_Item PRIMARY KEY 
(Stock_ID)
)

CREATE TABLE Show_Signs
(
Symptom_Name varchar(25),
Patient_SSN char(9),
Entry_Date char(8),
CONSTRAINT PK_Show_Signs PRIMARY KEY 
(Symptom_Name, Patient_SSN, Entry_Date),
CONSTRAINT FK_Show_Signs_Symptom FOREIGN KEY 
(Symptom_Name) REFERENCES Symptom
([Name]),
CONSTRAINT FK_Show_Signs_Visited_History FOREIGN KEY 
(Patient_SSN, Entry_Date) REFERENCES Visited_History
(Patient_SSN, Entry_Date)
)

CREATE TABLE Stayed_In
(
Room_Number NUMERIC(9,0),
Patient_SSN char(9),
Entry_Date char(8),
CONSTRAINT PK_Stayed_In PRIMARY KEY 
(Room_Number, Patient_SSN, Entry_Date),
CONSTRAINT FK_Stayed_In_Room FOREIGN KEY 
(Room_Number) REFERENCES Room
(Room_Number),
CONSTRAINT FK_Stayed_In_Visited_History FOREIGN KEY 
(Patient_SSN, Entry_Date) REFERENCES Visited_History
(Patient_SSN, Entry_Date)
)

CREATE TABLE Attended_Physican
(
[User_Name] varchar(25),
Patient_SSN char(9),
Entry_Date char(8),
CONSTRAINT PK_Attended_Physican PRIMARY KEY 
([User_name], Patient_SSN, Entry_Date),
CONSTRAINT FK_Attended_Physican_Staff FOREIGN KEY 
([User_Name]) REFERENCES Staff
([User_name]),
CONSTRAINT FK_Attended_Physican_Visited_History FOREIGN KEY 
(Patient_SSN, Entry_Date) REFERENCES Visited_History
(Patient_SSN, Entry_Date)
)

CREATE TABLE Takes_Care
(
Patient_SSN char(9),
[User_Name] varchar(25),
CONSTRAINT PK_Takes_Care PRIMARY KEY 
(Patient_SSN, [User_Name]),
CONSTRAINT FK_Takes_Care_Patient FOREIGN KEY 
(Patient_SSN) REFERENCES Patient
(SSN),
CONSTRAINT FK_Takes_Care_Staff FOREIGN KEY 
([User_Name]) REFERENCES Staff
([User_Name])
)

CREATE TABLE [Use]
(
[User_Name] varchar(25),
Stock_ID char(5),
Patient_SSN char(9),
Entry_Date char(8),
Quantity_Used NUMERIC(5,0),
[Date] char(18),
CONSTRAINT PK_Use PRIMARY KEY 
(Patient_SSN, Entry_Date),
CONSTRAINT FK_Use_Staff FOREIGN KEY 
([User_Name]) REFERENCES Staff
([User_Name]),
CONSTRAINT FK_Use_Item FOREIGN KEY 
(Stock_ID) REFERENCES Item
(Stock_ID),
CONSTRAINT FK_Use_Visited_History FOREIGN KEY 
(Patient_SSN, Entry_Date) REFERENCES Visited_History
(Patient_SSN, Entry_Date)
)

CREATE PROCEDURE Verify_Login @username nvarchar(25), @passwordnvarchar(50)
AS
BEGIN
SELECT User_Name, User_Type
FROM Staff
WHERE User_Name=@username 
AND Password=@password
END
* */
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
		ROOM = 2
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
		ENTRYDATETIMEADDJUSTED = 8,
        EXITDATETIME = 12,
		EXITDATETIMEADDJUSTED = 8,
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
        ORGANDONOR = 1
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
				if (e == User.failedLoginException) throw User.failedLoginException;
				throw failedConnectionException;
			}
		}

        public static Session establishSession(string username, string password)
        {
			Session tempSession;
            if (theSession == null)
            {
                theSession = new Session(username, password);
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

	class Diagnosis
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
		Timer logoffTimer; // This needs to be moved to the parent of the front end
		int AFKTime;
		int timerThreshold = 1000 * 60; // 1000 = 1 second, 60 seconds = 1 minute
		int logoffThreshold = 15;
		int warningThreshold = 10;
		bool loggedIn = false;
		public static Exception failedLoginException = new Exception("Incorrect username or password");

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

		public void resetTimer()
		{
			AFKTime = 0;
			logoffTimer.Stop();
			logoffTimer.Start();
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

	class BasicAddress
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

        public Patient(string theLastName, string theFirstName, char middle, char theGender, string SSN, DateTime birthdate, BasicAddress theAddress, bool theDnrStatus, bool Donor)
        {
            
        }
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
            SqlCommand command = new SqlCommand("Select * FROM Item", connection);
            SqlDataReader reader = command.ExecuteReader();
            inventory.Load(reader);
            return inventory;
        }

        public static DataTable searchInventory(string id, string description, string size)
        {
            DataTable inventory = new DataTable();
            SqlConnection connection = Session.getCurrentSession().getConnection();
            SqlCommand command = new SqlCommand("Select * FROM Item WHERE Item_Description like '%" + description + "%' AND Stock_ID like '%" + id + "%' AND Size like '%" + size + "%'", connection);
            SqlDataReader reader = command.ExecuteReader();
            inventory.Load(reader);
            return inventory;
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
			while ((line = file.ReadLine()) != null) // all casts are just integer enumerations to make it more readable
			{
                string id = line.Substring((int)DataStart.STOCKID, (int)DataLength.STOCKID);
                string quantity = line.Substring((int)DataStart.QUANTITY, (int)DataLength.QUANTITY);
                string description = line.Substring((int)DataStart.DESCRIPTION, (int)DataLength.DESCRIPTION);
                string size = line.Substring((int)DataStart.SIZE, (int)DataLength.SIZE);
                string cost = line.Substring((int)DataStart.COST, (int)DataLength.COST);
                string commandString = "INSERT INTO Item(Stock_ID, Size, Cost, Item_Description, Quantity) VALUES('" + id + "', '" + size + "', '" + cost + "', '" + description + "', '" + quantity + "')";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }
		}

		private static void importMedical(System.IO.StreamReader file, SqlConnection connection)
		{
			string line;
			while ((line = file.ReadLine()) != null)
			{
				string lastName = line.Substring((int)DataStart.LASTNAME, (int)DataLength.LASTNAME);
				lastName = lastName.Replace("'", "''");
				string firstName = line.Substring((int)DataStart.FIRSTNAME, (int)DataLength.FIRSTNAME);
				firstName = firstName.Replace("'", "''");
				string middleInitial = line.Substring((int)DataStart.MIDDLEINITIAL, (int)DataLength.MIDDLEINITIAL);
				string gender = line.Substring((int)DataStart.GENDER, (int)DataLength.GENDER);
				string ssn = line.Substring((int)DataStart.SSN, (int)DataLength.SSN);
				string birthDate = line.Substring((int)DataStart.BIRTHDATE, (int)DataLength.BIRTHDATE);
				string entryDateTime = line.Substring((int)DataStart.ENTRYDATETIME, (int)DataLength.ENTRYDATETIME);
				entryDateTime = entryDateTime.Substring(0, (int)DataLength.ENTRYDATETIMEADDJUSTED);
				string exitDateTime = line.Substring((int)DataStart.EXITDATETIME, (int)DataLength.EXITDATETIME);
				exitDateTime = exitDateTime.Substring(0, (int)DataLength.EXITDATETIMEADDJUSTED);
				string attendingPhys = line.Substring((int)DataStart.ATTENDINGPHY, (int)DataLength.ATTENDINGPHY);
				string roomNo = line.Substring((int)DataStart.ROOMNO, (int)DataLength.ROOMNO);
				string symptom1 = line.Substring((int)DataStart.SYMPTOM1, (int)DataLength.SYMPTOM1);
				string symptom2 = line.Substring((int)DataStart.SYMPTOM2, (int)DataLength.SYMPTOM2);
				string symptom3 = line.Substring((int)DataStart.SYMPTOM3, (int)DataLength.SYMPTOM3);
				string symptom4 = line.Substring((int)DataStart.SYMPTOM4, (int)DataLength.SYMPTOM4);
				string symptom5 = line.Substring((int)DataStart.SYMPTOM5, (int)DataLength.SYMPTOM5);
				string symptom6 = line.Substring((int)DataStart.SYMPTOM6, (int)DataLength.SYMPTOM6);
				string diagnosis = line.Substring((int)DataStart.DIAGNOSIS, (int)DataLength.DIAGNOSIS);
				string notes = line.Substring((int)DataStart.NOTES, (int)DataLength.NOTES);
				string insurer = line.Substring((int)DataStart.INSURER, (int)DataLength.INSURER);
				string addressLine1 = line.Substring((int)DataStart.ADDRESSLINE1, (int)DataLength.ADDRESSLINE1);
				string addressLine2 = line.Substring((int)DataStart.ADDRESSLINE2, (int)DataLength.ADDRESSLINE2);
				string addressCity = line.Substring((int)DataStart.ADDRESSCITY, (int)DataLength.ADDRESSCITY);
				addressCity = addressCity.Replace("'", "''");
				string addressState = line.Substring((int)DataStart.ADDRESSSTATE, (int)DataLength.ADDRESSSTATE);
				string addressZip = line.Substring((int)DataStart.ADDRESSZIP, (int)DataLength.ADDRESSZIP);
				string dnrStatus = line.Substring((int)DataStart.DNRSTATUS, (int)DataLength.DNRSTATUS);
				string organDonor = line.Substring((int)DataStart.ORGANDONOR, (int)DataLength.ORGANDONOR);
				string commandString = "INSERT INTO Patient(Last_Name, First_Name, Middle_Initial, Gender, SSN, Birth_Date, Address_Line1, Address_Line2, Address_City, Address_State, Address_Zip, DNR_Status, Organ_Donor) VALUES('" + 
					lastName + "', '" + firstName + "', '" + middleInitial + "', '" + gender + "', '" + ssn + "', '" + birthDate + "', '" + addressLine1 + "', '" + addressLine2 + "', '" + addressCity + "', '" + addressState + "', '" + 
					addressZip + "', '" + dnrStatus + "', '" + organDonor + "')";
				SqlCommand command = new SqlCommand(commandString, connection);
				command.ExecuteNonQuery();

				commandString = "INSERT INTO Visited_History(Patient_SSN, Entry_Date, Exit_Date, Diagnosis, Insurer, Notes) VALUES('" + ssn + "', '" +
					entryDateTime + "', '" + exitDateTime + "', '" + diagnosis + "', '" + insurer + "', '" + notes + "')";
				command = new SqlCommand(commandString, connection);
				command.ExecuteNonQuery();

				// TODO: !!!!!!!!!!!!!!!!!STILL NEED ATTENDING PHYSICIAN AND SYMPTOMS!!!!!!!!!!!!!!!!!!!!!!!

				command.Dispose();
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
