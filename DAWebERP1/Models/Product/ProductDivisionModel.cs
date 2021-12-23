using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DADataManager.Library.Models.Product
{
    public class ProductDivisionModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "numeric(3,0)")]
        public int Number { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string ProductType { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; }
        [Required]
        public ProductKindModel ProductKind { get; set; }
        [Required]
        public ProductMaterialModel ProductMaterial { get; set; }
    }
}