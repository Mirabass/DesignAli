CREATE TABLE [dbo].[ProductColorDesign]
(
	[Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY, 
    [Quantity] SMALLINT NULL, 
    [Orientation] NVARCHAR(256) NULL, 
    [MainPartRAL] NUMERIC(4) NULL, 
    [MainPartColorName] NVARCHAR(128) NULL, 
    [PocketColorName] NVARCHAR(128) NULL, 
    [PocketRAL] NUMERIC(4) NULL
)
