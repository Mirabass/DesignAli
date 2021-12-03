CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Id int output,
	@Designation nvarchar(50),
	@EAN bigint
AS
begin
	set nocount on;
	insert into dbo.Product(Designation,EAN)
	values (@Designation,@EAN)

	--select @Id = @@IDENTITY
end