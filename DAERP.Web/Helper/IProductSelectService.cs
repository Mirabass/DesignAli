using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace DAERP.Web.Helper
{
    public interface IProductSelectService
    {
        List<SelectedProduct> Get(int? addSelected, int? removeSelected, int? removeAllSelected, ITempDataDictionary tempData, bool checkStockRemain);
        List<SelectedProduct> Get(ITempDataDictionary tempData);
    }
}