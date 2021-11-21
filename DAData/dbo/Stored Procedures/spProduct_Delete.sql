CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int
AS
begin
	set nocount on;
	delete from dbo.Product
	where Id = @Id
end
