using DAERP.BL.Models.Movements;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface IDeliveryNoteData
    {
        IEnumerable<DeliveryNoteModel> GetDeliveryNotes();
        void AddRangeOfDeliveryNotes(List<DeliveryNoteModel> deliveryNotes);
    }
}