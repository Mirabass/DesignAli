using DAERP.BL.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Movements
{
    public sealed class ProductReceiptModel : MovementModel
    {
        [Required]
        [Display(Name = "Množství")]
        public int Amount { get; set; }
        [Required]
        [Display(Name = "Nákladová cena")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        public ProductReceiptModel()
        {
              
        }
        public ProductReceiptModel(ProductModel product, int amount, decimal costPrice, int? lastOrderThisYear)
            :base(product, lastOrderThisYear)
        {
            Amount = amount;
            CostPrice = costPrice;
            ValueWithoutVAT += amount * costPrice;
            ValueWithVAT += PriceCalculation.IncreaseOfVAT(ValueWithoutVAT);
        }
    }
}
