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
    public class DeliveryNoteFileModel : NoteFileModel
    {
        public DeliveryNoteFileModel()
        {
        }
        public DeliveryNoteFileModel(string deliveryNoteNumber, CustomerModel customer, IEnumerable<DeliveryNoteModel> deliveryNotes, string dnTemplateFilePath)
            : base(deliveryNoteNumber, customer, deliveryNotes, dnTemplateFilePath)
        {
        }
        protected override string CreateFileName()
        {
            return $"DL {NoteNumber} - {Notes.FirstOrDefault().DateCreated.ToString("dd.MM.yy")} - {Customer.Designation} {Customer.DFName}.xlsx";
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
            foreach (DeliveryNoteModel deliveryNote in Notes)
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
    }
}
