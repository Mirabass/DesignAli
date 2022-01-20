using DAERP.BL.Models.Product;
using System.Collections.Generic;

namespace DAERP.Web.ViewModels
{
    public class ProductCustomersReadViewModel
    {
        public ProductModel Product { get; set; }
        public List<int> CustomerStockAmounts { get; set; }
        public int AmountInSelectedCustomersStocks { get; set; }
    }
}
