using DAERP.BL.Models.Product;
using DAERP.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DAERP.Web.Helper
{
    public static class Helper
    {
        internal static void SetDataForSortingPurposes<T>(dynamic viewData, string sortOrder, T model, string defaultProp)
        {
            (_, string defaultPropParam, string defaultProp_desc) = GetNeededStringsForSortingPurposes(defaultProp);
            viewData[defaultPropParam] = String.IsNullOrEmpty(sortOrder) ? defaultProp + "_desc" : "";
            List<string> paramsForSorting = DataOperations.GetParametersForSortingPurposes(model);
            foreach (var item in paramsForSorting)
            {
                if (item != defaultProp)
                {
                    (string prop, string propParam, string prop_desc) = GetNeededStringsForSortingPurposes(item);
                    viewData[propParam] = sortOrder == item ? prop_desc : prop;
                }
            }
        }

        private static (string prop, string propParam, string prop_desc) GetNeededStringsForSortingPurposes(string prop)
        {
            string propParam = prop.Replace(".", String.Empty) + "SortParam";
            string prop_desc = prop + "_desc";
            return (prop, propParam, prop_desc);
        }
    }
}
