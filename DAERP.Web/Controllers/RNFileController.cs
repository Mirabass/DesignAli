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
    public class RNFileController : Controller
    {
        private readonly IReturnNoteData _returnNoteData;
        public RNFileController(IReturnNoteData returnNoteData)
        {
            _returnNoteData = returnNoteData;
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
            IEnumerable<ReturnNoteFileModel> rnFiles = _returnNoteData.GetReturnNoteFiles();
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                rnFiles = rnFiles.Where(f =>
                    f.FileName.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    f.Finished.ToString().Contains(normalizedSearchString)
                );
            }
            if (rnFiles.Count() > 0)
            {
                string defaultPropToSort = "FileName_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(ViewData, sortOrder, rnFiles.FirstOrDefault(), defaultPropToSort);
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
                    rnFiles = rnFiles.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    rnFiles = rnFiles.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            int pageSize = 12;
            return View(PaginatedList<ReturnNoteFileModel>.Create(rnFiles, pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public IActionResult Download(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            ReturnNoteFileModel rnFile = _returnNoteData.GetReturnNoteFileBy((int)id);
            if (rnFile is null)
            {
                return NotFound();
            }
            MemoryStream rnFileStreamResult = rnFile.GetNoteMemoryStream();
            string fileName = rnFile.FileName;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            return new FileStreamResult(rnFileStreamResult, new MediaTypeHeaderValue("application/vnd.ms-excel"))
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
            ReturnNoteFileModel dnFile = _returnNoteData.GetReturnNoteFileBy((int)id);
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
        public async Task<IActionResult> Upload(ReturnNoteFileModel updatedReturnNoteFile)
        {
            if (ModelState.IsValid)
            {
                ReturnNoteFileModel oldRNFile = _returnNoteData.GetReturnNoteFileBy(updatedReturnNoteFile.Id);
                using MemoryStream memoryStream = new MemoryStream();
                var file = Request.Form.Files.FirstOrDefault();
                await file.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2097152) // 2 MB
                {
                    oldRNFile.ExcelFile = memoryStream.ToArray();
                    await _returnNoteData.UpdateFileAsync(oldRNFile);
                    return RedirectToAction("Index");                    
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large");
                }
            }
            return View(updatedReturnNoteFile);
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
            ReturnNoteFileModel rnFile = _returnNoteData.GetReturnNoteFileBy((int)id);
            if (rnFile is null)
            {
                return NotFound();
            }
            rnFile.Finished = true;
            await _returnNoteData.UpdateFileAsync(rnFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "RNFile", action = "Index",
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
            ReturnNoteFileModel rnFile = _returnNoteData.GetReturnNoteFileBy((int)id);
            if (rnFile is null)
            {
                return NotFound();
            }
            rnFile.Finished = false;
            await _returnNoteData.UpdateFileAsync(rnFile);
            return RedirectToAction("Index", new RouteValueDictionary(
                new
                {
                    controller = "RNFile",
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
