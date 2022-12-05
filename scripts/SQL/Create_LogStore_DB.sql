USE [master];
GO

IF (DB_ID('LogStore') IS NOT NULL)
	BEGIN
		DROP DATABASE [LogStore]
	END;
GO

IF (DB_ID('LogStore') IS NULL)
	BEGIN
		CREATE DATABASE [LogStore]
	END;
GO

USE [LogStore];

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SourceSystems')
	CREATE TABLE SourceSystems (
		[ID] int IDENTITY (1,1) PRIMARY KEY,
		[Name] nvarchar (255),
		[SourceFolder] nvarchar (1024),
		[LineTemplate] nvarchar (255)
	)

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LogFiles')
	CREATE TABLE LogFiles (
		[ID] [int] IDENTITY (1,1) PRIMARY KEY,
		[SourceSystemID] int FOREIGN KEY REFERENCES SourceSystems,
		[FileName] nvarchar (255),
		[FileHash] nvarchar (255) ) 
	GO
	   
IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LogLines')
	CREATE TABLE LogLines (
		[ID] bigint IDENTITY (1,1) PRIMARY KEY,
		[SourceSystemID] int FOREIGN KEY REFERENCES SourceSystems,
		[LogFileID] int FOREIGN KEY REFERENCES LogFiles,
		[TimeOfEvent] datetime,
		[Severity] varchar (100),
		[EventDescription] nvarchar (1024),
		[SourceModule] nvarchar (1024),
		[RawText] nvarchar (2048)
	)