CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Id int output,
	@Designation nvarchar(50),
	@EAN bigint,
	@Name nvarchar(256),
	@Type nvarchar(256)
AS
begin
	set nocount on;
	insert into dbo.Product(Designation,EAN,Name,Type)
	values (@Designation,@EAN,@Name,@Type)

	--select @Id = @@IDENTITY
end