using DAERP.BL.Models.Movements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IProductReceiptData
    {
        IEnumerable<ProductReceiptModel> GetProductReceipts();
        void AddRangeOfProductReceipts(List<ProductReceiptModel> productReceipts);
    }
}