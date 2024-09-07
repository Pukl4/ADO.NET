USE master;
GO

CREATE DATABASE University;
GO

USE University;
GO

CREATE TABLE Students (
	Id INT PRIMARY KEY IDENTITY(1,1), 
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	Email NVARCHAR(20),
	PhoneNumber NVARCHAR(25),
	AverageGrade DOUBLE(2,1),
	FacultyId INT 
);
GO

INSERT INTO Students
VALUES 
	('Nick', 'Smith', 'n.f@protonmail.edu', ' 123-45-111-22-33', 8.5, 1),
	('Sally', 'Cage', 'el@google.couk', '222-11-333-23-23', 5.7, 2),
	('George', 'Jones', 'd.m@yahoo.net', '111-22-123-22-11', 7.5, 2),
	('Rick', 'Davis', 'sit@yahoo.couk', '333-33-222-11-00', 7.8, 3),
	-- the last student does not have a faculty id so there would be a noticable difference when practicing JOINs
	('Erich', 'Miller', 'u.p.c@aol.net', '321-12-122-33-22', 4.9, NULL);
GO

CREATE TABLE Faculties (
	Id INT PRIMARY KEY IDENTITY(1,1), -- a primary key that is populated automatically by the database engine
	Name NVARCHAR(20) NOT NULL,
	Description NVARCHAR(30)
);
GO

INSERT INTO Faculties
VALUES
	('Engineering', 'Faculty of Engineering'),
	('Economics', 'Faculty of Economics'),
	('Law', 'Faculty of Law');
GO

CREATE PROCEDURE sp_AddStudent 
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

CREATE PROCEDURE sp_SelectAllStudents
AS
BEGIN    
    SELECT * FROM Students
END;
GO

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

CREATE FUNCTION fk_GetFacultyName
(
	@StudentId INT
)
RETURNS NVARCHAR(20)
AS
BEGIN
	DECLARE @FacultyName NVARCHAR(20);
	DECLARE @FacultyId INT;

	SELECT @FacultyId = st.FacultyId
	FROM Students st
	WHERE Id = @StudentId;

	SELECT @FacultyName = fc.Name
	FROM Faculties fc
	WHERE Id = @FacultyId;
	
	RETURN @FacultyName;
END;
GO