using DAERP.BL.Models;
using DAERP.DAL.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class EshopController : Controller
    {
        private IEshopData _eshopData;
        public EshopController(IEshopData eshopData)
        {
            _eshopData = eshopData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index()
        {
            IEnumerable<EshopModel> eshops = _eshopData.GetAllEshops();
            return View(eshops);
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
        public async Task<IActionResult> Create(EshopModel eshop)
        {
            if (ModelState.IsValid)
            {
                eshop.DateCreated = System.DateTime.Today;
                eshop.DateLastModified = System.DateTime.Today;
                await _eshopData.AddEshopAsync(eshop);
                return RedirectToAction("Index");
            }
            return View(eshop);
        }
        // Get-Delete
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            EshopModel eshop = _eshopData.GetEshopBy(Id);
            if (eshop == null)
            {
                return NotFound();
            }
            return View(eshop);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeletePost(int? Id)
        {
            EshopModel eshop = _eshopData.GetEshopBy(Id);
            if (eshop == null)
            {
                return NotFound();
            }
            else
            {
                _eshopData.RemoveEshop(eshop);
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
            EshopModel eshop = _eshopData.GetEshopBy(Id);
            if (eshop == null)
            {
                return NotFound();
            }
            return View(eshop);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(EshopModel eshop)
        {
            if (ModelState.IsValid)
            {
                EshopModel oldEshop = _eshopData.GetEshopBy(eshop.Id);
                eshop.DateCreated = oldEshop.DateCreated;
                eshop.DateLastModified = System.DateTime.Today;
                _eshopData.UpdateEshop(eshop);
                return RedirectToAction("Index");
            }
            return View(eshop);
        }
    }
}
