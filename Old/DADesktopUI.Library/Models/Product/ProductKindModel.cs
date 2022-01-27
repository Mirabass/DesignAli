﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DADesktopUI.Library.Models.Product
{
    public class ProductKindModel:IPrototype<ProductKindModel>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }

        public ProductKindModel CreateDeepCopy()
        {
            return (ProductKindModel)MemberwiseClone();
        }
    }
}