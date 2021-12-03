using DADataManager.Library.DataAccess;
using DADataManager.Library.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier,Manager,Admin")]
    public class ProductDivisionController : Controller
    {
        private readonly IProductData _productData;
        public ProductDivisionController(IProductData productData)
        {
            _productData = productData;
        }
        [HttpGet]
        public List<ProductDivisionModel> Get()
        {
            return _productData.GetProductDivisions();
        }
    }
}
