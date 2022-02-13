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
    public abstract class NoteFileModel
    {
        public int Id { get; set; }
        public string NoteNumber { get; set; }
        [Display(Name = "Název souboru")]
        public string FileName { get; set; }
        [Display(Name = "MS Excel soubor")]
        public byte[] ExcelFile { get; set; }
        [Display(Name = "Označit jako dokončený")]
        public bool Finished { get; set; } = false;
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }

        protected readonly string _noteTemplateFilePath;

        protected readonly int _productsStartingRow = 31;
        protected readonly int _productsEndingRow = 80;
        public NoteFileModel()
        {

        }
        public NoteFileModel(string noteNumber, CustomerModel customer, IEnumerable<NoteModel> notes, string templateFilePath)
            :this()
        {
            Customer = customer;
            NoteNumber = noteNumber;
            Notes = notes;
            CustomerId = Customer.Id;
            _noteTemplateFilePath = templateFilePath;
            FileName = CreateFileName();
        }

        public MemoryStream GetNoteMemoryStream()
        {
            return new MemoryStream(ExcelFile);
        }

        public async Task Create()
        {
            using MemoryStream memoryStream = new MemoryStream();
            using FileStream fileStream = File.OpenRead(_noteTemplateFilePath);
            await FillExcelFile(fileStream, memoryStream);
            SaveFileToByteProperty(memoryStream);
        }
        public void ClearChildModels()
        {
            Customer = null;
            Notes = null;
        }
        protected virtual string CreateFileName()
        {
            throw new NotImplementedException();
        }


        private async Task FillExcelFile(FileStream fileStream, MemoryStream memoryStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            await package.LoadAsync(fileStream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();
            FillMainData(ws);
            FillCustomerData(ws);
            FillProductsData(ws);
            FillExtraData(ws);
            FinalAdjustments(ws);
            await package.SaveAsAsync(memoryStream);
        }

        private void SaveFileToByteProperty(MemoryStream memoryStream)
        {
            this.ExcelFile = memoryStream.ToArray();
        }
        protected virtual void FillMainData(ExcelWorksheet ws)
        {
            ws.Cells["N2"].Value = NoteNumber;
            ws.Cells["N3"].Value = Notes.FirstOrDefault()
                .DateCreated.ToString("dd.MM.yy");
        }

        protected virtual void FillCustomerData(ExcelWorksheet ws)
        {
            // performs when delivery note or return note is created
            ws.Cells["K7"].Value = Customer.DFName;
            ws.Cells["K9"].Value = Customer.DFStreetAndNo;
            ws.Cells["K10"].Value = $"{Customer.DFZIP} - {Customer.DFCity}";
            ws.Cells["K12"].Value = Customer.DFPhone;
            ws.Cells["K13"].Value = Customer.DFMobile;
            ws.Cells["K14"].Value = Customer.DFEmail;
            ws.Cells["K16"].Value = Customer.DFIN;
            ws.Cells["K17"].Value = Customer.DFTIN;
            ws.Cells["K19"].Value = Customer.MDName;
            ws.Cells["K20"].Value = Customer.MDContactPerson;
            ws.Cells["K22"].Value = Customer.MDStreetAndNo;
            ws.Cells["K23"].Value = $"{Customer.MDZIP} - {Customer.MDCity}";

            ws.Cells["L84"].Value = Customer.CurrencyCode;
        }

        protected virtual void FillProductsData(ExcelWorksheet ws)
        {
            throw new NotImplementedException();
        }
        protected virtual void FillExtraData(ExcelWorksheet ws)
        {
        }
        private void FinalAdjustments(ExcelWorksheet ws)
        {
            for (int row = _productsStartingRow + Notes.Count(); row <= _productsEndingRow; row++)
            {
                ws.Row(row).Hidden = true;
            }
        }
    }
}
