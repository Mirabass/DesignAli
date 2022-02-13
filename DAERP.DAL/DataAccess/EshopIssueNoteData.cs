using DAERP.BL.Models.Movements;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class EshopIssueNoteData : IEshopIssueNoteData
    {
        private ApplicationDbContext _db;
        public EshopIssueNoteData(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddRangeOfEshopIssueNotes(List<EshopIssueNoteModel> eshopIssueNotes)
        {
            _db.EshopIssueNotes.AddRange(eshopIssueNotes);
            _db.SaveChanges();
        }

        public IEnumerable<EshopIssueNoteModel> GetEshopIssueNotes()
        {
            return _db.EshopIssueNotes
                .Include(dn => dn.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(dn => dn.Eshop).AsNoTracking();
        }

    }
}
