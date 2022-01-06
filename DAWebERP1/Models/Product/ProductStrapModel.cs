using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAWebERP1.Models.Product
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
        [Column(TypeName = "numeric(5,3)")]
        public decimal? Length { get; set; }
        [Display(Name = "Šířka")]
        [Column(TypeName = "numeric(5,3)")]
        public decimal? Width { get; set; }
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
    }
}