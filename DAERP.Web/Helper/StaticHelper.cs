using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.DAL.DataAccess;
using DAERP.DAL.Services;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Helper
{
    public static class StaticHelper
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
        internal static void SetDynamicDataForSortingPurposes(dynamic viewData, string sortOrder, List<string> dynamicNames)
        {
            foreach (var dynamicName in dynamicNames)
            {
                viewData[dynamicName + "SortParam"] = sortOrder == dynamicName ? dynamicName + "_desc" : dynamicName;
            }
        }
        private static (string prop, string propParam, string prop_desc) GetNeededStringsForSortingPurposes(string prop)
        {
            string propParam = prop.Replace(".", String.Empty) + "SortParam";
            string prop_desc = prop + "_desc";
            return (prop, propParam, prop_desc);
        }

        internal static string ConvertImageToURL(byte[] image, string imageType)
        {
            string imageBase64Data = Convert.ToBase64String(image);
            string imageDataURL = string.Format($"data:image/{imageType};base64,{imageBase64Data}");
            return imageDataURL;
        }

        internal static MultiDropDownListViewModel GetModelForCustomerSelect(ICustomerData _customerData)
        {
            var activeCustomers = _customerData.GetAllCustomers().Where(c => c.State == "A");
            var data = new List<MultiDropDownListViewModel>();
            foreach (var customer in activeCustomers)
            {
                data.Add(new MultiDropDownListViewModel
                {
                    Id = customer.Id,
                    Name = customer.Designation + ": " + customer.Name
                });
            }
            MultiDropDownListViewModel model = new();
            model.ItemList = data.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return model;
        }

        internal static PaginatedList<ProductModel> SortAndFilterProductsForSelectPurpose(string currentSort,
            string sortOrder, string currentFilter, string searchString, int? pageNumber,
            ViewDataDictionary viewData, IEnumerable<ProductModel> products)
        {
            int pageSize = 12;
            if (sortOrder is null)
            {
                sortOrder = currentSort;
            }
            viewData["CurrentSort"] = sortOrder;
            viewData["CurrentFilter"] = searchString ?? currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                products = products.Where(p =>
                    p.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.EAN.ToString().Contains(normalizedSearchString) ||
                    p.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    p.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            if (products.Count() > 0)
            {
                string defaultPropToSort = "Designation";
                Helper.StaticHelper.SetDataForSortingPurposes(viewData, sortOrder, products.FirstOrDefault(), defaultPropToSort);
                if (String.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = defaultPropToSort;
                }
                bool descending = false;
                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }
                if (descending)
                {
                    products = products.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    products = products.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            return PaginatedList<ProductModel>.Create(products, pageNumber ?? 1, pageSize);
        }

        internal static PaginatedList<DeliveryNoteModel> SortAndFilterDeliveryNotesForSelectPurpose(string currentSort,
            string sortOrder, string currentFilter, string searchString, int? pageNumber,
            ViewDataDictionary viewData, IEnumerable<DeliveryNoteModel> deliveryNotes)
        {
            int pageSize = 12;
            if (sortOrder is null)
            {
                sortOrder = currentSort;
            }
            viewData["CurrentSort"] = sortOrder;
            viewData["CurrentFilter"] = searchString ?? currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                string normalizedSearchString = searchString.Normalize(System.Text.NormalizationForm.FormD).ToUpper();
                deliveryNotes = deliveryNotes.Where(dn =>
                    dn.Product.Designation.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.EAN.ToString().Contains(normalizedSearchString) ||
                    dn.Product.ProductDivision.Name.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Product.ProductDivision.ProductType.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString) ||
                    dn.Number.Normalize(System.Text.NormalizationForm.FormD).ToUpper().Contains(normalizedSearchString)
                );
            }
            if (deliveryNotes.Count() > 0)
            {
                string defaultPropToSort = "Number_desc";
                Helper.StaticHelper.SetDataForSortingPurposes(viewData, sortOrder, deliveryNotes.FirstOrDefault(), defaultPropToSort);
                if (String.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = defaultPropToSort;
                }
                bool descending = false;
                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }
                if (descending)
                {
                    deliveryNotes = deliveryNotes.OrderByDescending(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
                else
                {
                    deliveryNotes = deliveryNotes.OrderBy(e => DataOperations.GetPropertyValue(e, sortOrder));
                }
            }
            return PaginatedList<DeliveryNoteModel>.Create(deliveryNotes, pageNumber ?? 1, pageSize);
        }
    }
}
