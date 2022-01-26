using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using DAERP.DAL.Services;
using DAERP.Web.Helper;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class MainStockController : Controller
    {
        private readonly IProductReceiptData _productReceiptData;
        private readonly IProductData _productData;
        public MainStockController(IProductReceiptData productReceiptData, IProductData productData)
        {
            _productReceiptData = productReceiptData;
            _productData = productData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index(
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (sortOrder is null)
            {
                sortOrder = currentSort;
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            IEnumerable<ProductReceiptModel> productReceipts = _productReceiptData.GetProductReceipts();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                productReceipts = productReceipts.Where(pr =>
                    pr.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.DateCreated.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.Amount.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.CostPrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.ValueWithVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pr.ValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["AmountSum"] = productReceipts.Select(pr => pr.Amount).Sum();
            ViewData["ValueWithoutVATSum"] = productReceipts.Select(pr => pr.ValueWithoutVAT).Sum();
            ViewData["ValueWithVATSum"] = productReceipts.Select(pr => pr.ValueWithVAT).Sum();
            if (productReceipts.Count() > 0)
            {
                string defaultPropToSort = "Product.Designation";
                Helper.Helper.SetDataForSortingPurposes(ViewData, sortOrder, productReceipts.FirstOrDefault(), defaultPropToSort);
                if (String.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = defaultPropToSort;
                }
                bool descending = false;
                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }
                if (descending)
                {
                    productReceipts = productReceipts.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    productReceipts = productReceipts.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<ProductReceiptModel>.Create(productReceipts, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Create(
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber,
            int? addSelected)
        {
            var var1 = TempData["SelectedProductsIds"];
            var var2 = var1 as int[];
            var var3 = TempData["SelectedProductAmounts"];
            var var4 = var3 as int[];
            TempData.Clear();
            IEnumerable<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded();
            ProductReceiptCreateViewModel newViewModel = new ProductReceiptCreateViewModel()
            {
                Products = PaginatedList<ProductModel>.Create(products, 1, 12),
                SelectedProducts = new List<SelectedProduct>()
                {
                    new SelectedProduct()
                    {
                        Product = products.ElementAt(0),
                        Amount = 2
                    },
                    new SelectedProduct()
                    {
                        Product = products.ElementAt(2),
                        Amount = 1
                    },
                    new SelectedProduct()
                    {
                        Product = products.ElementAt(4),
                        Amount = 3
                    }
                }
            };

            return View(newViewModel);
        }

    }
}
