using System;
using System.Collections.Generic;
using System.Text;

namespace DADataManager.Library.Models.Product
{
    public class ProductModel
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        public int Id { get; set; }
        public string Designation { get; set; }
        public long EAN { get; set; }
        public ProductDivisionModel ProductDivision { get; set; }
        public ProductColorDesignModel ProductColorDesign { get; set; }
        public ProductStrapModel ProductStrap { get; set; }
        public int Design { get; set; }
        public string Motive { get; set; }
        public string Accessories { get; set; }
    }
}