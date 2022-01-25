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

        public IEnumerable<ProductReceiptModel> GetProductReceipts()
        {
            return _db.ProductReceipts
                .Include(pr => pr.Product)
                    .ThenInclude(p => p.ProductDivision);
        }
    }
}
