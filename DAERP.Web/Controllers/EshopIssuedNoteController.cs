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
    public class EshopIssueNoteController : Controller
    {
        private readonly IEshopIssueNoteData _eshopIssueNoteData;
        private readonly IProductData _productData;
        private readonly IEshopData _eshopData;
        private readonly IProductSelectService _productSelectService;
        public EshopIssueNoteController(IEshopIssueNoteData eshopIssueNoteData,
            IProductData productData, IEshopData eshopData, IProductSelectService productSelectService)
        {
            _eshopIssueNoteData = eshopIssueNoteData;
            _productData = productData;
            _eshopData = eshopData;
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
            IEnumerable<EshopIssueNoteModel> eshopIssueNotes = _eshopIssueNoteData.GetEshopIssueNotes();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                eshopIssueNotes = eshopIssueNotes.Where(ii =>
                    ii.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Eshop.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Eshop.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.DateCreated.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.Amount.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.OperatedSellingPrice.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.ValueWithVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    ii.ValueWithoutVAT.ToString().Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["AmountSum"] = eshopIssueNotes.Select(dn => dn.Amount).Sum();
            ViewData["ValueWithoutVATSum"] = eshopIssueNotes.Select(dn => dn.ValueWithoutVAT).Sum();
            ViewData["ValueWithVATSum"] = eshopIssueNotes.Select(dn => dn.ValueWithVAT).Sum();
            if (eshopIssueNotes.Count() > 0)
            {
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, eshopIssueNotes.FirstOrDefault(), defaultPropToSort);
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
                    eshopIssueNotes = eshopIssueNotes.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    eshopIssueNotes = eshopIssueNotes.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<EshopIssueNoteModel>.Create(eshopIssueNotes, pageNumber ?? 1, pageSize));

        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [HttpPost]
        public IActionResult PostSelectedEshop(PostSelectedViewModel model)
        {
            if (model.SelectedIds.Count() != 1)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create", new RouteValueDictionary(
                new { controller = "EshopIssueNote", action = "Create", eshopId = model.SelectedIds.First() }));
        }
        public IActionResult Create(int? eshopId,
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
            List<ProductModel> products = _productData.GetAllProductsWithChildModelsIncluded().ToList();
            List<SelectedProduct> selectedProducts = _productSelectService.Get(addSelected, removeSelected,
                removeAllSelected, TempData, false);
            DecreaseStockViewBySelectedProducts(products, selectedProducts);
            PaginatedList<ProductModel> paginatedList = StaticHelper.SortAndFilterProductsForSelectPurpose(currentSort, sortOrder, currentFilter,
                searchString, pageNumber, ViewData, products);
            selectedProducts.ForEach(sp => sp.Product = _productData.GetProductWithChildModelsIncludedBy(sp.Product.Id));
            EshopModel eshop = _eshopData.GetEshopBy(eshopId);
            ProductsSelectionViewModel productSelectionViewModel = new ProductsSelectionViewModel()
            {
                Eshop = eshop,
                Products = paginatedList,
                SelectedProducts = selectedProducts
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
        public IActionResult CreatePost()
        {
            int? eshopId = TempData["EshopId"] as int?;
            EshopModel eshop = _eshopData.GetEshopBy(eshopId);
            TempData["EshopId"] = null;
            List<SelectedProduct> selectedProducts = _productSelectService.Get(TempData);
            selectedProducts.ForEach(sdn => {
                sdn.Product = _productData.GetProductWithChildModelsIncludedBy(sdn.Product.Id);
            });
            int? lastOrderThisYear = NoteModel.GetMovementLastOrderThisYear(_eshopIssueNoteData.GetEshopIssueNotes());
            List<EshopIssueNoteModel> eshopIssueNotes = new List<EshopIssueNoteModel>();
            foreach (SelectedProduct selectedProduct in selectedProducts)
            {
                selectedProduct.Product.DecreaseMainStockOf(selectedProduct.Amount);
                EshopIssueNoteModel eshopIssueNote = new EshopIssueNoteModel(
                    selectedProduct.Amount,
                    lastOrderThisYear,
                    selectedProduct.Product,
                    eshop,
                    selectedProduct.Product.ProductPrices.OperatedSellingPrice);
                eshopIssueNotes.Add(eshopIssueNote);
            }
            eshopIssueNotes.ForEach(rn => rn.ClearChildModels());

            // Database:
            _eshopIssueNoteData.AddRangeOfEshopIssueNotes(eshopIssueNotes);

            TempData["SelectedProductsIds"] = null;
            TempData["SelectedProductAmounts"] = null;
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
