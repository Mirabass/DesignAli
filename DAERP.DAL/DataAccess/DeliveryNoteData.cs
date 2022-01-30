using DAERP.BL;
using DAERP.BL.Models.Movements;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class DeliveryNoteData : IDeliveryNoteData
    {
        private ApplicationDbContext _db;
        public DeliveryNoteData(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddRangeOfDeliveryNotes(List<DeliveryNoteModel> deliveryNotes)
        {
            var deliveryThisYear = _db.DeliveryNotes.AsNoTracking()
                .Where(pr => pr.DateCreated.Year == DateTime.Now.Year);
            int? lastOrderThisYear = null;
            if (deliveryThisYear.Any())
            {
                lastOrderThisYear = deliveryThisYear
                 .Select(pr => pr.OrderInCurrentYear)
                 .Max();
            }
            int newOrderThisYear = (lastOrderThisYear ?? 0) + 1;
            string newNumber = DateTime.Now.ToString("yy") + "-" + CustomOperations.LeadingZeros(newOrderThisYear, 4);
            deliveryNotes.ForEach(dn =>
            {
                dn.DateCreated = DateTime.Now.Date;
                dn.OrderInCurrentYear = newOrderThisYear;
                dn.Number = newNumber;
                dn.Remains = dn.StartingAmount;
                dn.ValueWithoutVAT = dn.StartingAmount * dn.DeliveryNotePrice;
                dn.ValueWithVAT = PriceCalculation.IncreaseOfVAT(dn.ValueWithoutVAT);
                dn.Product.MainStockAmount -= dn.StartingAmount;
                dn.Product.MainStockValue -= dn.StartingAmount * dn.Product.ProductPrices.OperatedCostPrice;
             });
            _db.DeliveryNotes.AddRange(deliveryNotes);
            _db.SaveChanges();
        }

        public IEnumerable<DeliveryNoteModel> GetDeliveryNotes()
        {
            return _db.DeliveryNotes
                .Include(dn => dn.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(dn => dn.Customer).AsNoTracking();
        }
    }
}
