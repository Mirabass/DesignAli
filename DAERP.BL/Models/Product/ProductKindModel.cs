using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Product
{
    public class ProductKindModel
    {
        public int Id { get; set; }
        [Display(Name = "Číslo druhu")]
        public int? Number { get; set; }
        [Display(Name = "Název druhu")]
        public string Name { get; set; }

        public static ProductKindModel Map(Dictionary<int, string> productDivisionDataRow, Dictionary<string, int> mapSettings)
        {
            ProductKindModel productKind = Mapper<ProductKindModel>.Map(productDivisionDataRow, mapSettings);
            return productKind;
        }
    }
}
