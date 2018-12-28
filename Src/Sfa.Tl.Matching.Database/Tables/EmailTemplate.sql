﻿CREATE TABLE [dbo].[EmailTemplate]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [TemplateName] NVARCHAR(50) NOT NULL, 
    [TemplateId] NVARCHAR(50) NOT NULL,
    [CreatedOn] DATETIME2 NULL DEFAULT GetDate(), 
    [ModifiedOn] DATETIME2 NULL, 
)
