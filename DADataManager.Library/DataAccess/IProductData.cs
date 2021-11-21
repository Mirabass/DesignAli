using DADataManager.Library.Models;
using System.Collections.Generic;

namespace DADataManager.Library.DataAccess
{
    public interface IProductData
    {
        void AddProduct(ProductModel product);
        List<ProductModel> GetProducts();
        void DeleteProduct(int productId);
        void Update(ProductModel product);
    }
}