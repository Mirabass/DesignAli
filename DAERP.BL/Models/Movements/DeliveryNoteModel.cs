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
        public DeliveryNoteModel()
        {

        }
        public DeliveryNoteModel(ProductModel product, CustomerModel customer, int startingAmount, decimal issuedInvoicePrice, decimal deliveryNotePrice, int? lastOrderThisYear)
        {
            ProductId = product.Id;
            CustomerId = customer.Id;
            StartingAmount = startingAmount;
            IssuedInvoicePrice = issuedInvoicePrice;
            DeliveryNotePrice = deliveryNotePrice;
            Fill(lastOrderThisYear);
        }

        private void Fill(int? lastOrderThisYear)
        {
            int newOrderThisYear = (lastOrderThisYear ?? 0) + 1;
            string newNumber = DateTime.Now.ToString("yy") + "-" + CustomOperations.LeadingZeros(newOrderThisYear, 4);
            this.DateCreated = DateTime.Now.Date;
            this.OrderInCurrentYear = newOrderThisYear;
            this.Number = newNumber;
            this.Remains = this.StartingAmount;
            this.ValueWithoutVAT = this.StartingAmount * this.DeliveryNotePrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
            this.RemainValueWithoutVAT = this.ValueWithoutVAT;
        }

        public void ClearChildModels()
        {
            Product = null;
            Customer = null;
        }
    }
}
