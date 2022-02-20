using DAERP.DAL.DataAccess;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly ICustomerData _customerData;
        public MenuController(ICustomerData customerData)
        {
            _customerData = customerData;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult CustomerSelect(string controllerToRedirect)
        {
            ViewData["ControllerToRedirect"] = controllerToRedirect;
            MultiDropDownListViewModel model = Helper.StaticHelper.GetModelForCustomerSelect(_customerData);
            if (model.ItemList.Count == 0)
            {
                return View("Error", new ErrorViewModel()
                {

                });
            }
            return View("_CustomerSelect", model);
        }
    }
}
