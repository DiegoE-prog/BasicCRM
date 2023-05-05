USE BasicCRM;

CREATE TABLE [dbo].[Addresses](
	[AddressID] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
	[AddressLine] VARCHAR(250) NOT NULL,
	[AddressDetails] VARCHAR(250) NULL,
	[City] VARCHAR(50) NOT NULL,
	[State] VARCHAR(50) NOT NULL,
	[ZipCode] INT NOT NULL,
	[Country] VARCHAR(25) NOT NULL,
	CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressID])
);

CREATE TABLE [dbo].[Clients](
	[ClientID] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
	[FirstName] VARCHAR(25) NOT NULL,
	[LastName] VARCHAR(25) NOT NULL,
	[DateOfBirth] DATE NOT NULL,
	[Email] VARCHAR(25) NOT NULL,
	[PhoneNumber] VARCHAR(15) NOT NULL,
	CONSTRAINT [ClientID] PRIMARY KEY CLUSTERED([ClientID]),
	[AddressID] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES [dbo].[Addresses]([AddressID]),
);

ALTER TABLE [dbo].[Addresses]
ADD IsActive BIT;

ALTER TABLE [dbo].[Clients]
ADD IsActive BIT;

-- GetAll
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllAddresses]
AS
BEGIN 
	SELECT * FROM Addresses
	WHERE IsActive = 1
END;

-- Get By ID
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAddressById]
	@AddressID UNIQUEIDENTIFIER
AS
BEGIN 
	SELECT * FROM Addresses
	WHERE AddressID = @AddressID AND IsActive = 1
END;

-- Insert
CREATE OR ALTER PROCEDURE [dbo].[usp_CreateAddress]
	@AddressID UNIQUEIDENTIFIER = NULL, 
	@AddressLine VARCHAR(250),
	@AddressDetails VARCHAR(250) = NULL,
	@City VARCHAR(50),
	@State VARCHAR(50),
	@ZipCode INT,
	@Country VARCHAR(25)
AS 
BEGIN
BEGIN TRY 
	BEGIN TRAN
	IF(@AddressID IS NULL OR @AddressID = '00000000-0000-0000-0000-000000000000')
		SET @AddressID = newid()
	INSERT INTO [dbo].[Addresses]
	VALUES
	(@AddressID, @AddressLine, @AddressDetails, @City, @State, @ZipCode, @Country, 1)
	COMMIT TRAN 
END TRY
BEGIN CATCH 
	ROLLBACK TRAN
END CATCH 
END;

-- Update
CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateAddress]
(	
	@AddressID UNIQUEIDENTIFIER, 
	@AddressLine VARCHAR(250),
	@AddressDetails VARCHAR(250) = NULL,
	@City VARCHAR(50),
	@State VARCHAR(50),
	@ZipCode INT,
	@Country VARCHAR(25)
)
AS 
BEGIN
BEGIN TRY
	BEGIN TRAN
		UPDATE [dbo].[Addresses]
			SET AddressLine = @AddressLine,
				AddressDetails = @AddressDetails,
				City = @City,
				State = @State,
				ZipCode = @ZipCode,
				Country = @Country
		WHERE AddressID = @AddressID
	COMMIT TRAN 
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH
END;

CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteAddress]
	@AddressID UNIQUEIDENTIFIER
AS 
BEGIN
BEGIN TRY 
	BEGIN TRAN
		UPDATE [dbo].[Addresses]
			SET IsActive = 0
		WHERE AddressId = @AddressID
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN
END CATCH
END;

EXEC [dbo].[usp_GetAllAddresses];

EXEC [dbo].[usp_GetAddressById] @AddressID = '975D832B-C83B-431C-B95A-79BEE99B43BE';

EXEC [dbo].[usp_CreateAddress] @AddressID = NULL, @AddressLine = "Echo Street",
@City = "Uriangato", @AddressDetails = NULL, @State ="Guanajuato", @ZipCode = 38887, @Country = Mexico;

EXEC [dbo].[usp_UpdateAddress] @AddressID = '212974DC-B299-4E62-81CC-FF1E9D1E7321', @AddressLine = 'Lone Street',
@City = 'Moroleon', @State ='Guanajuato', @ZipCode = 38887, @Country = Mexico;

EXEC [dbo].[usp_DeleteAddress] @AddressID = '212974DC-B299-4E62-81CC-FF1E9D1E7321';

---- Client Store Procedures ----

