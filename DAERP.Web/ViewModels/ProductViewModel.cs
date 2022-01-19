using DAERP.BL.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAERP.Web.ViewModels
{
    public class ProductViewModel
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Designation { get; set; }
        [Required]
        public long EAN { get; set; }
        [Display(Name = "Název výrobku")]
        [Required]
        public int ProductDivisionId { get; set; }
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
        public ProductImageModel ProductImage { get; set; }
    }
}
