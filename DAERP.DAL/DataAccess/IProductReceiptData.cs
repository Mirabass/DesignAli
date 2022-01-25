using DAERP.BL.Models.Movements;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface IProductReceiptData
    {
        IEnumerable<ProductReceiptModel> GetProductReceipts();
    }
}