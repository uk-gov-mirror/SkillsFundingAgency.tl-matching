﻿/*
Insert initial data for Email Templates
*/

MERGE INTO [dbo].[EmailTemplate] AS Target 
USING (VALUES 
	(N'ProviderReferral', N'f2a7a475-6bbb-4ca7-a010-14d83e9ed90a'),
	(N'ProvisionGapReport', N'4c8c1cb2-9d8c-4bc6-9ad4-ce5f98ba7220')
  )
  AS Source ([TemplateName], [TemplateId]) 
ON Target.[TemplateId] = Source.[TemplateId] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[TemplateName] <> Source.[TemplateName]) 
THEN 
UPDATE SET 
	[TemplateName] = Source.[TemplateName],
	[TemplateId] = Source.[TemplateId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([TemplateName], [TemplateId], [CreatedBy]) 
	VALUES ([TemplateName], [TemplateId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
