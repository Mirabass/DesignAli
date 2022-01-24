using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL
{
    public static class PriceCalculation
    {
        private static readonly decimal _percentageMinimalGain = 22.0m;
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

        public static decimal IssuedInvoicePrice(decimal deliveryNotePrice, decimal invoiceDiscountPercentValue)
        {
            decimal invoiceDiscount = invoiceDiscountPercentValue / 100.0m;
            return deliveryNotePrice * (1 - invoiceDiscount);
        }

        public static decimal StockValue(int amountInStock, decimal deliveryNotePrice)
        {
            return amountInStock * deliveryNotePrice;
        }
    }
}
