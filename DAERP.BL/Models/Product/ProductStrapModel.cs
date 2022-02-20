using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAERP.BL.Models.Product
{
    public class ProductStrapModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Typ")]
        [Column(TypeName = "nvarchar(256)")]
        public string Type { get; set; }
        [Display(Name = "Materiál")]
        [Column(TypeName = "nvarchar(256)")]
        public string Material { get; set; }
        [Display(Name = "Délka")]
        [Column(TypeName = "nvarchar(256)")]
        public string Length { get; set; }
        [Display(Name = "Šířka")]
        [Column(TypeName = "nvarchar(256)")]
        public string Width { get; set; }
        [Column(TypeName = "numeric(4)")]
        public int? RAL { get; set; }
        [Display(Name = "Barva")]
        [Column(TypeName = "nvarchar(128)")]
        public string ColorName { get; set; }
        [NotMapped]
        public string ColorHex { get; set; }
        [Display(Name = "Uchycení")]
        [Column(TypeName = "nvarchar(128)")]
        public string Attachment { get; set; }

        public static ProductStrapModel Map(Dictionary<int, string> productDataRow, Dictionary<string, int> mapSettings)
        {
            ProductStrapModel productStrap = Mapper<ProductStrapModel>.Map(productDataRow, mapSettings);
            return productStrap;
        }
    }
}