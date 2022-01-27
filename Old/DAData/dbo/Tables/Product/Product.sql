CREATE TABLE [dbo].[Product]
(
    [Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY,
    [Designation] NVARCHAR(15) NOT NULL,
    [EAN] BIGINT NOT NULL DEFAULT 1111111111111, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Design] NUMERIC(4) NOT NULL,
    [ProductDivisionId] INT NOT NULL
    CONSTRAINT [FK_ProductDivision_Product] FOREIGN KEY ([ProductDivisionId]) REFERENCES [ProductDivision]([Id]), 
    [ProductColorDesignId] INT NOT NULL,
    [ProductStrapId] INT NOT NULL, 
    [Motive] NVARCHAR(2048) NULL, 
    [Accessories] NVARCHAR(256) NULL, 
    CONSTRAINT [FK_ProductColorDesign_Product] FOREIGN KEY ([ProductColorDesignId]) REFERENCES [ProductColorDesign]([Id]),
    CONSTRAINT [FK_ProductStrap_Product] FOREIGN KEY ([ProductStrapId]) REFERENCES [ProductStrap]([Id])
)