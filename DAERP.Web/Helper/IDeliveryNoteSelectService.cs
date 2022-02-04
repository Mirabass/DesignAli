using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace DAERP.Web.Helper
{
    public interface IDeliveryNoteSelectService
    {
        List<SelectedDeliveryNote> Get(ITempDataDictionary tempData);
        List<SelectedDeliveryNote> Get(int? addSelected, int? removeSelected, int? removeAllSelected, ITempDataDictionary tempData, bool checkStockRemain);
    }
}