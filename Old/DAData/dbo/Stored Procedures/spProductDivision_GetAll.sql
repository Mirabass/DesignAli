CREATE PROCEDURE [dbo].[spProductDivision_GetAll]
AS
begin
	set nocount on;

	select d.*, k.*, m.*
		from dbo.ProductDivision d
		inner join dbo.ProductKind k
			on d.ProductKindId = k.Id
		inner join dbo.ProductMaterial m
			on d.ProductMaterialId = m.Id
end