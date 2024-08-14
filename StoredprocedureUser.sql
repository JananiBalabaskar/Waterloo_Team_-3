USE [Digidentdb]
GO

/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 09-07-2024 03:51:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegisterUser]
    @Email NVARCHAR(256),
    @Password NVARCHAR(256),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATE,
    @Address NVARCHAR(100),
    @PhoneNumber NVARCHAR(15)
AS
BEGIN
    INSERT INTO Users (Email, Password, FirstName, LastName, DateOfBirth, Address, PhoneNumber)
    VALUES (@Email, @Password, @FirstName, @LastName, @DateOfBirth, @Address, @PhoneNumber);
END
GO


