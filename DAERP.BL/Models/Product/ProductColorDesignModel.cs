﻿using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAERP.BL.Models.Product
{
    public class ProductColorDesignModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Počet")]
        [Column(TypeName = "nvarchar(256)")]
        public string Quantity { get; set; }
        [Display(Name = "Orientace")]
        [Column(TypeName = "nvarchar(256)")]
        public string Orientation { get; set; }
        [Display(Name = "RAL hlavní části")]
        [Column(TypeName = "numeric(4)")]
        public int? MainPartRAL { get; set; }
        [Display(Name = "Barva hlavní části")]
        [Column(TypeName = "nvarchar(128)")]
        public string MainPartColorName { get; set; }
        [NotMapped]
        public string MainPartColorHex { get; set; }
        [Display(Name = "RAL kapsy")]
        [Column(TypeName = "numeric(4)")]
        public int? PocketRAL { get; set; }
        [Display(Name = "Barva kapsy")]
        [Column(TypeName = "nvarchar(256)")]
        public string PocketColorName { get; set; }
        [NotMapped]
        public string PocketColorHex { get; set; }

        public static ProductColorDesignModel Map(Dictionary<int, string> productDataRow, Dictionary<string, int> mapSettings)
        {
            ProductColorDesignModel productColorDesign = Mapper<ProductColorDesignModel>.Map(productDataRow, mapSettings);
            return productColorDesign;
        }
    }
}