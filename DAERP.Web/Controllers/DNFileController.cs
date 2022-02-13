using DAERP.BL.Models.Files;
using DAERP.DAL.DataAccess;
using DAERP.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using System;
using System.Linq;
using DAERP.DAL.Services;
using Microsoft.AspNetCore.Authorization;

namespace DAERP.Web.Controllers
{
    [Authorize]
    public class DNFileController : Controller
    {
        private readonly IDeliveryNoteData _deliveryNoteData;
        public DNFileController(IDeliveryNoteData deliveryNoteData)
        {
            _deliveryNoteData = deliveryNoteData;
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Index(
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (sortOrder is null)
            {
                sortOrder = currentSort;
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            IEnumerable<DeliveryNoteFileModel> dnFiles = _deliveryNoteData.GetDeliveryNoteFiles();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                dnFiles = dnFiles.Where(f =>
                    f.FileName.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    f.Finished.ToString().Contains(normalizedSearchString)
                );
            }
            if (dnFiles.Count() > 0)
            {
                string defaultPropToSort = "FileName_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, dnFiles.FirstOrDefault(), defaultPropToSort);
                if (String.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = defaultPropToSort;
                }
                bool descending = false;
                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }
                if (descending)
                {
                    dnFiles = dnFiles.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    dnFiles = dnFiles.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            return View(PaginatedList<DeliveryNoteFileModel>.Create(dnFiles, pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Download(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            DeliveryNoteFileModel dnFile = _deliveryNoteData.GetDeliveryNoteFileBy((int)id);
            if (dnFile is null)
            {
                return NotFound();
            }
            MemoryStream dnFileStreamResult = dnFile.GetNoteMemoryStream();
            string fileName = dnFile.FileName;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            return new FileStreamResult(dnFileStreamResult, new MediaTypeHeaderValue("application/vnd.ms-excel"))
            {
                FileDownloadName = fileName
            };
        }

        // GET-Upload
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Upload(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DeliveryNoteFileModel dnFile = _deliveryNoteData.GetDeliveryNoteFileBy((int)id);
            if (dnFile == null)
            {
                return NotFound();
            }
            return View(dnFile);
        }

        // POST-Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> Upload(DeliveryNoteFileModel updatedDeliveryNoteFile)
        {
            if (ModelState.IsValid)
            {
                DeliveryNoteFileModel oldDNFile = _deliveryNoteData.GetDeliveryNoteFileBy(updatedDeliveryNoteFile.Id);
                using MemoryStream memoryStream = new MemoryStream();
                var file = Request.Form.Files.FirstOrDefault();
                await file.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2097152) // 2 MB
                {
                    oldDNFile.ExcelFile = memoryStream.ToArray();
                    await _deliveryNoteData.UpdateFileAsync(oldDNFile);
                    return RedirectToAction("Index");                    
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large");
                }
            }
            return View(updatedDeliveryNoteFile);
        }

        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> MakeFinished(int? id,
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            DeliveryNoteFileModel dnFile = _deliveryNoteData.GetDeliveryNoteFileBy((int)id);
            if (dnFile is null)
            {
                return NotFound();
            }
            dnFile.Finished = true;
            await _deliveryNoteData.UpdateFileAsync(dnFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "DNFile", action = "Index",
                    currentSort = currentSort,
                    sortOrder = sortOrder,
                    currentFilter = currentFilter,
                    searchString = searchString,
                    pageNumber = pageNumber}));
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> MakeNotFinished(int? id,
            string currentSort,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            DeliveryNoteFileModel dnFile = _deliveryNoteData.GetDeliveryNoteFileBy((int)id);
            if (dnFile is null)
            {
                return NotFound();
            }
            dnFile.Finished = false;
            await _deliveryNoteData.UpdateFileAsync(dnFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new
                {
                    controller = "DNFile",
                    action = "Index",
                    currentSort = currentSort,
                    sortOrder = sortOrder,
                    currentFilter = currentFilter,
                    searchString = searchString,
                    pageNumber = pageNumber
                }));
        }


    }
}
