using DAERP.BL.Models.Movements;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface IEshopIssueNoteData
    {
        void AddRangeOfEshopIssueNotes(List<EshopIssueNoteModel> eshopIssueNotes);
        IEnumerable<EshopIssueNoteModel> GetEshopIssueNotes();
    }
}