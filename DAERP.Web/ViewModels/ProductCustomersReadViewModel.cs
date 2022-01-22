using DAERP.BL.Models.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAERP.Web.ViewModels
{
    public class ProductCustomersReadViewModel
    {
        public ProductModel Product { get; set; }
        public Dictionary<int,int> CustomerStockAmounts { get; set; }
        [Display(Name = "Množství ve skladu všech odběratelů")]
        public int AmountInSelectedCustomersStocks { get; set; }
        public Dictionary<int,string> CustomersNames { get; set; }
    }
}
