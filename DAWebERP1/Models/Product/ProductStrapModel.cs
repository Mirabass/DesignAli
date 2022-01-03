using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAWebERP1.Models.Product
{
    public class ProductStrapModel
    {
        [Required]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Type { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Material { get; set; }
        [Column(TypeName = "numeric(5,3)")]
        public decimal? Length { get; set; }
        [Column(TypeName = "numeric(5,3)")]
        public decimal? Width { get; set; }
        [Column(TypeName = "numeric(4)")]
        public int? RAL { get; set; }
        [Column(TypeName = "nvarchar(128)")]
        public string ColorName { get; set; }
        [Column(TypeName = "nvarchar(128)")]
        public string Attachment { get; set; }
    }
}