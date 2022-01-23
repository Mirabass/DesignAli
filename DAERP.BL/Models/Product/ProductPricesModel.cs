using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Product
{
    public class ProductPricesModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Nákladová cena")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Required]
        public decimal OperatedCostPrice { get; set; } = 0;
        [Display(Name = "Prodejní cena")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Required]
        public decimal OperatedSellingPrice { get; set; } = 0;
    }
}
