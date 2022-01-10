using DAERP.BL.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAERP.BL;
using DAERP.DAL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using DAERP.Web.ViewModels;
using DAERP.DAL.DataAccess;
using AutoMapper;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductData _productData;
        private readonly IColorProvider _colorProvider;
        private readonly IMapper _mapper;

        public ProductController(IColorProvider colorProvider, IProductData productData, IMapper mapper)
        {
            _colorProvider = colorProvider;
            _productData = productData;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded();
            foreach (ProductModel product in products)
            {
                product.ProductColorDesign.MainPartColorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.MainPartRAL);
                product.ProductColorDesign.PocketColorHex = _colorProvider.GetHexFromRal(product.ProductColorDesign.PocketRAL);
                product.ProductStrap.ColorHex = _colorProvider.GetHexFromRal(product.ProductStrap.RAL);
            }
            return View(products);
        }
        // GET-Create
        public IActionResult Create()
        {
            CreateViewBagOfProductNames();
            return View();
        }

        private void CreateViewBagOfProductNames()
        {
            var productDivisions = _productData.GetAllProductDivisions();
            var list = productDivisions.Select(pd =>
                                            new SelectListItem
                                            {
                                                Value = pd.Id.ToString(),
                                                Text = pd.Name
                                            });
            ViewBag.ProductNames = new SelectList(list, "Value", "Text");
        }

        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductDivisionModel selectedProductDivision = _productData.GetProductDivisionBy(productViewModel.ProductDivisionId);
                ProductModel product = _mapper.Map<ProductModel>(productViewModel);
                product.ProductDivision = selectedProductDivision;
                CustomOperations.CreateAndAsignDesignationFor(product);
                product.DateCreated = System.DateTime.Today;
                product.DateLastModified = System.DateTime.Today;
                _productData.AddProduct(product);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
        // Get-Delete
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductModel product = _productData.GetProductBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            ProductModel product = _productData.GetProductWithChildModelsIncludedBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _productData.RemoveProduct(product);
                return RedirectToAction("Index");
            }
        }

        // GET-Update
        public IActionResult Update(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            ProductModel product = _productData.GetProductBy(Id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(product);
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductModel oldProduct = _productData.GetProductWithChildModelsIncludedBy(productViewModel.Id);
                ProductDivisionModel updatedProductDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(productViewModel.ProductDivisionId);
                ProductModel updatedProduct = _mapper.Map<ProductModel>(productViewModel);
                updatedProduct.ProductDivision = updatedProductDivision;
                updatedProduct.DateCreated = oldProduct.DateCreated;
                CustomOperations.CreateAndAsignDesignationFor(updatedProduct);
                updatedProduct.DateLastModified = System.DateTime.Today;
                _productData.UpdateProduct(updatedProduct);
                return RedirectToAction("Index");
            }
            CreateViewBagOfProductNames();
            return View(productViewModel);
        }
    }
}
