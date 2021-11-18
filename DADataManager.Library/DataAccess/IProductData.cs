using DADataManager.Library.Models;
using System.Collections.Generic;

namespace DADataManager.Library.DataAccess
{
    public interface IProductData
    {
        List<ProductModel> GetProducts();
    }
}