﻿using DAERP.BL.Models.Product;
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

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductData _productData;
        private readonly IColorProvider _colorProvider;

        public ProductController(IColorProvider colorProvider, IProductData productData)
        {
            _colorProvider = colorProvider;
            _productData = productData;
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
                ProductModel product = new ProductModel()
                {
                    ProductColorDesign = productViewModel.ProductColorDesign,
                    ProductStrap = productViewModel.ProductStrap,
                    ProductDivision = selectedProductDivision,
                    Design = productViewModel.Design,
                    EAN = productViewModel.EAN,
                    Motive = productViewModel.Motive,
                    Accessories = productViewModel.Accessories
                };
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
            CreateViewBagOfProductNames();
            return View(product);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductModel updatedProduct)
        {
            //if (ModelState.IsValid)
            //{
            ProductModel oldProduct = _productData.GetProductWithChildModelsIncludedBy(updatedProduct.Id);
            ProductDivisionModel updatedProductDivision = _productData.GetProductDivisionWithChildModelsIncludedBy(updatedProduct.ProductDivision.Id);
            updatedProduct.ProductDivision = updatedProductDivision;
            updatedProduct.DateCreated = oldProduct.DateCreated;
            CustomOperations.CreateAndAsignDesignationFor(updatedProduct);
            updatedProduct.DateLastModified = System.DateTime.Today;
            _productData.UpdateProduct(updatedProduct);
            return RedirectToAction("Index");
            //}
            //CreateViewBagOfProductNames();
            //return View(product);
        }
    }
}
