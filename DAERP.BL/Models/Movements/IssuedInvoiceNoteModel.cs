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
    public sealed class IssuedInvoiceModel : NoteModel
    {
        [Required]
        [Display(Name = "Množství")]
        public int Amount { get; set; }
        public int DeliveryNoteId { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public IssuedInvoiceModel(){}
        public IssuedInvoiceModel(DeliveryNoteModel deliveryNote, int amount, int? lastOrderThisYear)
        :base(deliveryNote.Product, deliveryNote.Customer, deliveryNote.IssuedInvoicePrice, deliveryNote.DeliveryNotePrice, lastOrderThisYear)
        {
            DeliveryNoteId = deliveryNote.Id;
            DeliveryNoteNumber = deliveryNote.Number;
            Amount = amount;
            Fill();
        }

        private void Fill()
        {
            this.ValueWithoutVAT = this.Amount * this.DeliveryNotePrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
        }
        public override void ClearChildModels()
        {
            base.ClearChildModels();
        }
    }
}
