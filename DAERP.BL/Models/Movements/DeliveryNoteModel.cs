﻿using DAERP.BL.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Movements
{
    public class DeliveryNoteModel : NoteModel
    {
        [Display(Name = "Číslo DL")]
        public override string Number { get; set; }
        [Required]
        [Display(Name = "Množství")]
        public int StartingAmount { get; set; }
        [Display(Name = "Zbývá")]
        [Required]
        public int Remains { get; set; }
        [Required]
        [Display(Name = "Hodnota - DPH zbývá")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal RemainValueWithoutVAT { get; set; }
        public DeliveryNoteModel()
            :base()
        {

        }
        public DeliveryNoteModel(ProductModel product, CustomerModel customer, int startingAmount, decimal issuedInvoicePrice, decimal deliveryNotePrice, int? lastOrderThisYear)
            :base(product,customer,issuedInvoicePrice,deliveryNotePrice,lastOrderThisYear)
        {
            StartingAmount = startingAmount;
            Fill();
        }

        private void Fill()
        {
            this.Remains = this.StartingAmount;
            this.ValueWithoutVAT = this.StartingAmount * this.DeliveryNotePrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
            this.RemainValueWithoutVAT = this.ValueWithoutVAT;
        }
    }
}
