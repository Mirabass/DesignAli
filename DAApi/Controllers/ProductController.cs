using DADataManager.Library.DataAccess;
using DADataManager.Library.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
        [HttpGet("{designationEncoded}")]
        public List<ProductModel> Get(string designationEncoded)
        {
            string designationDecoded = HttpUtility.UrlDecode(designationEncoded,System.Text.Encoding.UTF8);
            return _productData.GetProducts(designationDecoded);
        }
        [HttpPost]
        public IActionResult Post(ProductModel product)
        {
            (int,int,int) ids = _productData.AddProduct(product);
            return Json(new { NewProductId = ids.Item1, NewColorDesignId = ids.Item2, NewStrapId = ids.Item3 });
        }
        [HttpPut]
        public void Put(ProductModel product)
        {
            _productData.Update(product);
        }
        [HttpDelete]
        public void Delete(int id)
        {
            _productData.DeleteProduct(id);
        }
    }
}