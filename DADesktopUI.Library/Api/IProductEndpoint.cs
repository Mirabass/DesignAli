using DADesktopUI.Library.Models.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DADesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
        Task PostProduct(ProductModel product);
        Task DeleteProduct(ProductModel product);
        Task UpdateProduct(ProductModel selectedProduct);
        Task<List<ProductDivisionModel>> GetDivisions();
    }
}