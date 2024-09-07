-- tell the database engine to run commands against the master database
USE master;
GO

-- create a new University database
CREATE DATABASE University;
GO

-- tell the database engine to run commands against the newely created University database
USE University;
GO

-- create Students table
CREATE TABLE Students (
	Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by database engine
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	Email NVARCHAR(20),
	PhoneNumber NVARCHAR(25),
	AverageGrade FLOAT,
	FacultyId INT -- a foreign key to relate Students table to Faculties table
);
GO

-- populate Students table
INSERT INTO Students
VALUES 
	('Nick', 'Smith', 'n.f@protonmail.edu', ' 123-45-111-22-33', 8.5, 1),
	('Sally', 'Cage', 'el@google.couk', '222-11-333-23-23', 5.7, 2),
	('George', 'Jones', 'd.m@yahoo.net', '111-22-123-22-11', 7.5, 2),
	('Rick', 'Davis', 'sit@yahoo.couk', '333-33-222-11-00', 7.8, 3),
	-- the last student does not have a faculty id so there would be a noticable difference when practicing JOINs
	('Erich', 'Miller', 'u.p.c@aol.net', '321-12-122-33-22', 4.9, NULL);
GO

--create Faculties table
CREATE TABLE Faculties (
	Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by the database engine
	Name NVARCHAR(20) NOT NULL,
	Description NVARCHAR(30)
);
GO

--populate Faculties table
INSERT INTO Faculties
VALUES
	('Engineering', 'Faculty of Engineering'),
	('Economics', 'Faculty of Economics'),
	('Law', 'Faculty of Law');
GO

-- create a stored procedure to add a new student
CREATE PROCEDURE sp_AddStudent  -- stored procedure name  
(
	@FirstName NVARCHAR(20),
	@LastName NVARCHAR(20),
	@Email NVARCHAR(20),
	@PhoneNumber NVARCHAR(25),
	@AverageGrade FLOAT,
	@FacultyId INT,
	@Id INT OUTPUT 
)    
AS
BEGIN    
    INSERT INTO Students(FirstName, LastName, Email, PhoneNumber, AverageGrade, FacultyId)    
    VALUES(@FirstName, @LastName, @Email, @PhoneNumber, @AverageGrade, @FacultyId);
	
	SET @Id = SCOPE_IDENTITY() 
    RETURN @Id 
END;
GO

-- create a stored procedure to select all students
CREATE PROCEDURE sp_SelectAllStudents
AS
BEGIN    
    SELECT * FROM Students
END;
GO

-- create a stored procedure to delete a student by id 
CREATE PROCEDURE sp_DeleteStudent
(
	@Id INT
)
AS
BEGIN
	DELETE FROM Students
	WHERE Id = @Id
END;
GO

-- create a function that returns faculty names by student ids
CREATE FUNCTION fk_GetFacultyName
(
	@StudentId INT
)
-- define the output type
RETURNS NVARCHAR(20)
AS
-- BEGIN / END to indicate the function body
BEGIN
	DECLARE @FacultyName NVARCHAR(20);
	DECLARE @FacultyId INT;

	-- get student faculty id
	SELECT @FacultyId = st.FacultyId
	FROM Students st
	WHERE Id = @StudentId;

	-- get faculty name by id
	SELECT @FacultyName = fc.Name
	FROM Faculties fc
	WHERE Id = @FacultyId;
	
	-- returns the result
	RETURN @FacultyName;
END;
GO