using DAERP.BL.Models;
using DAERP.DAL.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            IEnumerable<CustomerModel> customers = _customerData.GetAllCustomers();
            return View(customers);
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
        public IActionResult Update(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                CustomerModel oldCustomer = _customerData.GetCustomerBy(customer.Id);
                customer.DateCreated = oldCustomer.DateCreated;
                customer.DateLastModified = System.DateTime.Today;
                _customerData.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}
