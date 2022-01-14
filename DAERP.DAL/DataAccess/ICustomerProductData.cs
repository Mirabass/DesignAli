using DAERP.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface ICustomerProductData
    {
        IEnumerable<CustomerProductModel> GetProductsInStockOfCustomerBy(int? id);
        IEnumerable<CustomerProductModel> GetCustomersWithStockOfProductBy(int? id);
    }
}
