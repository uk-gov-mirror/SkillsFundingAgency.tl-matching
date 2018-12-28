﻿CREATE TABLE [dbo].[Contact]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[EntityRefId] UNIQUEIDENTIFIER NOT NULL, 
	[ContactTypeId] INT NOT NULL, 
	[PreferredContactMethodType] INT NOT NULL,
	[IsPrimary] BIT NOT NULL DEFAULT 0,
	[FirstName] NVARCHAR(100) NULL, 
	[MiddleName] NVARCHAR(100) NULL, 
	[LastName] NVARCHAR(100) NULL, 
	[JobTitle] NVARCHAR(100) NULL, 
	[BusinessPhone] NVARCHAR(100) NULL, 
	[MobilePhone] NVARCHAR(100) NULL, 
	[HomePhone] NVARCHAR(100) NULL, 
	[Email] NVARCHAR(100) NULL, 
	[CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
	[ModifiedOn] DATETIME2 NULL, 
	CONSTRAINT [FK_Contact_Employer_EntityRefId] FOREIGN KEY (EntityRefId) REFERENCES [Employer]([Id]), 
	CONSTRAINT [FK_Contact_Provider_EntityRefId] FOREIGN KEY (EntityRefId) REFERENCES [Provider]([Id]), 
)
