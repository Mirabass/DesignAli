using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.Web.Helper;
using System.Collections.Generic;

namespace DAERP.Web.ViewModels
{
    public class ProductReceiptCreateViewModel
    {
        public PaginatedList<ProductModel> Products { get; set; }
        public List<SelectedProduct> SelectedProducts { get; set; }
        public string SearchString { get; set; }
        public string CurrentSort { get; set; }
        public string SortOrder { get; set; }
        public string CurrentSearch { get; set; }
        public int? PageNumber { get; set; }
    }
    public class SelectedProduct
    {
        public ProductModel Product { get; set; }
        public int Amount { get; set; }
    }
}
