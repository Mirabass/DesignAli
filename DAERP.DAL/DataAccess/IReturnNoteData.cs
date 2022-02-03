using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IReturnNoteData
    {
        Task AddAsync(ReturnNoteFileModel returnNoteFile);
        void AddRangeOfReturnNotes(List<ReturnNoteModel> returnNotes);
        ReturnNoteFileModel GetReturnNoteFileBy(int fileId);
        IEnumerable<ReturnNoteFileModel> GetReturnNoteFiles();
        IEnumerable<ReturnNoteModel> GetReturnNotes();
        Task UpdateFileAsync(ReturnNoteFileModel dnFile);
    }
}