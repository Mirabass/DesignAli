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
    }
}
