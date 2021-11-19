using DADataManager.Library.DataAccess;
using DADataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignAliAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier,Manager,Admin")]
    public class ProductController : Controller
    {
        private readonly IProductData _productData;
        public ProductController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet]
        public List<ProductModel> Get()
        {
            return _productData.GetProducts();
        }
        [HttpPost]
        public void Post(ProductModel product)
        {
            _productData.AddProduct(product);
        }
    }
}