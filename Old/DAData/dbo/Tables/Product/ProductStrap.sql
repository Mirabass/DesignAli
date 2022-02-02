CREATE TABLE [dbo].[ProductStrap]
(
	[Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY, 
    [Type] NVARCHAR(256) NULL, 
    [Material] NVARCHAR(256) NULL, 
    [Length] NUMERIC(5, 3) NULL, 
    [Width] NUMERIC(5, 3) NULL, 
    [RAL] NUMERIC(4) NULL, 
    [ColorName] NVARCHAR(128) NULL, 
    [Attachment] NVARCHAR(128) NULL
)
