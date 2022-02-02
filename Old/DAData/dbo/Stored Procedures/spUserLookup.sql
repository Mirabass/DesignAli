CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
begin
	set nocount on;

	select FirstName, LastName, EmailAdress
	from [dbo].[User]
	where Id = @Id;
end
