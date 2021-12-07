using System;
using System.Collections.Generic;
using System.Text;

namespace DADesktopUI.Library.Models.Product
{
    public class ProductMaterialModel : IPrototype<ProductMaterialModel>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }

        public ProductMaterialModel CreateDeepCopy()
        {
            return (ProductMaterialModel)MemberwiseClone();
        }
    }
}
