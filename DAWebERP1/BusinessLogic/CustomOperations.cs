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
    }
}
