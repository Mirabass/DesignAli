using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAWebERP1.Models.Product
{
    public class ProductKindModel
    {
        public int Id { get; set; }
        [Display(Name = "Číslo druhu")]
        public int? Number { get; set; }
        [Display(Name = "Název druhu")]
        public string Name { get; set; }
    }
}
