﻿using DAERP.BL.Models.Product;
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
        // GET-Create
        public IActionResult Create()
        {
            return View();
        }
        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View(productDivision);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
