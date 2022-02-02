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

        private readonly int _productsStartingRow = 31;
        private readonly int _productsEndingRow = 80;
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
        }
        public void ClearChildModels()
        {
            Customer = null;
            DeliveryNotes = null;
        }
        private string CreateFileName()
        {
            return $"DL {DeliveryNoteNumber} - {DeliveryNotes.FirstOrDefault().DateCreated.ToString("dd.MM.yy")} - {Customer.Designation} {Customer.DFName}.xlsx";
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
            ws.Cells["N3"].Value = DeliveryNotes.FirstOrDefault()
                .DateCreated.ToString("dd.MM.yy");
        }

        private void FillCustomerData(ExcelWorksheet ws)
        {
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

        private void FillProductsData(ExcelWorksheet ws)
        {
            const int productDesignationColumnNumber = 3;
            const int productNameColumnNumber = 4;
            const int productTypeColumnNumber = 5;
            const int productAmountColumnNumber = 6;
            const int vatColumnNumber = 8;
            const int productPriceWithoutVATColumnNumber = 9;
            const int vatValueColumnNumber = 10;
            const int productPriceWithVATColumnNumber = 11;
            int row = _productsStartingRow;
            decimal currencyConvertValue = PriceCalculation.CurrencyConvertValue(Customer.CurrencyCode);
            foreach (DeliveryNoteModel deliveryNote in DeliveryNotes)
            {
                ws.Cells[row, productDesignationColumnNumber].Value = deliveryNote.Product.Designation;
                ws.Cells[row, productNameColumnNumber].Value = deliveryNote.Product.ProductDivision.Name;
                ws.Cells[row, productTypeColumnNumber].Value = deliveryNote.Product.ProductDivision.ProductType;
                ws.Cells[row, productAmountColumnNumber].Value = deliveryNote.StartingAmount;
                ws.Cells[row, vatColumnNumber].Value = PriceCalculation.VATbasedOn(Customer.CurrencyCode);
                ws.Cells[row, productPriceWithoutVATColumnNumber].Formula =
                    PriceCalculation.FormulaForDNproductPriceWithoutVAT(
                        deliveryNote.DeliveryNotePrice,
                        currencyConvertValue,
                        Customer.CurrencyCode,
                        Customer.RoundPriceWithVAT);
                ws.Cells[row, productPriceWithVATColumnNumber].Formula =
                    PriceCalculation.FormulaForDNproductPriceWithVAT(
                        ws.Cells[row, productPriceWithoutVATColumnNumber].Address,
                        ws.Cells[row, vatValueColumnNumber].Address,
                        Customer.RoundPriceWithVAT);
                row++;
            }
        }

        private void FinalAdjustments(ExcelWorksheet ws)
        {
            for (int row = _productsStartingRow + DeliveryNotes.Count(); row <= _productsEndingRow; row++)
            {
                ws.Row(row).Hidden = true;
            }
        }
    }
}
