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
using System.Threading.Tasks;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class MainStockController : Controller
    {
        private readonly IProductReceiptData _productReceiptData;
        private readonly IProductData _productData;
        private readonly IProductSelectService _productSelectService;
        public MainStockController(IProductReceiptData productReceiptData, IProductData productData, IProductSelectService productSelectService)
        {
            _productReceiptData = productReceiptData;
            _productData = productData;
            _productSelectService = productSelectService;
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
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, productReceipts.FirstOrDefault(), defaultPropToSort);
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
            int? addSelected,
            int? removeSelected,
            int? removeAllSelected)
        {
            List<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded().ToList();
            List<SelectedProduct> selectedProducts = _productSelectService.Get(addSelected, removeSelected,
                removeAllSelected, TempData, false);
            IncreaseStockViewBySelectedProducts(products, selectedProducts);
            PaginatedList<ProductModel> paginatedList = StaticHelper.SortAndFilterProductsForSelectPurpose(currentSort, sortOrder, currentFilter,
                searchString, pageNumber, ViewData, products);
            selectedProducts.ForEach(sp => sp.Product = _productData.GetProductWithChildModelsIncludedBy(sp.Product.Id));
            ProductsSelectionViewModel productSelectionViewModel = new ProductsSelectionViewModel()
            {
                Products = paginatedList,
                SelectedProducts = selectedProducts
            };
            return View(productSelectionViewModel);
        }

        private void IncreaseStockViewBySelectedProducts(List<ProductModel> products, List<SelectedProduct> selectedProducts)
        {
            selectedProducts.ForEach(sp =>
            {
                products.FirstOrDefault(p => p.Id == sp.Product.Id)
                        .IncreaseMainStockOf(sp.Amount);
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            List<SelectedProduct> selectedProducts = _productSelectService.Get(TempData);
            int? lastOrderThisYear = NoteModel.GetMovementLastOrderThisYear(_productReceiptData.GetProductReceipts());
            List<ProductReceiptModel> productReceipts = new List<ProductReceiptModel>();
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
                var costPrice = _productData
                    .GetProductWithChildModelsIncludedBy(selectedProduct.Product.Id)
                    .ProductPrices.OperatedCostPrice;
                ProductReceiptModel productReceipt = new ProductReceiptModel(
                    selectedProduct.Product,
                    selectedProduct.Amount,
                    costPrice,
                    lastOrderThisYear);
                productReceipts.Add(productReceipt);
                selectedProduct.Product.IncreaseMainStockOf(productReceipt.Amount, costPrice);
            }

            _productReceiptData.AddRangeOfProductReceipts(productReceipts);
            var editedProducts = selectedProducts.Select(sp => sp.Product);
            _productData.UpdateRangeOfProducts(editedProducts);
            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }

    }
}
