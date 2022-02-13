using DAERP.BL.Models;
using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.Web.Helper;
using System.Collections.Generic;

namespace DAERP.Web.ViewModels
{
    public class ProductsSelectionViewModel
    {
        public PaginatedList<ProductModel> Products { get; set; }
        public List<SelectedProduct> SelectedProducts { get; set; }
        public CustomerModel Customer { get; set; }
        public EshopModel Eshop { get; set; }
    }
    public class SelectedProduct
    {
        public ProductModel Product { get; set; }
        public int Amount { get; set; }
        public bool IsPossibleAdd { get; set; } = true;
    }
}
