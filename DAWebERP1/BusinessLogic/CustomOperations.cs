using DAWebERP1.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAWebERP1.BusinessLogic
{
    public static class CustomOperations
    {
        public static string LeadingZeros(int? number, int nDigits)
        {
            if (number is null) { return ""; }
            string numberString = number.ToString();
            int numberLength = numberString.Length;
            if (numberLength >= nDigits)
            {
                return numberString;
            }
            else
            {
                string newNumber = numberString;
                for (int i = 0; i < nDigits - numberLength; i++)
                {
                    newNumber = "0" + newNumber;
                }
                return newNumber;
            }
        }
        #region::Designation
        public static void CreateAndAsignDesignationFor(ProductModel product)
        {
            string newDesignation = CreateDesignation(product);
            product.Designation = newDesignation;
        }

        private static string CreateDesignation(ProductModel product)
        {
            string newDivisionLZ = LeadingZeros(product.ProductDivision.Number, 3);
            string kindLZ = LeadingZeros(product.ProductDivision.ProductKind.Number, 2);
            string materialLZ = LeadingZeros(product.ProductDivision.ProductMaterial.Number, 2);
            string newDesignLZ = LeadingZeros(product.Design, 4);
            string newDesignation = $"{newDivisionLZ}-{kindLZ}-" +
                $"{materialLZ}/{newDesignLZ}";
            return newDesignation;
        }
        #endregion
    }
}
