using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DADesktopUI.Library.Models.Product
{
    public class ProductModel //: IPrototype<ProductModel>
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Designation { get; set; }
        public long EAN { get; set; }
        //public ProductDivisionModel ProductDivision { get; set; }
        //public ProductColorDesignModel ProductColorDesign { get; set; }
        //public ProductStrapModel ProductStrap { get; set; }
        public int Design { get; set; }
        public string Motive { get; set; }
        public string Accessories { get; set; }

        //public ProductModel CreateDeepCopy()
        //{
        //    var product = (ProductModel)MemberwiseClone();
        //    product.ProductDivision = ProductDivision.CreateDeepCopy();
        //    product.ProductColorDesign = ProductColorDesign.CreateDeepCopy();
        //    product.ProductStrap = ProductStrap.CreateDeepCopy();
        //    return product;
        //}
    }
}