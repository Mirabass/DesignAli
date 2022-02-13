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
    public class EshopIssueNoteModel : MovementModel
    {
        [Required]
        [Display(Name = "Množství")]
        public int Amount { get; set; }
        [Required]
        [ForeignKey("Eshop")]
        public int EshopId { get; set; }
        [Required]
        public EshopModel Eshop { get; set; }
        [Display(Name = "Cena - VE")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal OperatedSellingPrice { get; set; }
        public EshopIssueNoteModel() { }
        public EshopIssueNoteModel(int amount, int? lastOrderThisYear, ProductModel product, EshopModel eshop, decimal operatedSellingPrice)
        : base(product, lastOrderThisYear)
        {
            OperatedSellingPrice = operatedSellingPrice;
            EshopId = eshop.Id;
            Eshop = eshop;
            Amount = amount;
            Fill();
        }

        private void Fill()
        {
            this.ValueWithoutVAT = this.Amount * this.OperatedSellingPrice;
            this.ValueWithVAT = PriceCalculation.IncreaseOfVAT(this.ValueWithoutVAT);
        }
        public override void ClearChildModels()
        {
            base.ClearChildModels();
            Eshop = null;
        }
    }
}
