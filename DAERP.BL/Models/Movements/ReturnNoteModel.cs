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
    public class ReturnNoteModel : NoteModel
    {
        [Display(Name = "Číslo VZ")]
        public override string Number { get; set; }
        [Required]
        [Display(Name = "Množství")]
        public int Amount { get; set; }
        
        public ReturnNoteModel()
        {

        }
        public ReturnNoteModel(ProductModel product, CustomerModel customer, int amount, decimal issuedInvoicePrice, decimal deliveryNotePrice, int? lastOrderThisYear)
        {
            ProductId = product.Id;
            CustomerId = customer.Id;
            Amount = amount;
            IssuedInvoicePrice = issuedInvoicePrice;
            DeliveryNotePrice = deliveryNotePrice;
            Fill();
        }

        private void Fill()
        {
            this.ValueWithoutVAT = this.Amount * this.DeliveryNotePrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
        }
    }
}
