﻿using DAERP.BL.Models.Files;
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
    public class IssuedInvoiceFileController : Controller
    {
        private readonly IIssuedInvoiceData _issuedInvoiceData;
        public IssuedInvoiceFileController(IIssuedInvoiceData issuedInvoiceData)
        {
            _issuedInvoiceData = issuedInvoiceData;
        }
        [Authorize(Roles = "Admin,Manager")]
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
            IEnumerable<IssuedInvoiceFileModel> issuedInvoiceFiles = _issuedInvoiceData.GetIssuedInvoiceFiles();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                issuedInvoiceFiles = issuedInvoiceFiles.Where(f =>
                    f.FileName.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    f.Finished.ToString().Contains(normalizedSearchString)
                );
            }
            if (issuedInvoiceFiles.Count() > 0)
            {
                string defaultPropToSort = "FileName_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, issuedInvoiceFiles.FirstOrDefault(), defaultPropToSort);
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
                    issuedInvoiceFiles = issuedInvoiceFiles.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    issuedInvoiceFiles = issuedInvoiceFiles.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            return View(PaginatedList<IssuedInvoiceFileModel>.Create(issuedInvoiceFiles, pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Download(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            IssuedInvoiceFileModel issuedInvoiceFile = _issuedInvoiceData.GetIssuedInvoiceFileBy((int)id);
            if (issuedInvoiceFile is null)
            {
                return NotFound();
            }
            MemoryStream issuedInvoiceFileStreamResult = issuedInvoiceFile.GetNoteMemoryStream();
            string fileName = issuedInvoiceFile.FileName;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            return new FileStreamResult(issuedInvoiceFileStreamResult, new MediaTypeHeaderValue("application/vnd.ms-excel"))
            {
                FileDownloadName = fileName
            };
        }

        // GET-Upload
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Upload(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            IssuedInvoiceFileModel iiFile = _issuedInvoiceData.GetIssuedInvoiceFileBy((int)id);
            if (iiFile == null)
            {
                return NotFound();
            }
            return View(iiFile);
        }

        // POST-Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Upload(IssuedInvoiceFileModel updatedIssuedInvoiceFile)
        {
            if (ModelState.IsValid)
            {
                IssuedInvoiceFileModel oldIssuedInvoiceFile = _issuedInvoiceData.GetIssuedInvoiceFileBy(updatedIssuedInvoiceFile.Id);
                using MemoryStream memoryStream = new MemoryStream();
                var file = Request.Form.Files.FirstOrDefault();
                await file.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2097152) // 2 MB
                {
                    oldIssuedInvoiceFile.ExcelFile = memoryStream.ToArray();
                    await _issuedInvoiceData.UpdateFileAsync(oldIssuedInvoiceFile);
                    return RedirectToAction("Index");                    
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large");
                }
            }
            return View(updatedIssuedInvoiceFile);
        }

        [Authorize(Roles = "Admin,Manager")]
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
            IssuedInvoiceFileModel issuedInvoiceFile = _issuedInvoiceData.GetIssuedInvoiceFileBy((int)id);
            if (issuedInvoiceFile is null)
            {
                return NotFound();
            }
            issuedInvoiceFile.Finished = true;
            await _issuedInvoiceData.UpdateFileAsync(issuedInvoiceFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "IssuedInvoiceFile", action = "Index",
                    currentSort = currentSort,
                    sortOrder = sortOrder,
                    currentFilter = currentFilter,
                    searchString = searchString,
                    pageNumber = pageNumber}));
        }
        [Authorize(Roles = "Admin,Manager")]
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
            IssuedInvoiceFileModel issuedInvoiceFile = _issuedInvoiceData.GetIssuedInvoiceFileBy((int)id);
            if (issuedInvoiceFile is null)
            {
                return NotFound();
            }
            issuedInvoiceFile.Finished = false;
            await _issuedInvoiceData.UpdateFileAsync(issuedInvoiceFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new
                {
                    controller = "IssuedInvoiceFile",
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
