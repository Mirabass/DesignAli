using System;
using System.Collections.Generic;
using System.Text;

namespace DADataManager.Library.Models
{
    public class ProductModel
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        public int Id { get; set; }
        public string Designation { get; set; }
        public long EAN { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}