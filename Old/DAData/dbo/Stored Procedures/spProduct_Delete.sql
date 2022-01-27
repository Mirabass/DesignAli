CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int
AS
begin
	set nocount on;

	declare @StrapId as int, @ColorDesignId as int
	set @StrapId = (
			select p.ProductColorDesignId
			from dbo.Product p
			where Id = @Id
		);
	set @ColorDesignId = (
		select p.ProductColorDesignId
			from dbo.Product p
			where Id = @Id
		);
	delete from dbo.Product
	where Id = @Id
	delete from dbo.ProductColorDesign
	where Id = @ColorDesignId
	delete from dbo.ProductStrap
	where Id = @StrapId
end
