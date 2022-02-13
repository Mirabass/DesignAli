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
    public abstract class NoteModel : MovementModel
    {
        
        [ForeignKey("Customer")]
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public CustomerModel Customer { get; set; }
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
        public NoteModel()
        {

        }
        public NoteModel(ProductModel product, CustomerModel customer, decimal issuedInvoicePrice, decimal deliveryNotePrice, int? lastOrderThisYear)
        :base(product,lastOrderThisYear)
        {
            CustomerId = customer.Id;
            IssuedInvoicePrice = issuedInvoicePrice;
            DeliveryNotePrice = deliveryNotePrice;
        }
        public override void ClearChildModels()
        {
            base.ClearChildModels();
            Customer = null;
        }
    }
}
