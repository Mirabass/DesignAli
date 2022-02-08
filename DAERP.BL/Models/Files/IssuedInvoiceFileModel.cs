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
    public class IssuedInvoiceFileModel : NoteFileModel
    {
        public IssuedInvoiceFileModel()
        {
        }
        public IssuedInvoiceFileModel(string IssuedInvoiceNumber, CustomerModel customer, IEnumerable<IssuedInvoiceModel> IssuedInvoices, string iiTemplateFilePath)
            :base(IssuedInvoiceNumber, customer, IssuedInvoices, iiTemplateFilePath)
        {
        }

        protected override string CreateFileName()
        {
            return $"FV {NoteNumber} - {Notes.FirstOrDefault().DateCreated.ToString("dd.MM.yy")} - {Customer.Designation} {Customer.DFName}.xlsx";
        }
        protected override void FillMainData(ExcelWorksheet ws)
        {
            ws.Cells["P2"].Value = NoteNumber;
            ws.Cells["P3"].Value = Notes.FirstOrDefault()
                .DateCreated.ToString("dd.MM.yy");
        }
        protected override void FillCustomerData(ExcelWorksheet ws)
        {
            ws.Cells["N7"].Value = Customer.Name;
            ws.Cells["N9"].Value = Customer.DFStreetAndNo;
            ws.Cells["N10"].Value = $"{Customer.DFZIP} - {Customer.DFCity}";
            ws.Cells["N12"].Value = Customer.DFPhone;
            ws.Cells["N13"].Value = Customer.DFMobile;
            ws.Cells["N14"].Value = Customer.DFEmail;
            ws.Cells["N16"].Value = Customer.DFIN;
            ws.Cells["N17"].Value = Customer.DFTIN;
            ws.Cells["P19"].Value = Customer.FVDiscountPercentValue;
            ws.Cells["P22"].Value = DateTime.Now.AddDays(Customer.Maturity).ToString("dd.MM.yy");
            ws.Cells["O84"].Value = Customer.CurrencyCode;
            ws.Cells["L30"].Value = Customer.CurrencyCode;
            ws.Cells["O30"].Value = Customer.CurrencyCode;
        }
        protected override void FillProductsData(ExcelWorksheet ws)
        {
            const int productDesignationColumnNumber = 3;
            const int productNameColumnNumber = 4;
            const int productTypeColumnNumber = 5;
            const int productAmountColumnNumber = 6;
            const int vatColumnNumber = 8;
            const int productPriceWithoutVATColumnNumber = 9;
            int row = _productsStartingRow;
            decimal currencyConvertValue = PriceCalculation.CurrencyConvertValue(Customer.CurrencyCode);
            foreach (IssuedInvoiceModel IssuedInvoice in Notes)
            {
                ws.Cells[row, productDesignationColumnNumber].Value = IssuedInvoice.Product.Designation;
                ws.Cells[row, productNameColumnNumber].Value = IssuedInvoice.Product.ProductDivision.Name;
                ws.Cells[row, productTypeColumnNumber].Value = IssuedInvoice.Product.ProductDivision.ProductType;
                ws.Cells[row, productAmountColumnNumber].Value = IssuedInvoice.Amount;
                ws.Cells[row, vatColumnNumber].Value = PriceCalculation.VATbasedOn(Customer.CurrencyCode);
                ws.Cells[row, productPriceWithoutVATColumnNumber].Formula =
                    PriceCalculation.FormulaForIIProductPriceWithoutVAT(
                        IssuedInvoice.IssuedInvoicePrice,
                        currencyConvertValue);
                row++;
            }
        }
        protected override void FillExtraData(ExcelWorksheet ws)
        {
            List<string> dnNumbers = new();
            foreach (IssuedInvoiceModel IssuedInvoice in Notes)
            {
                dnNumbers.Add(IssuedInvoice.DeliveryNoteNumber);
            }
            dnNumbers = dnNumbers.Distinct().ToList();
            string dnNumebersResult = string.Join(" | ", dnNumbers);
            ws.Cells["D23"].Value = dnNumebersResult;
        }
    }
}
