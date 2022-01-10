using DAERP.BL.Models.Product;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface IProductData
    {
        IEnumerable<ProductModel> GetAllProductsChildModelsIncluded();
    }
}