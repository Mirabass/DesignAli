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
    public class ReturnNoteFileModel : NoteFileModel
    {
        public ReturnNoteFileModel()
        {
        }
        public ReturnNoteFileModel(string returnNoteNumber, CustomerModel customer, IEnumerable<ReturnNoteModel> returnNotes, string rnTemplateFilePath)
            :base(returnNoteNumber, customer, returnNotes, rnTemplateFilePath)
        {
        }

        protected override string CreateFileName()
        {
            return $"VZ {NoteNumber} - {Notes.FirstOrDefault().DateCreated.ToString("dd.MM.yy")} - {Customer.Designation} {Customer.DFName}.xlsx";
        }

        protected override void FillProductsData(ExcelWorksheet ws)
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
            foreach (ReturnNoteModel returnNote in Notes)
            {
                ws.Cells[row, productDesignationColumnNumber].Value = returnNote.Product.Designation;
                ws.Cells[row, productNameColumnNumber].Value = returnNote.Product.ProductDivision.Name;
                ws.Cells[row, productTypeColumnNumber].Value = returnNote.Product.ProductDivision.ProductType;
                ws.Cells[row, productAmountColumnNumber].Value = returnNote.Amount;
                ws.Cells[row, vatColumnNumber].Value = PriceCalculation.VATbasedOn(Customer.CurrencyCode);
                ws.Cells[row, productPriceWithoutVATColumnNumber].Formula =
                    PriceCalculation.FormulaForDNproductPriceWithoutVAT(
                        returnNote.DeliveryNotePrice,
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
        protected override void FillExtraData(ExcelWorksheet ws)
        {
            List<string> dnNumbers = new();
            foreach (ReturnNoteModel returnNote in Notes)
            {
                dnNumbers.Add(returnNote.DeliveryNoteNumber);
            }
            dnNumbers = dnNumbers.Distinct().ToList();
            string dnNumebersResult = string.Join(" | ", dnNumbers);
            ws.Cells["D22"].Value = dnNumebersResult;
        }
    }
}
