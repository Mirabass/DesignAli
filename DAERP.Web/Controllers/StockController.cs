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
    public class StockController : Controller
    {
        private readonly ICustomerData _customerData;
        public StockController(ICustomerData customerData)
        {
            _customerData = customerData;
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
        public IActionResult PostSelectedCustomer(PostSelectedViewModel model)
        {
            if (model.SelectedIds.Count() != 1)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Read", new RouteValueDictionary(
                new { controller = "Stock", action = "Read", customerId = model.SelectedIds.First() }));
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Read(int customerId,
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CustomerId"] = customerId;
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
            IEnumerable<CustomerProductModel> customerProducts = _customerData.GetCustomerProductsBy(customerId);
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                customerProducts = customerProducts.Where(cp =>
                    cp.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    cp.Product.EAN.ToString().Contains(normalizedSearchString) ||
                    cp.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    cp.Product.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            ViewData["StockSum"] = customerProducts.Select(cp => cp.AmountInStock).Sum();
            ViewData["StockValueSum"] = customerProducts.Select(cp => cp.Value).Sum();
            if (customerProducts.Count() > 0)
            {
                string defaultPropToSort = "Product.Designation";
                Helper.Helper.SetDataForSortingPurposes(ViewData, sortOrder, customerProducts.FirstOrDefault(), defaultPropToSort);
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
                    customerProducts = customerProducts.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
                else
                {
                    customerProducts = customerProducts.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder))
                        .ToList();
                }
            }
            int pageSize = 12;
            return View(PaginatedList<CustomerProductModel>.Create(customerProducts, pageNumber ?? 1, pageSize));
        }

    }
}
