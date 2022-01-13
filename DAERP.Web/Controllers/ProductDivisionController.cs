using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index()
        {
            IEnumerable<ProductDivisionModel> productDivisions = _productData.GetAllProductDivisionsWithChildModelsIncluded();
            return View(productDivisions);
        }
        // GET-Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }
        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(ProductDivisionModel productDivision)
        {
            if (ModelState.IsValid)
            {
                productDivision.DateCreated = System.DateTime.Today;
                productDivision.DateLastModified = System.DateTime.Today;
                _productData.AddProductDivision(productDivision);
                return RedirectToAction("Index");
            }
            return View(productDivision);
        }
        // Get-Delete
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductDivisionModel productDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(Id);
            if (productDivision == null)
            {
                return NotFound();
            }
            List<ProductModel> productsWithThisDivision = _productData.GetProductsBy(productDivision).ToList();
            if (productsWithThisDivision.Count > 0)
            {
                string productDesignations = "";
                foreach (ProductModel product in productsWithThisDivision)
                {
                    productDesignations += product.Designation;
                    productDesignations += "\n";
                }
                return Content("Není možné smazat toto rozdělení výrobku kvůli následujícím výrobkům: \n" + productDesignations);
            }
            return View(productDivision);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeletePost(int? Id)
        {
            ProductDivisionModel productDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(Id);
            if (productDivision == null)
            {
                return NotFound();
            }
            else
            {
                _productData.RemoveProductDivision(productDivision);
                return RedirectToAction("Index");
            }
        }
        // GET-Update
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductDivisionModel productDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(Id);
            if (productDivision == null)
            {
                return NotFound();
            }
            List<ProductModel> productsWithThisDivision = _productData.GetProductsBy(productDivision).ToList();
            if (productsWithThisDivision.Count > 0)
            {
                string productDesignations = "";
                foreach (ProductModel product in productsWithThisDivision)
                {
                    productDesignations += product.Designation;
                    productDesignations += "\n";
                }
                return Content("Není možné upravit toto rozdělení výrobku kvůli následujícím výrobkům: \n" + productDesignations);
            }
            return View(productDivision);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(ProductDivisionModel productDivision)
        {
            if (ModelState.IsValid)
            {
                ProductDivisionModel oldProductDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(productDivision.Id);
                productDivision.DateCreated = oldProductDivision.DateCreated;
                productDivision.DateLastModified = System.DateTime.Today;
                _productData.UpdateProductDivision(productDivision);
                return RedirectToAction("Index");
            }
            return View(productDivision);
        }
    }
}
