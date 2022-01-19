using DAERP.BL.Models.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IProductData
    {
        IEnumerable<ProductModel> GetAllProductsWithChildModelsIncluded();
        IEnumerable<ProductDivisionModel> GetAllProductDivisions();
        ProductDivisionModel GetProductDivisionBy(int productDivisionId);
        Task AddProduct(ProductModel product);
        ProductModel GetProductBy(int? id);
        ProductModel GetProductWithChildModelsIncludedBy(int? id);
        void RemoveProduct(ProductModel product);
        ProductDivisionModel GetProductDivisionWithChildModelsIncludedBy(int? id);
        IEnumerable<ProductDivisionModel> GetAllProductDivisionsWithChildModelsIncluded();
        void UpdateProduct(ProductModel updatedProduct);
        string GetProductDivisionNameBy(int productDivisionId);
        void AddProductDivision(ProductDivisionModel productDivision);
        void RemoveProductDivision(ProductDivisionModel productDivision);
        void UpdateProductDivision(ProductDivisionModel productDivision);
        IEnumerable<ProductModel> GetProductsBy(ProductDivisionModel productDivision);
        ProductImageModel GetProductImageBy(int? productId);
    }
}