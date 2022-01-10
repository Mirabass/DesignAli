using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAERP.Web.Controllers
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
