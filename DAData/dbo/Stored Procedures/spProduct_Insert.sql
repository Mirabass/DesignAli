CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Id int output,
	@Designation nvarchar(15),
	@Motive nvarchar(2048),
	@EAN bigint,
	@Accessories nvarchar(256),
	@Design int,
	@ProductColorDesignId int,
	@ProductDivisionId int,
	@ProductStrapId int,
	@NewProductColorDesignId int output,
	@NewProductStrapId int output
AS
begin
	set nocount on;
	insert into dbo.ProductColorDesign(Quantity, Orientation, MainPartRAL, MainPartColorName,
				PocketColorName, PocketRAL)
	select Quantity, Orientation, MainPartRAL, MainPartColorName, PocketColorName, PocketRAL
	from dbo.ProductColorDesign
	where Id = @productColorDesignId
	set @NewProductColorDesignId = SCOPE_IDENTITY();
	insert into dbo.ProductStrap(Type, Material, Length, Width, RAL, ColorName, Attachment)
	select Type, Material, Length, Width, RAL, ColorName, Attachment
	from dbo.ProductStrap
	where Id = @productStrapId
	set @NewProductStrapId = SCOPE_IDENTITY();
	insert into dbo.Product(Designation,Motive,EAN,Accessories,
				Design, ProductColorDesignId, ProductDivisionId, ProductStrapId)
	values (@Designation,@Motive,@EAN,@Accessories,@Design,
			@newProductColorDesignId,@productDivisionId,@newProductStrapId)
	select @Id = SCOPE_IDENTITY();
	select @NewProductColorDesignId
	select @NewProductStrapId
end