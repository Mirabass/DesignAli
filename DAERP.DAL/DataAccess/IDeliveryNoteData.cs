using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IDeliveryNoteData
    {
        IEnumerable<DeliveryNoteModel> GetDeliveryNotes();
        void AddRangeOfDeliveryNotes(List<DeliveryNoteModel> deliveryNotes);
        Task AddAsync(DeliveryNoteFileModel deliveryNoteFile);
        IEnumerable<DeliveryNoteFileModel> GetDeliveryNoteFiles();
        DeliveryNoteFileModel GetDeliveryNoteFileBy(int fileId);
    }
}