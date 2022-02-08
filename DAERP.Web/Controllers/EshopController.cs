using Microsoft.AspNetCore.Mvc;

namespace DAERP.Web.Controllers
{
    public class EshopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
