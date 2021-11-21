CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Designation nvarchar(50),
	@EAN bigint,
	@Name nvarchar(256),
	@Type nvarchar(256)
AS
begin
	set nocount on;
	update dbo.Product
	set		Designation = @Designation,
			EAN = @EAN,
			Name = @Name,
			Type = @Type
	where Id = @Id
end