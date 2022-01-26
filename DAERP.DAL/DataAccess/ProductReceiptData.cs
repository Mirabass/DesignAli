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
    public class ProductReceiptData : IProductReceiptData
    {
        private ApplicationDbContext _db;
        public ProductReceiptData(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddRangeOfProductReceiptsAsync(List<ProductReceiptModel> productReceipts)
        {
            var receiptsThisYear = _db.ProductReceipts
                .Where(pr => pr.DateCreated.Year == DateTime.Now.Year);
            int? lastOrderThisYear = null;
            if (receiptsThisYear.Any())
            {
               lastOrderThisYear = receiptsThisYear
                .Select(pr => pr.OrderInCurrentYear)
                .Max();
            }
            int newOrderThisYear = (lastOrderThisYear ?? 0) + 1;
            string newNumber = DateTime.Now.ToString("yy") + "-" + CustomOperations.LeadingZeros(newOrderThisYear, 4);
            productReceipts.ForEach(pr =>
            {
                pr.DateCreated = DateTime.Now;
                pr.OrderInCurrentYear = newOrderThisYear;
                pr.Number = newNumber;
                pr.ValueWithoutVAT = pr.Amount * pr.CostPrice;
                pr.ValueWithVAT = PriceCalculation.IncreaseOfVAT(pr.ValueWithoutVAT);
                pr.Product.MainStockAmount += pr.Amount;
                pr.Product.MainStockValue += pr.ValueWithoutVAT;
            });
            _db.ProductReceipts.AddRange(productReceipts);
            _db.SaveChanges();
        }

        public IEnumerable<ProductReceiptModel> GetProductReceipts()
        {
            return _db.ProductReceipts
                .Include(pr => pr.Product)
                    .ThenInclude(p => p.ProductDivision);
        }
    }
}
