using DAERP.DAL.DataAccess;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly ICustomerProductData _customerProductData;
        public StockController(ICustomerData customerData, ICustomerProductData customerProductData)
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
            
            return RedirectToAction("Read");
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Read()
        {
            return View();
        }
    }
}
