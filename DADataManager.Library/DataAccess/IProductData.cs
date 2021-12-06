using DADataManager.Library.Models.Product;
using System.Collections.Generic;

namespace DADataManager.Library.DataAccess
{
    public interface IProductData
    {
        /// <summary>
        /// Adds product to database and returns id of new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>id of new product</returns>
        int AddProduct(ProductModel product);
        List<ProductModel> GetProducts();
        void DeleteProduct(int productId);
        void Update(ProductModel product);
        List<ProductDivisionModel> GetProductDivisions();
        List<ProductModel> GetProducts(string designation);
    }
}