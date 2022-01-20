using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            var data = new List<MultiDropDownListViewModel> {
            new MultiDropDownListViewModel { Id=1, Name="Bill"},
            new MultiDropDownListViewModel { Id=2, Name="John"},
            new MultiDropDownListViewModel { Id=3, Name="Doug"},
            new MultiDropDownListViewModel { Id=4, Name="Sugan" },
            new MultiDropDownListViewModel { Id=5, Name="Selina"},
            new MultiDropDownListViewModel { Id=6, Name="Sylvia"},
            new MultiDropDownListViewModel { Id=7, Name="Ubina" }
            };
            MultiDropDownListViewModel model = new();
            model.ItemList = data.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult PostSelectedValues(PostSelectedViewModel model)
        {
            return View();
        }
    }
}
