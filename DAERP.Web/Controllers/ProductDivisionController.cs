using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class ProductDivisionController : Controller
    {
        private readonly IProductData _productData;
        public ProductDivisionController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index()
        {
            IEnumerable<ProductDivisionModel> productDivisions = _productData.GetAllProductDivisionsWithChildModelsIncluded();
            return View(productDivisions);
        }
    }
}
