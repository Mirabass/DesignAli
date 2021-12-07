CREATE TABLE [dbo].[ProductDivision]
(
	[Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY,
	[Number] NUMERIC(3,0) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
    [ProductKindId] INT NOT NULL,
	CONSTRAINT [FK_ProductKind_ProductDivision] FOREIGN KEY ([ProductKindId]) REFERENCES [ProductKind]([Id]),
	[ProductMaterialId] INT NOT NULL,
	CONSTRAINT [FK_ProductMaterial_ProductDivision] FOREIGN KEY ([ProductMaterialId]) REFERENCES [ProductMaterial]([Id]),
	[ProductType] NVARCHAR(256) NOT NULL,
	[Comment] NVARCHAR(MAX)
)
