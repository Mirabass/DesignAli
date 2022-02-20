using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAERP.BL.Models.Product
{
    public class ProductMaterialModel
    {
        public int Id { get; set; }
        [Display(Name = "Číslo materiálu výrobku")]
        public int? Number { get; set; }
        [Display(Name = "Název materiálu výrobku")]
        public string Name { get; set; }

        public static ProductMaterialModel Map(Dictionary<int, string> productDivisionDataRow, Dictionary<string, int> mapSettings)
        {
            ProductMaterialModel productMaterial = Mapper<ProductMaterialModel>.Map(productDivisionDataRow, mapSettings);
            return productMaterial;
        }
    }
}
