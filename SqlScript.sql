-- Check if the database exists, if not create it
IF NOT EXISTS(SELECT *
              FROM sys.databases
              WHERE name = 'BrewlogsDB')
    BEGIN
        CREATE DATABASE BrewlogsDB;
    END
GO

-- Use the BrewlogsDB database
USE BrewlogsDB;
GO

-- Create schema BrewData if it does not exist
IF NOT EXISTS (SELECT *
               FROM sys.schemas
               WHERE name = 'BrewData')
    BEGIN
        EXEC ('CREATE SCHEMA BrewData');
    END
GO

-- Create Users table
CREATE TABLE BrewData.Users
(
    UserId    INT IDENTITY (1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName  NVARCHAR(50),
    Email     NVARCHAR(50),
    Active    BIT
);
GO

-- Create Brewlogs table
CREATE TABLE BrewData.Brewlogs
(
    Id         INT IDENTITY (1,1) PRIMARY KEY,
    Author     INT,
    CoffeeName NVARCHAR(100) NOT NULL,
    Dose       INT           NOT NULL,
    Grind      NVARCHAR(100),
    BrewRatio  INT           NOT NULL,
    Roast      NVARCHAR(100),
    BrewerUsed NVARCHAR(50)  NOT NULL,
    CreatedAt  DATETIME DEFAULT GETDATE(),
    LastEdited DATETIME      NULL
);
GO

-- Create Auth table
CREATE TABLE BrewData.Auth
(
    Email        NVARCHAR(50) PRIMARY KEY,
    PasswordHash VARBINARY(MAX),
    PasswordSalt VARBINARY(MAX)
);
GO

-- Create spRegistration_Upsert stored procedure
CREATE OR ALTER PROCEDURE BrewData.spRegistration_Upsert @Email NVARCHAR(50),
                                                         @PasswordHash VARBINARY(MAX),
                                                         @PasswordSalt VARBINARY(MAX)
AS
BEGIN
    IF NOT EXISTS (SELECT *
                   FROM BrewData.Auth
                   WHERE Email = @Email)
        BEGIN
            INSERT INTO Brewdata.Auth
            ([Email],
             [PasswordHash],
             [PasswordSalt])
            VALUES (@Email,
                    @PasswordHash,
                    @PasswordSalt)
        END
    ELSE
        BEGIN
            UPDATE BrewData.Auth
            SET PasswordHash = @PasswordHash,
                PasswordSalt = @PasswordSalt
            WHERE Email = @Email
        END
END
GO

CREATE OR ALTER PROCEDURE BrewData.spUser_Upsert @FirstName NVARCHAR(50),
                                                 @LastName NVARCHAR(50),
                                                 @Email NVARCHAR(50),
                                                 @Active BIT = 1,
                                                 @UserId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM BrewData.Users WHERE UserId = @UserId)
        BEGIN
            IF NOT EXISTS (SELECT * FROM BrewData.Users WHERE Email = @Email)
                BEGIN
                    INSERT INTO BrewData.Users([FirstName],
                                               [LastName],
                                               [Email],
                                               [Active])
                    VALUES (@FirstName,
                            @LastName,
                            @Email,
                            @Active)
                END
        END
    ELSE
        BEGIN
            UPDATE BrewData.Users
            SET FirstName = @FirstName,
                LastName  = @LastName,
                Email     = @Email,
                Active    = @Active
            WHERE UserId = @UserId
        END
END;
GO

CREATE OR ALTER PROCEDURE BrewData.spPasswordReset @UserId INT,
                                                   @Email NVARCHAR(50),
                                                   @NewPasswordHash VARBINARY(MAX),
                                                   @NewPasswordSalt VARBINARY(MAX)
AS
BEGIN
    IF EXISTS (SELECT 1
               FROM BrewData.Users
               WHERE UserId = @UserId
                 AND Email = @Email)
        BEGIN
            -- Update the password
            UPDATE BrewData.Auth
            SET PasswordHash = @NewPasswordHash,
                PasswordSalt = @NewPasswordSalt
            WHERE Email = @Email;
        END
    ELSE
        BEGIN
            THROW 50001, 'The user ID does not match the provided email address.', 1;
        END
