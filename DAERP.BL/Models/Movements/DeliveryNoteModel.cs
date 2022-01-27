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
    public class DeliveryNoteModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo PV")]
        public string Number { get; set; }
        public int OrderInCurrentYear { get; set; }
        [Required]
        [Display(Name = "Datum")]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
        [ForeignKey("Customer")]
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public CustomerModel Customer { get; set; }
        [Required]
        [Display(Name = "Množství")]
        public int StartingAmount { get; set; }
        [Display(Name = "Zbývá")]
        [Required]
        public int Remains { get; set; }
        [Required]
        [Display(Name = "Cena - FV")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal IssuedInvoicePrice { get; set; }
        [Required]
        [Display(Name = "Cena - DL")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal DeliveryNotePrice { get; set; }
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
        [Required]
        [Display(Name = "Hodnota - DPH zbývá")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal RemainValueWithoutVAT { get; set; }
    }
}
