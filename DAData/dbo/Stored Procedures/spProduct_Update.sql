CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Designation nvarchar(50),
	@EAN bigint
AS
begin
	set nocount on;
	update dbo.Product
	set		Designation = @Designation,
			EAN = @EAN
	where Id = @Id
end