using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL
{
    public static class PriceCalculation
    {
        private static readonly decimal _percentageVAT = 21.0m;
        private static readonly decimal _percentageMinimalGain = 22.0m;
        private static readonly decimal _CZKEUR = 25.0m;
        public static decimal DeliveryNotePrice(decimal percentageGain, decimal percentageProvision, decimal costPrice)
        {
            // convert percentages to 0 .. 1:
            percentageGain = percentageGain / 100.0m;
            percentageProvision = percentageProvision / 100.0m;
            decimal minimalGain = _percentageMinimalGain / 100.0m;


            if (percentageGain - percentageProvision > minimalGain)
            {
                return (1 + (percentageGain - percentageProvision)) * costPrice;
            }
            else
            {
                return (1 + minimalGain) * costPrice;
            }
        }

        public static decimal GainPercentValue(decimal operatedCostPrice, decimal operatedSellingPrice)
        {
            if (operatedCostPrice == 0)
            {
                return 0;
            }
            else
            {
                return (operatedSellingPrice / operatedCostPrice - 1) * 100.0m;
            }
        }

        public static decimal IncreaseOfVAT(decimal valueWithoutVAT)
        {
            decimal VAT = _percentageVAT / 100.0m;
            return valueWithoutVAT * (1 + VAT);
        }

        public static decimal IssuedInvoicePrice(decimal deliveryNotePrice, decimal invoiceDiscountPercentValue)
        {
            decimal invoiceDiscount = invoiceDiscountPercentValue / 100.0m;
            return deliveryNotePrice * (1 - invoiceDiscount);
        }

        public static decimal StockValue(int amountInStock, decimal deliveryNotePrice)
        {
            return amountInStock * deliveryNotePrice;
        }

        internal static decimal VATbasedOn(string currencyCode)
        {
            if (currencyCode == "CZK")
            {
                return _percentageVAT;
            }
            else
            {
                return 0.0m;
            }
        }

        internal static decimal CurrencyConvertValue(string currencyCode)
        {
            if (currencyCode == "CZK")
            {
                return 1.0m;
            }
            else if(currencyCode == "EUR")
            {
                return 1.0m / _CZKEUR;
            }
            else
            {
                throw new NotImplementedException($"CurrencyCode: {currencyCode} is not implemented.");
            }
        }

        internal static string FormulaForDNproductPriceWithoutVAT(decimal deliveryNotePrice, decimal currencyConvertValue, string currencyCode, bool roundPriceWithVAT)
        {
            if (roundPriceWithVAT)
            {
                return String.Format("{0:G}*{1:G}", deliveryNotePrice, currencyConvertValue);
            }
            else
            {
                if (currencyCode == "CZK")
                {
                    return String.Format("ROUND({0:G}*{1:G};0)", deliveryNotePrice, currencyConvertValue);
                }
                else if (currencyCode == "EUR")
                {
                    return String.Format("ROUND({0:G}*{1:G};1)", deliveryNotePrice, currencyConvertValue);
                }
                else
                {
                    throw new NotImplementedException($"CurrencyCode: {currencyCode} is not implemented.");
                }
            }
        }

        internal static string FormulaForDNproductPriceWithVAT(string priceWithoutVATAdress, string vatValueAdress, bool roundPriceWithVAT)
        {
            if (roundPriceWithVAT)
            {
                return string.Format("{0}+{1}", priceWithoutVATAdress, vatValueAdress);
            }
            else
            {
                return string.Format("{0}+{1}", priceWithoutVATAdress, vatValueAdress);
            }
        }
    }
}
