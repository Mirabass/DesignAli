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
    public class ProductReceiptModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo PV")]
        public string Number { get; set; }
        public int OrderInCurrentYear { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [Display(Name = "Nákladová cena")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Required]
        [Display(Name = "Hodnota - DPH")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal ValueWithoutVAT { get; set; }
        [Required]
        [Display(Name = "Hodnota + DPH")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal ValueWithVAT { get; set; }
    }
}
