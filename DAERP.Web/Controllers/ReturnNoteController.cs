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
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAERP.Web.Controllers
{
    public class ReturnNoteController : Controller
    {
        private readonly IReturnNoteData _returnNoteData;
        private readonly IDeliveryNoteData _deliveryNoteData;
        private readonly IProductData _productData;
        private readonly ICustomerData _customerData;
        private readonly ICustomerProductData _customerProductData;
        private readonly IDeliveryNoteSelectService _deliveryNoteSelectService;
        private readonly IPathProvider _pathProvider;
        private static string _returnNoteFilePath = "static_files/VZ rr-xxxx - dd.MM.rr - Odběratel - vzor.xlsx";
        public ReturnNoteController(IReturnNoteData returnNoteData, IProductData productData, ICustomerData customerData, ICustomerProductData customerProductData, IDeliveryNoteSelectService deliveryNoteSelectService, IPathProvider pathProvider, IDeliveryNoteData deliveryNoteData)
        {
            _returnNoteData = returnNoteData;
            _productData = productData;
            _customerData = customerData;
            _customerProductData = customerProductData;
            _deliveryNoteSelectService = deliveryNoteSelectService;
            _pathProvider = pathProvider;
            _deliveryNoteData = deliveryNoteData;
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
            IEnumerable<ReturnNoteModel> returnNotes = _returnNoteData.GetReturnNotes();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                returnNotes = returnNotes.Where(dn =>
                    dn.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Customer.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Customer.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.DateCreated.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Amount.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.IssuedInvoicePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.DeliveryNotePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.ValueWithVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.ValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["AmountSum"] = returnNotes.Select(dn => dn.Amount).Sum();
            ViewData["ValueWithoutVATSum"] = returnNotes.Select(dn => dn.ValueWithoutVAT).Sum();
            ViewData["ValueWithVATSum"] = returnNotes.Select(dn => dn.ValueWithVAT).Sum();
            if (returnNotes.Count() > 0)
            {
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, returnNotes.FirstOrDefault(), defaultPropToSort);
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
                    returnNotes = returnNotes.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    returnNotes = returnNotes.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<ReturnNoteModel>.Create(returnNotes, pageNumber ?? 1, pageSize));

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
                new { controller = "ReturnNote", action = "Create", customerId = model.SelectedIds.First() }));
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
            // TODO: Zde nenabízet produkty, ale DeliveryNotes!
            List<DeliveryNoteModel> deliveryNotes = _deliveryNoteData.GetDeliveryNotes().ToList();
            deliveryNotes = deliveryNotes.Where(dn => dn.Remains > 0).ToList();
            List<SelectedDeliveryNote> selectedDeliveryNotes = _deliveryNoteSelectService.Get(addSelected,
                removeSelected,
                removeAllSelected,
                TempData,
                true);
            selectedDeliveryNotes.ForEach(sdn => sdn.DeliveryNote.Product = _productData.GetProductWithChildModelsIncludedBy(sdn.DeliveryNote.ProductId));
            DecreaseDeliveryRemainsViewBySelected(deliveryNotes, selectedDeliveryNotes);
            PaginatedList<DeliveryNoteModel> paginatedList =
                StaticHelper.SortAndFilterDeliveryNotesForSelectPurpose(currentSort, sortOrder, currentFilter,
                searchString, pageNumber, ViewData, deliveryNotes);
            DeliveryNotesSelectionViewModel deliveryNoteSelectionViewModel = new DeliveryNotesSelectionViewModel()
            {
                DeliveryNotes = paginatedList,
                SelectedDeliveryNotes = selectedDeliveryNotes,
                Customer = _customerData.GetCustomerBy(customerId)
            };
            return View(deliveryNoteSelectionViewModel);
        }

        private void DecreaseDeliveryRemainsViewBySelected(List<DeliveryNoteModel> deliveryNotes, List<SelectedDeliveryNote> selectedDeliveryNotes)
        {
            selectedDeliveryNotes.ForEach(sdn =>
            {
                deliveryNotes.FirstOrDefault(dn => dn.Id == sdn.DeliveryNote.Id)
                        .DecreaseRemainsOf(sdn.Amount);
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            //int? customerId = TempData["CustomerId"] as int?;
            //CustomerModel customer = _customerData.GetCustomerBy(customerId);
            //TempData["CustomerId"] = null;
            //List<SelectedProduct> selectedProducts = _productSelectService.Get(TempData);
            //int? lastOrderThisYear = NoteModel.GetMovementLastOrderThisYear(_returnNoteData.GetReturnNotes());
            //List<ReturnNoteModel> returnNotes = new List<ReturnNoteModel>();
            //List<CustomerProductModel> influencedCustomerProducts = new List<CustomerProductModel>();
            //foreach (SelectedProduct selectedProduct in selectedProducts)
            //{
            //    CustomerProductModel customerProduct = _customerProductData.GetCustomerProductBy((int)customerId, selectedProduct.Product.Id);
            //    ReturnNoteModel returnNote = new ReturnNoteModel(
            //        selectedProduct.Product,
            //        customer,
            //        selectedProduct.Amount,
            //        customerProduct.IssuedInvoicePrice,
            //        customerProduct.DeliveryNotePrice,
            //        lastOrderThisYear);
            //    returnNotes.Add(returnNote);
            //    var costPrice = _productData.GetProductWithChildModelsIncludedBy(selectedProduct.Product.Id).ProductPrices.OperatedCostPrice;
            //    selectedProduct.Product.DecreaseMainStockOf(returnNote.Amount, costPrice);
            //    customerProduct.IncreaseStock(returnNote.Amount);
            //    influencedCustomerProducts.Add(customerProduct);
            //}
            //var editedProducts = selectedProducts.Select(sp => sp.Product);
            //string dnPath = _pathProvider.MapPath(_returnNoteFilePath);
            //returnNotes.ForEach(dn => dn.Product = _productData.GetProductWithChildModelsIncludedBy(dn.ProductId));
            //ReturnNoteFileModel returnNoteFile = new ReturnNoteFileModel(returnNotes.FirstOrDefault().Number, customer, returnNotes, dnPath);
            //await returnNoteFile.Create();
            //returnNoteFile.ClearChildModels();
            //returnNotes.ForEach(dn => dn.ClearChildModels());

            //// Database:
            //_returnNoteData.AddRangeOfReturnNotes(returnNotes);
            //_productData.UpdateRangeOfProducts(editedProducts);
            //_customerProductData.UpdateRange(influencedCustomerProducts);
            //await _returnNoteData.AddAsync(returnNoteFile);

            //TempData["SelectedProductsIds"] = null;
            //TempData["SelectedProductAmounts"] = null;
            //TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