-- Get All Clients
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllClients]
AS
BEGIN 
	SELECT "ClientID", "FirstName", "LastName", "DateOfBirth", "Email", "PhoneNumber", "AddressID"
	FROM Clients
	WHERE IsActive = 1
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllClientsWithAddress]
AS
BEGIN 
	SELECT c.ClientID, c.FirstName, c.LastName, c.DateOfBirth, c.Email, c.PhoneNumber,
		a.AddressID, a.AddressLine, a.AddressDetails, a.City, a.State, a.ZipCode, a.Country
	FROM Clients AS c
	LEFT JOIN Addresses AS a 
	ON c.AddressID = a.AddressID
	WHERE c.IsActive = 1 
END

-- Get Client By Id
CREATE OR ALTER PROCEDURE [dbo].[usp_GetClientById]
	@ClientID UNIQUEIDENTIFIER
AS
BEGIN 
	SELECT "ClientID", "FirstName", "LastName", "DateOfBirth", "Email", "PhoneNumber", "AddressID"
	FROM Clients
	WHERE IsActive = 1 AND ClientID = @ClientID
END

CREATE OR ALTER PROCEDURE [dbo].[usp_GetClientWithAddressById]
	@ClientID UNIQUEIDENTIFIER
AS
BEGIN 
	SELECT c.ClientID, c.FirstName, c.LastName, c.DateOfBirth, c.Email, c.PhoneNumber,
		a.AddressID, a.AddressLine, a.AddressDetails, a.City, a.State, a.ZipCode, a.Country
	FROM Clients AS c
	LEFT JOIN Addresses AS a 
	ON c.AddressID = a.AddressID
	WHERE c.IsActive = 1  AND ClientID = @ClientID
END

-- Insert
CREATE OR ALTER PROCEDURE [dbo].[usp_CreateClient]
	@FirstName VARCHAR(25),
	@LastName VARCHAR(25),
	@DateOfBirth DATE,
	@Email VARCHAR(25),
	@PhoneNumber VARCHAR(15),
	@AddressID UNIQUEIDENTIFIER = NULL
AS
BEGIN
BEGIN TRY 
	BEGIN TRAN 
		INSERT INTO [dbo].[Clients](FirstName, LastName, DateOfBirth, Email, PhoneNumber, AddressID, IsActive)
		VALUES
		(@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber, @AddressID, 1)
	COMMIT TRAN
END TRY
BEGIN CATCH
	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRAN
END CATCH
END

-- Update Client
CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateClient]
	@ClientID UNIQUEIDENTIFIER,
	@FirstName VARCHAR(25),
	@LastName VARCHAR(25),
	@DateOfBirth DATE,
	@Email VARCHAR(25),
	@PhoneNumber VARCHAR(15),
	@AddressID UNIQUEIDENTIFIER = NULL
AS
BEGIN 
BEGIN TRY 
	BEGIN TRAN 
		UPDATE [dbo].[Clients]
			SET FirstName = @FirstName,
				LastName = @LastName,
				DateOfBirth = @DateOfBirth,
				Email = @Email,
				PhoneNumber = @PhoneNumber,
				AddressID = @AddressID
			WHERE ClientID = @ClientID
	COMMIT TRAN
END TRY
BEGIN CATCH
	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRAN
END CATCH
END

CREATE OR ALTER PROCEDURE [dbo].[usp_DeleteClient]
	@ClientID UNIQUEIDENTIFIER
AS
BEGIN
BEGIN TRY
	BEGIN TRAN
		UPDATE [dbo].[Clients]
			SET IsActive = 0
		WHERE ClientID = @ClientID
	COMMIT TRAN
END TRY
BEGIN CATCH
	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRAN
END CATCH
END

EXEC [dbo].[usp_CreateClient] @FirstName = 'Diego', @LastName = 'Olvera', @DateOfBirth='1997-07-05', 
	@Email= 'diego.Olvera@hotmail.com', @PhoneNumber = ''; 

EXEC [dbo].[usp_UpdateClient] @ClientID = '6BEC120D-10B3-43FB-B3EA-0D4A88E3B07D', @FirstName = 'Diego', @LastName = 'Sargent',
	@DateOfBirth = '1997-07-05', @Email = 'diego.sargent@hotmail.com',
	@PhoneNumber = '445132343222222222222222', @AddressID = '975D832B-C83B-431C-B95A-79BEE99B43BE';

EXEC [dbo].[usp_DeleteClient] @ClientID = '691569B1-A729-4291-AC67-B019EB608027'

EXEC [dbo].[usp_GetAllClients];

EXEC [dbo].[usp_GetAllClientsWithAddress];

EXEC [dbo].[usp_GetClientWithAddressById] @ClientId = '51694513-8D80-4ABA-8BF1-1E727D1F75AD';

EXEC [dbo].[usp_GetAllClientWithAddressById] @ClientId = '6BEC120D-10B3-43FB-B3EA-0D4A88E3B07D' ;
