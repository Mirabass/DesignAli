using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace DAERP.Web.Helper
{
    public class DeliveryNoteSelectService : IDeliveryNoteSelectService
    {
        public DeliveryNoteSelectService()
        {

        }
        public List<SelectedDeliveryNote> Get(int? addSelected,
            int? removeSelected,
            int? removeAllSelected, ITempDataDictionary tempData, bool checkStockRemain)
        {
            throw new NotImplementedException();
        }
    }
}
