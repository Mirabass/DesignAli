using DAERP.BL.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models
{
    public class CustomerProductModel
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public CustomerModel Customer { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
        [Display(Name = "Množství ve skladu")]
        [Required]
        public int AmountInStock { get; set; } = 0;
        [Required]
        [Display(Name = "Cena - DL")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal DeliveryNotePrice { get; set; } = 0;
        [Required]
        [Display(Name = "Cena - FV")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal IssuedInvoicePrice { get; set; } = 0;
        [Required]
        [Display(Name = "Hodnota")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Value { get; set; } = 0;

        public void IncreaseStock(int amount)
        {
            this.AmountInStock += amount;
            this.Value += amount * this.IssuedInvoicePrice;
        }
    }
}
