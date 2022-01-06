using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAWebERP1.Models.Product
{
    public class ProductMaterialModel
    {
        public int Id { get; set; }
        [Display(Name = "Číslo materiálu výrobku")]
        public int? Number { get; set; }
        [Display(Name = "Název materiálu výrobku")]
        public string Name { get; set; }
    }
}
