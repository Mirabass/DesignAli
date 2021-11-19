﻿CREATE TABLE [dbo].[Product]
(
    [Id] INT NOT NULL PRIMARY KEY,
	[Designation] NVARCHAR(50) NOT NULL, 
    [EAN] BIGINT NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
    [Type] NVARCHAR(256) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate()
    
)
