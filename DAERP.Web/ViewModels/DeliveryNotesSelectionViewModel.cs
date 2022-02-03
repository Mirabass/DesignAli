using DAERP.BL.Models;
using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.Web.Helper;
using System.Collections.Generic;

namespace DAERP.Web.ViewModels
{
    public class DeliveryNotesSelectionViewModel
    {
        public PaginatedList<DeliveryNoteModel> DeliveryNotes { get; set; }
        public List<SelectedDeliveryNote> SelectedDeliveryNotes { get; set; }
        public CustomerModel Customer { get; set; }
    }
    public class SelectedDeliveryNote
    {
        public DeliveryNoteModel DeliveryNote { get; set; }
        public int Amount { get; set; }
        public bool IsPossibleAdd { get; set; } = true;
    }
}
