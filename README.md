# HRAS_Codebase
All of the contributor can log-in to the demo system by their first name as user name and last name as their password.

CREATE DATABASE HRAS_iTas_Test

GO
USE HRAS_iTas_Test

CREATE TABLE Staff
(
[User_Name] varchar(25),
[Password] varchar(50),
User_Type char,
Failed_Login int,
CONSTRAINT PK_Staff PRIMARY KEY 
([User_Name])
)

CREATE TABLE Patient
(
Last_Name varchar(50),
First_Name varchar(25),
Middle_initial char,
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
Entry_Date datetime(8),
Exit_Date datetime(8),
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
Entry_Date datetime(8),
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
Entry_Date datetime(8),
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
Stock_ID char(5),
[User_Name] varchar(25),
Patient_SSN char(9),
Entry_Date datetime(8),
Quantity_Used NUMERIC(5,0),
[Date] char(18),
CONSTRAINT PK_Use PRIMARY KEY 
(Stock_ID, [User_Name], Patient_SSN, Entry_Date),
CONSTRAINT FK_Use_Item FOREIGN KEY 
(Stock_ID) REFERENCES Item
(Stock_ID),
CONSTRAINT FK_Use_Staff FOREIGN KEY 
([User_Name]) REFERENCES Staff
([User_Name]),
CONSTRAINT FK_Use_Visited_History FOREIGN KEY 
(Patient_SSN, Entry_Date) REFERENCES Visited_History
(Patient_SSN, Entry_Date)
)

USE HRAS_iTas_Test

CREATE PROCEDURE Verify_Login @username nvarchar(25), @password varchar(50)
AS
BEGIN
SELECT User_Name, User_Type
FROM Staff
WHERE User_Name = @username 
AND Password = @password
END

CREATE PROCEDURE Import_Medical_Record @ssn varchar(9), @entryDateTime varchar(12), @exitDateTime varchar(12), @diagnosis varchar(75), @insurer varchar(5), @notes varchar(100)
AS
BEGIN
INSERT INTO Visited_History(Patient_SSN, Entry_Date, Exit_Date, Diagnosis, Insurer, Notes) VALUES(@ssn, @entryDateTime, @exitDateTime, @diagnosis, @insurer, @notes)
END

CREATE PROCEDURE Import_Patient @lastName varchar(50), @firstName varchar(25), @middleInitial varchar(1), @gender varchar(1), @ssn varchar(9), @birthDate varchar(8), @addressLine1 varchar(35), @addressLine2 varchar(35), @addressCity varchar(25), @addressState varchar(2), @addressZip varchar(5), @dnrStatus varchar(1), @organDonor varchar(1)
AS
BEGIN
INSERT INTO Patient(Last_Name, First_Name, Middle_Initial, Gender, SSN, Birth_Date, Address_Line1, Address_Line2, Address_City, Address_State, Address_Zip, DNR_Status, Organ_Donor) VALUES(@lastName, @firstName, @middleInitial, @gender, @ssn, @birthDate, @addressLine1, @addressLine2, @addressCity, @addressState, @addressZip, @dnrStatus, @organDonor)
END

CREATE PROCEDURE Import_Room @roomNumber varchar(9), @hourlyRate numeric(5), @effectiveDate varchar(8)
AS
BEGIN
INSERT INTO Room(Room_Number, Hourly_Rate, Effective_Date) VALUES(@roomNumber, @hourlyRate, @effectiveDate)
END

CREATE PROCEDURE Import_Show_Signs @symptomName varchar(25), @ssn numeric(9), @entryDate varchar(12)
AS
BEGIN
INSERT INTO Show_Signs(Symptom_Name, Patient_SSN, Entry_Date) VALUES(@symptomName, @ssn, @entryDate)
END

CREATE PROCEDURE Import_Symptom @symptomName varchar(25)
AS
BEGIN
INSERT INTO Symptom(Name) VALUES(@symptomName)
END

CREATE PROCEDURE Retrieve_Symptom @symptomName varchar(25)
AS
BEGIN
Select @symptomName FROM Symptom
END

CREATE PROCEDURE Import_Stayed_In @roomNumber varchar(9), @ssn varchar(9), @entryDate varchar(12)
AS
BEGIN
INSERT INTO Stayed_In(Room_Number, Patient_SSN, Entry_Date) VALUES(@roomNumber, @ssn, @entryDate)
END

GO

CREATE USER HRAS_MW_iTas IDENTIFIED BY ZMNv01X
IDENTIFIED WITH READ
IDENTIFIED WITH WRITE
IDENTIFIED WITH Verify_Login
IDENTIFIED WITH Import_Medical_Record
IDENTIFIED WITH Import_Patient
IDENTIFIED WITH Import_Room
IDENTIFIED WITH Import_Show_Signs
IDENTIFIED WITH Import_Symptom
IDENTIFIED WITH Retrieve_Symptom
IDENTIFIED WITH Import_Stayed_In
