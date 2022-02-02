using DAERP.BL.Models;
using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using DAERP.DAL.Services;
using DAERP.Web.Helper;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class DeliveryNoteController : Controller
    {
        private readonly IDeliveryNoteData _deliveryNoteData;
        private readonly IProductData _productData;
        private readonly ICustomerData _customerData;
        private readonly ICustomerProductData _customerProductData;
        private readonly IProductSelectService _productSelectService;
        private readonly IPathProvider _pathProvider;
        private static string _deliveryNoteFilePath = "static_files/DL rr-xxxx - dd.mm.rr - Odběratel - vzor.xlsx";
        public DeliveryNoteController(IDeliveryNoteData deliveryNoteData, IProductData productData, ICustomerData customerData, ICustomerProductData customerProductData, IProductSelectService productSelectService, IPathProvider pathProvider)
        {
            _deliveryNoteData = deliveryNoteData;
            _productData = productData;
            _customerData = customerData;
            _customerProductData = customerProductData;
            _productSelectService = productSelectService;
            _pathProvider = pathProvider;
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
            IEnumerable<DeliveryNoteModel> deliveryNotes = _deliveryNoteData.GetDeliveryNotes();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                deliveryNotes = deliveryNotes.Where(dn =>
                    dn.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Customer.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Customer.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.DateCreated.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.StartingAmount.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Remains.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.IssuedInvoicePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.DeliveryNotePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.ValueWithVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.ValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.RemainValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["StartingAmountSum"] = deliveryNotes.Select(dn => dn.StartingAmount).Sum();
            ViewData["RemainsSum"] = deliveryNotes.Select(dn => dn.Remains).Sum();
            ViewData["ValueWithoutVATSum"] = deliveryNotes.Select(dn => dn.ValueWithoutVAT).Sum();
            ViewData["ValueWithVATSum"] = deliveryNotes.Select(dn => dn.ValueWithVAT).Sum();
            ViewData["RemainValueWithoutVATSum"] = deliveryNotes.Select(dn => dn.RemainValueWithoutVAT).Sum();
            if (deliveryNotes.Count() > 0)
            {
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, deliveryNotes.FirstOrDefault(), defaultPropToSort);
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
                    deliveryNotes = deliveryNotes.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    deliveryNotes = deliveryNotes.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<DeliveryNoteModel>.Create(deliveryNotes, pageNumber ?? 1, pageSize));

        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult CustomerSelect()
        {
            MultiDropDownListViewModel model = Helper.StaticHelper.GetModelForCustomerSelect(_customerData);
            return View(model);
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [HttpPost]
        public IActionResult PostSelectedCustomer(PostSelectedViewModel model)
        {
            if (model.SelectedIds.Count() != 1)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create", new RouteValueDictionary(
                new { controller = "DeliveryNote", action = "Create", customerId = model.SelectedIds.First() }));
        }
        public IActionResult Create(int? customerId,
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber,
            int? addSelected,
            int? removeSelected,
            int? removeAllSelected
            )
        {
            if (customerId is null)
            {
                customerId = TempData["CustomerId"] as int?;
            } 
            TempData["CustomerId"] = null;
            List<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded().ToList();
            List<SelectedProduct> selectedProducts = _productSelectService.Get(addSelected, removeSelected,
                removeAllSelected, TempData, true);
            selectedProducts.ForEach(sp => sp.Product = _productData.GetProductWithChildModelsIncludedBy(sp.Product.Id));
            DecreaseStockViewBySelectedProducts(products, selectedProducts);
            products = products.Where(p => p.MainStockAmount > 0).ToList();
            PaginatedList<ProductModel> paginatedList =
                StaticHelper.SortAndFilterProductsForSelectPurpose(currentSort, sortOrder, currentFilter,
                searchString, pageNumber, ViewData, products);
            ProductsSelectionViewModel productSelectionViewModel = new ProductsSelectionViewModel()
            {
                Products = paginatedList,
                SelectedProducts = selectedProducts,
                Customer = _customerData.GetCustomerBy(customerId)
            };
            return View(productSelectionViewModel);
        }

        private void DecreaseStockViewBySelectedProducts(List<ProductModel> products, List<SelectedProduct> selectedProducts)
        {
            selectedProducts.ForEach(sp =>
            {
                products.FirstOrDefault(p => p.Id == sp.Product.Id)
                        .DecreaseMainStockOf(sp.Amount);
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            int? customerId = TempData["CustomerId"] as int?;
            CustomerModel customer = _customerData.GetCustomerBy(customerId);
            TempData["CustomerId"] = null;
            List<SelectedProduct> selectedProducts = _productSelectService.Get(TempData);
            int? lastOrderThisYear = GetDeliveryNoteLastOrderThisYear();
            List<DeliveryNoteModel> deliveryNotes = new List<DeliveryNoteModel>();
            List<CustomerProductModel> influencedCustomerProducts = new List<CustomerProductModel>();
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
                CustomerProductModel customerProduct = _customerProductData.GetCustomerProductBy((int)customerId, selectedProduct.Product.Id);
                DeliveryNoteModel deliveryNote = new DeliveryNoteModel(
                    selectedProduct.Product,
                    customer,
                    selectedProduct.Amount,
                    customerProduct.IssuedInvoicePrice,
                    customerProduct.DeliveryNotePrice,
                    lastOrderThisYear);
                deliveryNotes.Add(deliveryNote);
                var costPrice = _productData.GetProductWithChildModelsIncludedBy(selectedProduct.Product.Id).ProductPrices.OperatedCostPrice;
                selectedProduct.Product.DecreaseMainStockOf(deliveryNote.StartingAmount, costPrice);
                customerProduct.IncreaseStock(deliveryNote.StartingAmount);
                influencedCustomerProducts.Add(customerProduct);
            }
            var editedProducts = selectedProducts.Select(sp => sp.Product);
            string dnPath = _pathProvider.MapPath(_deliveryNoteFilePath);
            deliveryNotes.ForEach(dn => dn.Product = _productData.GetProductWithChildModelsIncludedBy(dn.ProductId));
            DeliveryNoteFileModel deliveryNoteFile = new DeliveryNoteFileModel(deliveryNotes.FirstOrDefault().Number, customer, deliveryNotes, dnPath);
            await deliveryNoteFile.Create();
            deliveryNoteFile.ClearChildModels();
            deliveryNotes.ForEach(dn => dn.ClearChildModels());

            // Database:
            _deliveryNoteData.AddRangeOfDeliveryNotes(deliveryNotes);
            _productData.UpdateRangeOfProducts(editedProducts);
            _customerProductData.UpdateRange(influencedCustomerProducts);
            await _deliveryNoteData.AddAsync(deliveryNoteFile);

            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }

        private int? GetDeliveryNoteLastOrderThisYear()
        {
            var deliveriesThisYear = _deliveryNoteData.GetDeliveryNotes()
                    .Where(pr => pr.DateCreated.Year == DateTime.Now.Year);
            int? lastOrderThisYear = null;
            if (deliveriesThisYear.Any())
            {
                lastOrderThisYear = deliveriesThisYear
                 .Select(pr => pr.OrderInCurrentYear)
                 .Max();
            }
            return lastOrderThisYear;
        }
    }
}
