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
            int? addSelected,
            int? removeSelected,
            int? removeAllSelected)
        {
            List<SelectedProduct> selectedProducts = RetrieveSelectedProduct();
            if (addSelected is not null)
            {
                AddSelected(selectedProducts, addSelected);
            }
            if (removeSelected is not null)
            {
                RemoveSelected(selectedProducts, removeSelected);
            }
            if (removeAllSelected is not null)
            {
                RemoveAllSelected(selectedProducts, removeAllSelected);
            }
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
            IEnumerable<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                products = products.Where(p =>
                    p.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.EAN.ToString().Contains(normalizedSearchString) ||
                    p.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            if (products.Count() > 0)
            {
                string defaultPropToSort = "Designation";
                Helper.Helper.SetDataForSortingPurposes(ViewData, sortOrder, products.FirstOrDefault(), defaultPropToSort);
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
                    products = products.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    products = products.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            ProductReceiptCreateViewModel productReceiptCreateViewModel = new ProductReceiptCreateViewModel()
            {
                Products = PaginatedList<ProductModel>.Create(products, pageNumber ?? 1, pageSize),
                SelectedProducts = selectedProducts
            };

            return View(productReceiptCreateViewModel);
        }

        private void RemoveAllSelected(List<SelectedProduct> selectedProducts, int? removeAllSelected)
        {
            if (selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)removeAllSelected))
            {
                SelectedProduct selectedProduct = selectedProducts.Where(sp => sp.Product.Id == (int)removeAllSelected).FirstOrDefault();
                selectedProducts.Remove(selectedProduct);
            }
            else
            {
                // TODO: add error
            }
        }

        private void RemoveSelected(List<SelectedProduct> selectedProducts, int? removeSelected)
        {
            if (selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)removeSelected))
            {
                SelectedProduct selectedProduct = selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault();
                if (selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault().Amount == 1)
                {
                    selectedProducts.Remove(selectedProduct);
                }
                else
                {
                    selectedProducts.Where(sp => sp.Product.Id == (int)removeSelected).FirstOrDefault().Amount -= 1;
                }
            }
            else
            {
                // TODO: add error
            }
        }

        private void AddSelected(List<SelectedProduct> selectedProducts, int? addSelected)
        {
            if (selectedProducts.Select(sp => sp.Product.Id).ToList().Contains((int)addSelected))
            {
                selectedProducts.Where(sp => sp.Product.Id == (int)addSelected).FirstOrDefault().Amount += 1;
            }
            else
            {
                ProductModel product = _productData.GetProductBy(addSelected);
                selectedProducts.Add(new SelectedProduct()
                {
                    Product = product,
                    Amount = 1
                });
            }
        }

        private List<SelectedProduct> RetrieveSelectedProduct()
        {
            List<SelectedProduct> selectedProducts = new List<SelectedProduct>();
            var selectedProductsIds = TempData["SelectedProductsIds"] as int[];
            var selectedProductAmounts = TempData["SelectedProductAmounts"] as int[];
            if (selectedProductsIds is not null)
            {
                for (int i = 0; i < selectedProductsIds.Length; i++)
                {
                    int id = selectedProductsIds[i];
                    int amount = selectedProductAmounts[i];
                    ProductModel product = _productData.GetProductBy(id);
                    SelectedProduct selectedProduct = new SelectedProduct()
                    {
                        Product = product,
                        Amount = amount
                    };
                    selectedProducts.Add(selectedProduct);
                }
            }
            TempData.Clear();
            return selectedProducts;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            List<SelectedProduct> selectedProducts = RetrieveSelectedProduct();
            List<ProductReceiptModel> productReceipts = new List<ProductReceiptModel>();
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
                ProductReceiptModel productReceipt = new ProductReceiptModel()
                {
                    ProductId = selectedProduct.Product.Id,
                    Product = selectedProduct.Product,
                    Amount = selectedProduct.Amount,
                    CostPrice = selectedProduct.Product.ProductPrices.OperatedCostPrice,
                    DateCreated = DateTime.Now
                };
                productReceipts.Add(productReceipt);
            }
            _productReceiptData.AddRangeOfProductReceiptsAsync(productReceipts);
            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
