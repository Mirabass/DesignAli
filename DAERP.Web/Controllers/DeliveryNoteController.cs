using DAERP.BL.Models;
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
        public DeliveryNoteController(IDeliveryNoteData deliveryNoteData, IProductData productData, ICustomerData customerData, ICustomerProductData customerProductData, IProductSelectService productSelectService)
        {
            _deliveryNoteData = deliveryNoteData;
            _productData = productData;
            _customerData = customerData;
            _customerProductData = customerProductData;
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
        public IActionResult Create(int customerId,
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
            IEnumerable<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded();
            products = products.Where(p => p.MainStockAmount > 0);
            PaginatedList<ProductModel> paginatedList = StaticHelper.SortAndFilterProductsForSelectPurpose(currentSort, sortOrder, currentFilter,
                searchString, pageNumber, ViewData, products);
            List<SelectedProduct> selectedProducts = _productSelectService.Get(addSelected, removeSelected,
                removeAllSelected, TempData, true);
            ProductsSelectionViewModel productSelectionViewModel = new ProductsSelectionViewModel()
            {
                Products = paginatedList,
                SelectedProducts = selectedProducts
            };
            ViewData["CustomerId"] = customerId;
            return View(productSelectionViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(int customerId)
        {
            List<SelectedProduct> selectedProducts = _productSelectService.Get(TempData);
            List<DeliveryNoteModel> deliveryNotes = new List<DeliveryNoteModel>();
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
                CustomerProductModel customerProduct = _customerProductData.GetCustomerProductBy(customerId, selectedProduct.Product.Id);
                DeliveryNoteModel deliveryNote = new DeliveryNoteModel()
                {
                    ProductId = selectedProduct.Product.Id,
                    Product = _productData.GetProductWithChildModelsIncludedBy(selectedProduct.Product.Id),
                    CustomerId = customerId,
                    Customer = _customerData.GetCustomerBy(customerId),
                    StartingAmount = selectedProduct.Amount,
                    IssuedInvoicePrice = customerProduct.IssuedInvoicePrice,
                    DeliveryNotePrice = customerProduct.DeliveryNotePrice
                };
                deliveryNotes.Add(deliveryNote);
            }
            _deliveryNoteData.AddRangeOfDeliveryNotes(deliveryNotes);
            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