END
GO

-- Create spUser_Upsert stored procedure
CREATE OR ALTER PROCEDURE BrewData.spUser_Upsert @FirstName NVARCHAR(50),
                                                 @LastName NVARCHAR(50),
                                                 @Email NVARCHAR(50),
                                                 @Active BIT = 1,
                                                 @UserId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM BrewData.Users WHERE UserId = @UserId)
        BEGIN
            IF NOT EXISTS (SELECT * FROM BrewData.Users WHERE Email = @Email)
                BEGIN
                    INSERT INTO BrewData.Users([FirstName],
                                               [LastName],
                                               [Email],
                                               [Active])
                    VALUES (@FirstName,
                            @LastName,
                            @Email,
                            @Active)
                END
        END
    ELSE
        BEGIN
            UPDATE BrewData.Users
            SET FirstName = @FirstName,
                LastName  = @LastName,
                Email     = @Email,
                Active    = @Active
            WHERE UserId = @UserId
        END
END;
GO

-- Create spLoginConfirmation_Get stored procedure
CREATE OR ALTER PROCEDURE BrewData.spLoginConfirmation_Get @Email NVARCHAR(50)
AS
BEGIN
    SELECT [Auth].[PasswordHash],
           [Auth].[PasswordSalt]
    FROM BrewData.Auth AS Auth
    WHERE Auth.Email = @Email
END;
GO

-- Create spBrewlogs_Get stored procedure
CREATE OR ALTER PROCEDURE BrewData.spBrewlogs_Get
    @Author INT = NULL,
    @BrewlogId INT = NULL,
    @SearchValue NVARCHAR(MAX) = NULL
AS
BEGIN
    SELECT Brewlogs.Id,
           Brewlogs.Author,
           Brewlogs.CoffeeName,
           Brewlogs.Dose,
           Brewlogs.Grind,
           Brewlogs.BrewRatio,
           Brewlogs.Roast,
           Brewlogs.BrewerUsed,
           Brewlogs.CreatedAt,
           Brewlogs.LastEdited
    FROM BrewData.Brewlogs AS Brewlogs
    WHERE (@BrewlogId IS NULL OR Brewlogs.Id = @BrewlogId)
      AND (@Author IS NULL OR Brewlogs.Author = @Author)
      AND (@SearchValue IS NULL
        OR Brewlogs.CoffeeName LIKE '%' + @SearchValue + '%'
        OR Brewlogs.BrewerUsed LIKE '%' + @SearchValue + '%')
END
GO

-- Create spBrewlogs_Upsert stored procedure
CREATE OR ALTER PROCEDURE BrewData.spBrewlogs_Upsert @Id INT = NULL,
                                                     @Author INT,
                                                     @CoffeeName NVARCHAR(MAX),
                                                     @Dose INT,
                                                     @Grind NVARCHAR(MAX),
                                                     @BrewRatio INT,
                                                     @Roast NVARCHAR(MAX),
                                                     @BrewerUsed NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        IF @Id IS NULL
            BEGIN
                INSERT INTO BrewData.Brewlogs
                ([Author],
                 [CoffeeName],
                 [Dose],
                 [Grind],
                 [BrewRatio],
                 [Roast],
                 [BrewerUsed])
                VALUES (@Author,
                        @CoffeeName,
                        @Dose,
                        @Grind,
                        @BrewRatio,
                        @Roast,
                        @BrewerUsed);
            END
        ELSE
            BEGIN
                UPDATE BrewData.Brewlogs
                SET Author     = @Author,
                    CoffeeName = @CoffeeName,
                    Dose       = @Dose,
                    Grind      = @Grind,
                    BrewRatio  = @BrewRatio,
                    Roast      = @Roast,
                    BrewerUsed = @BrewerUsed,
                    LastEdited = GETDATE()
                WHERE Id = @Id;
            END
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER()    AS ErrorNumber,
               ERROR_SEVERITY()  AS ErrorSeverity,
               ERROR_STATE()     AS ErrorState,
               ERROR_PROCEDURE() AS ErrorProcedure,
               ERROR_LINE()      AS ErrorLine,
               ERROR_MESSAGE()   AS ErrorMessage;
    END CATCH
END
GO
