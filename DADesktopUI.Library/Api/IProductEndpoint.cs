using DADesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DADesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}