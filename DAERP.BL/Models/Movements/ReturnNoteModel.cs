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
    public sealed class ReturnNoteModel : NoteModel
    {
        [Required]
        [Display(Name = "Množství")]
        public int Amount { get; set; }
        
        public ReturnNoteModel()
        {

        }
        public ReturnNoteModel(ProductModel product, CustomerModel customer, int amount, decimal issuedInvoicePrice, decimal deliveryNotePrice, int? lastOrderThisYear)
        :base(product,customer,issuedInvoicePrice, deliveryNotePrice, lastOrderThisYear)
        {
            Amount = amount;
            Fill();
        }

        private void Fill()
        {
            this.ValueWithoutVAT = this.Amount * this.DeliveryNotePrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
        }
    }
}
