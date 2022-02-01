using DAERP.BL.Models.Files;
using DAERP.DAL.DataAccess;
using DAERP.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace DAERP.Web.Controllers
{
    public class DNFileController : Controller
    {
        private readonly IDeliveryNoteData _deliveryNoteData;
        public DNFileController(IDeliveryNoteData deliveryNoteData)
        {
            _deliveryNoteData = deliveryNoteData;
        }
        public IActionResult Index(int? pageNumber)
        {
            IEnumerable<DeliveryNoteFileModel> dnFiles = _deliveryNoteData.GetDeliveryNoteFiles();
            int pageSize = 12;
            return View(PaginatedList<DeliveryNoteFileModel>.Create(dnFiles, pageNumber ?? 1, pageSize));
        }
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
            MemoryStream dnFileStreamResult = dnFile.GetDeliveryNoteMemoryStream();
            return new FileStreamResult(dnFileStreamResult, new MediaTypeHeaderValue("application/vnd.ms-excel"))
            {
                FileDownloadName = dnFile.FileName
            };
        }
    }
}
