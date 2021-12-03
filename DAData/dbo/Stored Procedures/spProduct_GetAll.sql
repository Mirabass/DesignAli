CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin
	set nocount on;

	select p.*, d.*, m.*, cd.*, k.*, s.*
		from dbo.Product p
		inner join dbo.ProductColorDesign cd
			on p.ProductColorDesignId = cd.Id
		inner join dbo.ProductStrap s
			on p.ProductStrapId = s.Id
		inner join dbo.ProductDivision d
			on p.ProductDivisionId = d.Id
		inner join dbo.ProductKind k
			on d.ProductKindId = k.Id
		inner join dbo.ProductMaterial m
			on d.ProductMaterialId = m.Id
end