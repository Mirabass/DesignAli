using DAERP.BL.Models;
using DAERP.DAL.DataAccess;
using DAERP.DAL.Services;
using DAERP.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ICustomerData _customerData;
        private ICustomerProductData _customerProductData;
        public CustomerController(ICustomerData customerData, ICustomerProductData customerProductData)
        {
            _customerData = customerData;
            _customerProductData = customerProductData;
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
            IEnumerable<CustomerModel> customers = _customerData.GetAllCustomers();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                customers = customers.Where(c =>
                    c.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    c.DFName.Normalize(System.Text.NormalizationForm.FormD).ToString().Contains(normalizedSearchString) ||
                    c.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    c.SFName.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    c.MDName.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            if (customers.Count() > 0)
            {
                string defaultPropToSort = "Designation";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, customers.FirstOrDefault(), defaultPropToSort);
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
                    customers = customers.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    customers = customers.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            return View(PaginatedList<CustomerModel>.Create(customers, pageNumber ?? 1, pageSize));
        }
        // GET-Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }
        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                customer.DateCreated = System.DateTime.Today;
                customer.DateLastModified = System.DateTime.Today;
                await _customerData.AddCustomerAsync(customer);
                await _customerData.UpdateCustomerProductsPrices(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        // Get-Delete
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            CustomerModel customer = _customerData.GetCustomerBy(Id);
            if (customer == null)
            {
                return NotFound();
            }
            List<CustomerProductModel> productsInStock = _customerProductData.GetProductsInStockOfCustomerWithChildModelsIncludedBy(Id).ToList();
            if (productsInStock.Count > 0)
            {
                string productsInStockMessage = "";
                productsInStock.ForEach(p =>
                {
                    productsInStockMessage += p.Product.Designation + ": " + p.AmountInStock + "\n";
                });
                return Content("Není možné smazat tohoto odběratele, protože má naskladněny následující výrobky:\n" + productsInStockMessage);
            }
            return View(customer);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeletePost(int? Id)
        {
            CustomerModel customer = _customerData.GetCustomerBy(Id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                _customerData.RemoveCustomer(customer);
                return RedirectToAction("Index");
            }
        }
        // GET-Update
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            CustomerModel customer = _customerData.GetCustomerBy(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                CustomerModel oldCustomer = _customerData.GetCustomerBy(customer.Id);
                customer.DateCreated = oldCustomer.DateCreated;
                customer.DateLastModified = System.DateTime.Today;
                _customerData.UpdateCustomer(customer);
                await _customerData.UpdateCustomerProductsPrices(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}
