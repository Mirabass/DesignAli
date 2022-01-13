using DAERP.BL.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models
{
    public class CustomerProductModel
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public CustomerModel Customer { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public ProductModel Product { get; set; }
        public int AmountInStock { get; set; }
    }
}
