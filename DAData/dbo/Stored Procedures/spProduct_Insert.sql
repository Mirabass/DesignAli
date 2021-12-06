CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Id int output,
	@Designation nvarchar(15),
	@Motive nvarchar(2048),
	@EAN bigint,
	@Accessories nvarchar(256),
	@Design int,
	@productColorDesignId int,
	@productDivisionId int,
	@productStrapId int
AS
begin
	set nocount on;
	insert into dbo.Product(Designation,Motive,EAN,Accessories,
	Design, ProductColorDesignId, ProductDivisionId, ProductStrapId)
	values (@Designation,@Motive,@EAN,@Accessories,@Design,
	@productColorDesignId,@productDivisionId,@productStrapId)

	select SCOPE_IDENTITY();
end