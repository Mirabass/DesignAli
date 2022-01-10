using DAERP.BL.Models.Product;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface IProductData
    {
        IEnumerable<ProductModel> GetAllProductsWithChildModelsIncluded();
        IEnumerable<ProductDivisionModel> GetAllProductDivisions();
        ProductDivisionModel GetProductDivisionBy(int productDivisionId);
        void AddProduct(ProductModel product);
        ProductModel GetProductBy(int? id);
        ProductModel GetProductWithChildModelsIncludedBy(int? id);
        void RemoveProduct(ProductModel product);
        ProductDivisionModel GetProductDivisionWithChildModelsIncludedBy(int id);
        void UpdateProduct(ProductModel updatedProduct);
        string GetProductDivisionNameBy(int productDivisionId);
    }
}