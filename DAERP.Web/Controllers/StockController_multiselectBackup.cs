using DAERP.BL.Models;
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
    public class StockController_multiselectBackup : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly ICustomerProductData _customerProductData;
        public StockController_multiselectBackup(ICustomerData customerData, ICustomerProductData customerProductData)
        {
            _customerData = customerData;
            _customerProductData = customerProductData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index()
        {
            var activeCustomers = _customerData.GetAllCustomers().Where(c => c.State == "A");
            var data = new List<MultiDropDownListViewModel>();
            foreach (var customer in activeCustomers)
            {
                data.Add(new MultiDropDownListViewModel
                {
                    Id = customer.Id,
                    Name = customer.Designation + ": " + customer.Name
                });
            }
            MultiDropDownListViewModel model = new();
            model.ItemList = data.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        [HttpPost]
        public IActionResult PostSelectedCustomers(PostSelectedViewModel model)
        {
            string customerIds = null;
            if (model.SelectedIds is not null)
            {
                customerIds = string.Join(";", model.SelectedIds);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Read", new RouteValueDictionary(
                new { controller = "Stock", action = "Read", customersIds = customerIds }));
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Read(string customersIds,
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CustomersIds"] = customersIds;
            int[] customersIdsInt = null;
            if (customersIds is not null)
            {
                customersIdsInt = customersIds.Split(";").ToList().Select(int.Parse).ToList().ToArray();
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
            IEnumerable<CustomerProductModel> customerProducts = _customerProductData.GetProductsInStockOfCustomersWithChildModelsIncludedBy(customersIdsInt);
            List<ProductCustomersReadViewModel> productCustomersViewModel = Mapper.ToProductCustomerReadViewModelListFrom(customerProducts.ToList());
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                productCustomersViewModel = productCustomersViewModel.Where(pc =>
                    pc.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pc.Product.EAN.ToString().Contains(normalizedSearchString) ||
                    pc.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pc.Product.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    pc.AmountInSelectedCustomersStocks.ToString().Contains(normalizedSearchString)
                ).ToList();
            }
            ViewData["StockAmountSum"] = productCustomersViewModel.Select(vm => vm.AmountInSelectedCustomersStocks).Sum();
            if (productCustomersViewModel.Count > 0)
            {
                string defaultPropToSort = "Product.Designation";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, productCustomersViewModel.FirstOrDefault(), defaultPropToSort);
                Helper.StaticHelper.SetDynamicDataForSortingPurposes(ViewData, sortOrder, productCustomersViewModel.FirstOrDefault().CustomersNames.Values.ToList());
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
                if (DataOperations.IsSortingDynamic(sortOrder, productCustomersViewModel.FirstOrDefault().CustomersNames.Values.ToList()))
                {
                    SortDynamic(productCustomersViewModel, sortOrder, descending);
                }
                else
                {
                    if (descending)
                    {
                        productCustomersViewModel = productCustomersViewModel.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                            .ToList();
                    }
                    else
                    {
                        productCustomersViewModel = productCustomersViewModel.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                            .ToList();
                    }
                }
            }
            int pageSize = 12;
            return View(PaginatedList<ProductCustomersReadViewModel>.Create(productCustomersViewModel, pageNumber ?? 1, pageSize));
        }

        private void SortDynamic(List<ProductCustomersReadViewModel> productCustomersViewModel, string sortOrder, bool descending)
        {
            List<ProductCustomersReadViewModel> sortedList = new List<ProductCustomersReadViewModel>();
            // chci seřadit List<ProductCustomersReadViewModel> podle MNOŽSTVÍ, které je součtem List<int> CustomerStockAmounts
            if (descending)
            {

                //int indexOfCustomerName = productCustomersViewModel.FirstOrDefault().CustomersNames.IndexOf(sortOrder);
            }
            else
            {

            }
        }
    }
}
