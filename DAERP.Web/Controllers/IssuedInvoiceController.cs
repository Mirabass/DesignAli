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
    public class IssuedInvoiceController : Controller
    {
        private readonly IIssuedInvoiceData _issuedInvoiceData;
        private readonly IDeliveryNoteData _deliveryNoteData;
        private readonly IProductData _productData;
        private readonly ICustomerData _customerData;
        private readonly ICustomerProductData _customerProductData;
        private readonly IDeliveryNoteSelectService _deliveryNoteSelectService;
        private readonly IPathProvider _pathProvider;
        private static string _issuedInvoiceFilePath = "static_files/FV rr-xxxx - dd.MM.rr - Odběratel - vzor.xlsx";
        public IssuedInvoiceController(IIssuedInvoiceData issuedInvoiceData, IProductData productData, ICustomerData customerData, ICustomerProductData customerProductData, IDeliveryNoteSelectService deliveryNoteSelectService, IPathProvider pathProvider, IDeliveryNoteData deliveryNoteData)
        {
            _issuedInvoiceData = issuedInvoiceData;
            _productData = productData;
            _customerData = customerData;
            _customerProductData = customerProductData;
            _deliveryNoteSelectService = deliveryNoteSelectService;
            _pathProvider = pathProvider;
            _deliveryNoteData = deliveryNoteData;
        }
        [Authorize(Roles = "Admin,Manager")]
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
            IEnumerable<IssuedInvoiceModel> issuedInvoices = _issuedInvoiceData.GetIssuedInvoices();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                issuedInvoices = issuedInvoices.Where(ii =>
                    ii.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Customer.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Customer.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.DateCreated.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Amount.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.IssuedInvoicePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.DeliveryNotePrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.ValueWithVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.ValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["AmountSum"] = issuedInvoices.Select(dn => dn.Amount).Sum();
            ViewData["ValueWithoutVATSum"] = issuedInvoices.Select(dn => dn.ValueWithoutVAT).Sum();
            ViewData["ValueWithVATSum"] = issuedInvoices.Select(dn => dn.ValueWithVAT).Sum();
            if (issuedInvoices.Count() > 0)
            {
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, issuedInvoices.FirstOrDefault(), defaultPropToSort);
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
                    issuedInvoices = issuedInvoices.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    issuedInvoices = issuedInvoices.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<IssuedInvoiceModel>.Create(issuedInvoices, pageNumber ?? 1, pageSize));

        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult PostSelectedCustomer(PostSelectedViewModel model)
        {
            if (model.SelectedIds.Count() != 1)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create", new RouteValueDictionary(
                new { controller = "IssuedInvoice", action = "Create", customerId = model.SelectedIds.First() }));
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
            List<DeliveryNoteModel> deliveryNotes = _deliveryNoteData.GetDeliveryNotes().ToList();
            List<SelectedDeliveryNote> selectedDeliveryNotes = _deliveryNoteSelectService.Get(addSelected,
                removeSelected,
                removeAllSelected,
                TempData,
                true);
            deliveryNotes.ForEach(dn => dn.Product = _productData.GetProductWithChildModelsIncludedBy(dn.ProductId));
            selectedDeliveryNotes.ForEach(sdn => sdn.DeliveryNote.Product = _productData.GetProductWithChildModelsIncludedBy(sdn.DeliveryNote.ProductId));
            AdjustStocksViewBySelected(deliveryNotes, selectedDeliveryNotes);
            deliveryNotes = deliveryNotes
                .Where(dn => dn.CustomerId == customerId && dn.Remains > 0).ToList();
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

        private void AdjustStocksViewBySelected(List<DeliveryNoteModel> deliveryNotes, List<SelectedDeliveryNote> selectedDeliveryNotes)
        {
            selectedDeliveryNotes.ForEach(sdn =>
            {
                var dn = deliveryNotes.FirstOrDefault(dn => dn.Id == sdn.DeliveryNote.Id);
                if (dn != null)
                {
                    dn.DecreaseRemainsOf(sdn.Amount);
                    dn.Product.ProductCustomers.First(pd => pd.CustomerId == dn.CustomerId).DecreaseStock(sdn.Amount);
                }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            int? customerId = TempData["CustomerId"] as int?;
            CustomerModel customer = _customerData.GetCustomerBy(customerId);
            TempData["CustomerId"] = null;
            List<SelectedDeliveryNote> selectedDeliveryNotes = _deliveryNoteSelectService.Get(TempData);
            selectedDeliveryNotes.ForEach(sdn => {
                sdn.DeliveryNote.Product = _productData.GetProductWithChildModelsIncludedBy(sdn.DeliveryNote.ProductId);
            });
            int? lastOrderThisYear = NoteModel.GetMovementLastOrderThisYear(_issuedInvoiceData.GetIssuedInvoices());
            List<IssuedInvoiceModel> issuedInvoices = new List<IssuedInvoiceModel>();
            List<CustomerProductModel> influencedCustomerProducts = new List<CustomerProductModel>();
            foreach (SelectedDeliveryNote selectedDeliveryNote in selectedDeliveryNotes)
            {
                selectedDeliveryNote.DeliveryNote.Customer = customer;
                selectedDeliveryNote.DeliveryNote.DecreaseRemainsOf(selectedDeliveryNote.Amount);
                CustomerProductModel customerProduct = _customerProductData
                    .GetCustomerProductBy((int)customerId, selectedDeliveryNote.DeliveryNote.ProductId); // customer stock
                IssuedInvoiceModel issuedInvoice = new IssuedInvoiceModel(
                    selectedDeliveryNote.DeliveryNote,
                    selectedDeliveryNote.Amount,
                    lastOrderThisYear);
                issuedInvoice.Product = selectedDeliveryNote.DeliveryNote.Product;
                issuedInvoices.Add(issuedInvoice);
                var costPrice = selectedDeliveryNote.DeliveryNote.Product.ProductPrices.OperatedCostPrice;
                customerProduct.DecreaseStock(issuedInvoice.Amount);
                influencedCustomerProducts.Add(customerProduct); // customer stock
            }
            var editedDeliveryNotes = selectedDeliveryNotes.Select(sdn => sdn.DeliveryNote);
            string iiPath = _pathProvider.MapPath(_issuedInvoiceFilePath);
            IssuedInvoiceFileModel IssuedInvoiceFile = new IssuedInvoiceFileModel(issuedInvoices.FirstOrDefault().Number, customer, issuedInvoices, iiPath);
            await IssuedInvoiceFile.Create();
            IssuedInvoiceFile.ClearChildModels();
            editedDeliveryNotes.ToList().ForEach(edn => edn.ClearChildModels());
            issuedInvoices.ForEach(rn => rn.ClearChildModels());

            // Database:
            _issuedInvoiceData.AddRangeOfIssuedInvoices(issuedInvoices);
            _deliveryNoteData.UpdateRangeOfDeliveryNotes(editedDeliveryNotes);
            _customerProductData.UpdateRange(influencedCustomerProducts);
            await _issuedInvoiceData.AddAsync(IssuedInvoiceFile);

            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
