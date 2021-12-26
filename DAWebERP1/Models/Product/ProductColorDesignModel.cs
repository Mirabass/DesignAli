using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAWebERP1.Models.Product
{
    public class ProductColorDesignModel
    {
        [Required]
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Orientation { get; set; }
        [Column(TypeName = "numeric(4)")]
        public int MainPartRAL { get; set; }
        [Column(TypeName = "nvarchar(128)")]
        public string MainPartColorName { get; set; }
        [Column(TypeName = "numeric(4)")]
        public int PocketRAL { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string PocketColorName { get; set; }
    }
}