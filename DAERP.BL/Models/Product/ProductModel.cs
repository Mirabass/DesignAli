using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAERP.BL.Models.Product
{
    public class ProductModel
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo výrobku")]
        [Required]
        [Column(TypeName ="nvarchar(15)")]
        public string Designation { get; set; }
        [Required]
        public long EAN { get; set; }
        //[Required]
        public ProductDivisionModel ProductDivision { get; set; }
        [Required]
        public ProductColorDesignModel ProductColorDesign { get; set; }
        [Required]
        public ProductStrapModel ProductStrap { get; set; }
        [Display(Name = "Provedení")]
        [Required]
        [Column(TypeName = "numeric(4)")]
        public int Design { get; set; }
        [Display(Name = "Motiv")]
        [Column(TypeName = "nvarchar(2048)")]
        public string Motive { get; set; }
        [Display(Name = "Doplňky")]
        [Column(TypeName = "nvarchar(256)")]
        public string Accessories { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateLastModified { get; set; }

        public IList<CustomerProductModel> CustomerProducts { get; set; }
        [Display(Name = "Obrázek")]
        public ProductImageModel ProductImage { get; set; }
    }
}