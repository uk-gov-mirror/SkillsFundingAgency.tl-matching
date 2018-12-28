﻿CREATE TABLE [dbo].[Course]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [LarsId] UNIQUEIDENTIFIER NOT NULL, 
    [QualificationTitle] NVARCHAR(100) NOT NULL, 
    [Summary] NVARCHAR(50) NULL, 
    [Keywords] NVARCHAR(50) NULL, 
    [CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
    [ModifiedOn] DATETIME2 NULL
)
