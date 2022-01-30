using DAERP.BL.Models;
using DAERP.BL.Models.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface ICustomerProductData
    {
        IEnumerable<CustomerProductModel> GetProductsInStockOfCustomerWithChildModelsIncludedBy(int? id);
        IEnumerable<CustomerProductModel> GetCustomersWithStockOfProductWithChildModelsIncludedBy(int? id);
        IEnumerable<CustomerProductModel> GetProductsInStockOfCustomersWithChildModelsIncludedBy(int[] customersIds);
        CustomerProductModel GetCustomerProductBy(int customerId, int productId);
        void IncreaseStock(List<DeliveryNoteModel> deliveryNotes);
    }
}
