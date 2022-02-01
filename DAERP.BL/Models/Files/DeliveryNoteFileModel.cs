using DAERP.BL.Models.Movements;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Files
{
    public class DeliveryNoteFileModel
    {
        public int Id { get; set; }
        public string DeliveryNoteNumber { get; set; }
        [Display(Name = "Název souboru")]
        public string FileName { get; set; }
        [Display(Name = "MS Excel soubor")]
        public byte[] ExcelFile { get; set; }
        [Display(Name = "Označit jako dokončený")]
        public bool Finished { get; set; } = false;
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public IEnumerable<DeliveryNoteModel> DeliveryNotes { get; set; }

        private readonly string _deliveryNoteTemplateFilePath;
        public DeliveryNoteFileModel()
        {
        }
        public DeliveryNoteFileModel(string deliveryNoteNumber, CustomerModel customer, IEnumerable<DeliveryNoteModel> deliveryNotes, string dnTemplateFilePath)
            :this()
        {
            Customer = customer;
            DeliveryNoteNumber = deliveryNoteNumber;
            DeliveryNotes = deliveryNotes;
            CustomerId = Customer.Id;
            _deliveryNoteTemplateFilePath = dnTemplateFilePath;
            FileName = CreateFileName();
        }

        public MemoryStream GetDeliveryNoteMemoryStream()
        {
            return new MemoryStream(ExcelFile);
        }

        public async Task Create()
        {
            using MemoryStream memoryStream = new MemoryStream();
            using FileStream fileStream = File.OpenRead(_deliveryNoteTemplateFilePath);
            await FillExcelFile(fileStream, memoryStream);
            SaveFileToByteProperty(memoryStream);
            //System.Diagnostics.Process.Start(_deliveryNoteTemplateFilePath);
        }
        public void ClearChildModels()
        {
            Customer = null;
            DeliveryNotes = null;
        }
        private string CreateFileName()
        {
            return $"DL {DeliveryNoteNumber} - {DeliveryNotes.FirstOrDefault().DateCreated.ToString("dd.mm.yy")} - {Customer.Designation} {Customer.Name}.xlsx";
        }


        private async Task FillExcelFile(FileStream fileStream, MemoryStream memoryStream)
        {
            // TODO: Create MS Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            await package.LoadAsync(fileStream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();
            FillMainData(ws);
            FillCustomerData(ws);
            FillProductsData(ws);
            FinalAdjustments(ws);
            await package.SaveAsAsync(memoryStream);
        }

        private void SaveFileToByteProperty(MemoryStream memoryStream)
        {
            this.ExcelFile = memoryStream.ToArray();
        }
        private void FillMainData(ExcelWorksheet ws)
        {
            ws.Cells["N2"].Value = DeliveryNoteNumber;
        }

        private void FillCustomerData(ExcelWorksheet ws)
        {
            //throw new NotImplementedException();
        }

        private void FillProductsData(ExcelWorksheet ws)
        {
            //throw new NotImplementedException();
        }

        private void FinalAdjustments(ExcelWorksheet ws)
        {
            //throw new NotImplementedException();
        }
    }
}
