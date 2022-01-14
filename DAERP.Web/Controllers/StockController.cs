using Microsoft.AspNetCore.Mvc;

namespace DAERP.Web.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
