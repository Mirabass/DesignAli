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
        private IEshopData _EshopData;
        public EshopController(IEshopData EshopData)
        {
            _EshopData = EshopData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index()
        {
            IEnumerable<EshopModel> Eshops = _EshopData.GetAllEshops();
            return View(Eshops);
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
        public async Task<IActionResult> Create(EshopModel Eshop)
        {
            if (ModelState.IsValid)
            {
                Eshop.DateCreated = System.DateTime.Today;
                Eshop.DateLastModified = System.DateTime.Today;
                await _EshopData.AddEshopAsync(Eshop);
                return RedirectToAction("Index");
            }
            return View(Eshop);
        }
        // Get-Delete
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            EshopModel Eshop = _EshopData.GetEshopBy(Id);
            if (Eshop == null)
            {
                return NotFound();
            }
            return View(Eshop);
        }
        // POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeletePost(int? Id)
        {
            EshopModel Eshop = _EshopData.GetEshopBy(Id);
            if (Eshop == null)
            {
                return NotFound();
            }
            else
            {
                _EshopData.RemoveEshop(Eshop);
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
            EshopModel Eshop = _EshopData.GetEshopBy(Id);
            if (Eshop == null)
            {
                return NotFound();
            }
            return View(Eshop);
        }
        // POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(EshopModel Eshop)
        {
            if (ModelState.IsValid)
            {
                EshopModel oldEshop = _EshopData.GetEshopBy(Eshop.Id);
                Eshop.DateCreated = oldEshop.DateCreated;
                Eshop.DateLastModified = System.DateTime.Today;
                _EshopData.UpdateEshop(Eshop);
                return RedirectToAction("Index");
            }
            return View(Eshop);
        }
    }
}
