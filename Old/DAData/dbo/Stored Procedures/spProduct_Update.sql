CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int output,
	@Designation nvarchar(15),
	@Motive nvarchar(2048),
	@EAN bigint,
	@Accessories nvarchar(256),
	@Design int,
	@ColorDesignQuantity smallint,
	@ColorDesignOrientation nvarchar(256),
	@ColorDesignMainPartRAL int,
	@ColorDesignMainPartColorName nvarchar(128),
	@ColorDesignPocketColorName nvarchar(128),
	@ColorDesignPocketRAL int,
	@StrapType nvarchar(256),
	@StrapMaterial nvarchar(256),
	@StrapLength numeric(5,3),
	@StrapWidth numeric(5,3),
	@StrapRAL numeric(4),
	@StrapColorName nvarchar(128),
	@StrapAttachment nvarchar(128)
AS
begin
	set nocount on;
	update dbo.Product
	set Designation = @Designation, Motive = @Motive, EAN = @EAN,
		Accessories = @Accessories, Design = @Design
	where Id = @Id
	update cd
	set cd.Quantity = @ColorDesignQuantity, cd.Orientation = @ColorDesignOrientation,
		cd.MainPartRAL = @ColorDesignMainPartRAL, cd.MainPartColorName = @ColorDesignMainPartColorName,
		cd.PocketColorName = @ColorDesignPocketColorName, cd.PocketRAL = @ColorDesignPocketRAL
	from dbo.Product p
		inner join dbo.ProductColorDesign cd
			on p.ProductColorDesignId = cd.Id
	where p.Id = @Id
	update s
	set s.Type = @StrapType, s.Material = @StrapMaterial, s.Length = @StrapLength,
		s.Width = @StrapWidth, s.RAL = @StrapRAL, s.ColorName = @StrapColorName,
		s.Attachment = @StrapAttachment
	from dbo.Product p
		inner join dbo.ProductStrap s
			on p.ProductStrapId = s.Id
	where p.Id = @Id
end