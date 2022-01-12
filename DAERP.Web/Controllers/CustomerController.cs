using DAERP.BL.Models;
using DAERP.DAL.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DAERP.Web.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerData _customerData;
        public CustomerController(ICustomerData customerData)
        {
            _customerData = customerData;
        }
        public IActionResult Index()
        {
            IEnumerable<CustomerModel> customers = _customerData.GetAllCustomers();
            return View(customers);
        }
        // GET-Create
        public IActionResult Create()
        {
            return View();
        }
        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                customer.DateCreated = System.DateTime.Today;
                customer.DateLastModified = System.DateTime.Today;
                _customerData.AddCustomer(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        // Get-Delete
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
            return View(customer);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
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
