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
    public abstract class MovementModel
    {
        [Required]
        public int Id { get; set; }
        public virtual string Number { get; set; }
        public int OrderInCurrentYear { get; set; }
        [Required]
        [Display(Name = "Datum")]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
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
        public MovementModel()
        {
        }
        public MovementModel(ProductModel product, int? lastOrderThisYear)
        {
            ProductId = product.Id;
            Fill(lastOrderThisYear);
        }

        private void Fill(int? lastOrderThisYear)
        {
            int newOrderThisYear = (lastOrderThisYear ?? 0) + 1;
            string newNumber = DateTime.Now.ToString("yy") + "-" + CustomOperations.LeadingZeros(newOrderThisYear, 4);
            this.DateCreated = DateTime.Now.Date;
            this.OrderInCurrentYear = newOrderThisYear;
            this.Number = newNumber;
        }

        public virtual void ClearChildModels()
        {
            Product = null;
        }
        public static int? GetMovementLastOrderThisYear(IEnumerable<NoteModel> allOtherNoteModels)
        {
            var notesThisYear = allOtherNoteModels.Where(pr => pr.DateCreated.Year == DateTime.Now.Year);
            int? lastOrderThisYear = null;
            if (notesThisYear.Any())
            {
                lastOrderThisYear = notesThisYear
                 .Select(pr => pr.OrderInCurrentYear)
                 .Max();
            }
            return lastOrderThisYear;
        }




    }
}
