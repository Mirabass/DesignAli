using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAERP.BL.Models.Product
{
    public class ProductDivisionModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo rozdělení")]
        [Required]
        [Column(TypeName = "numeric(3,0)")]
        public int Number { get; set; }
        [Display(Name = "Název výrobku")]
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Display(Name = "Typ výrobku")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string ProductType { get; set; }
        [Display(Name = "Poznámka")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; }
        [Required]
        public ProductKindModel ProductKind { get; set; }
        [Required]
        public ProductMaterialModel ProductMaterial { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }

        public static ProductDivisionModel Map(Dictionary<int, string> productDivisionDataRow,
            Dictionary<string, int> mapSettings,
            ProductKindModel productKind,
            ProductMaterialModel productMaterial)
        {
            ProductDivisionModel productDivision = Mapper<ProductDivisionModel>.Map(productDivisionDataRow, mapSettings);
            productDivision.ProductKind = productKind;
            productDivision.ProductMaterial = productMaterial;
            productDivision.DateCreated = DateTime.Now;
            productDivision.DateLastModified = DateTime.Now;
            return productDivision;
        }
    }
}