using DAERP.BL.Models.Files;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DAERP.Web.Controllers
{
    public class DNFileController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<DeliveryNoteFileModel>()
            {
                new DeliveryNoteFileModel()
                {
                    Id = 1,
                    CustomerId = 1,
                    DeliveryNoteNumber = "22-0001"
                }
            });
        }
    }
}
