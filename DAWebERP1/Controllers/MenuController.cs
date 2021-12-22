using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAWebERP1.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
