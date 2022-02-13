using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
using DAERP.BL.Models.Product;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class ReturnNoteData : IReturnNoteData
    {
        private ApplicationDbContext _db;
        public ReturnNoteData(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ReturnNoteFileModel returnNoteFile)
        {
            await _db.ReturnNoteFiles.AddAsync(returnNoteFile);
            await _db.SaveChangesAsync();
        }

        public void AddRangeOfReturnNotes(List<ReturnNoteModel> returnNotes)
        {
            _db.ReturnNotes.AddRange(returnNotes);
            _db.SaveChanges();
        }

        public ReturnNoteFileModel GetReturnNoteFileBy(int fileId)
        {
            return _db.ReturnNoteFiles.First(dnf => dnf.Id == fileId);
        }

        public IEnumerable<ReturnNoteFileModel> GetReturnNoteFiles()
        {
            var dnFiles = _db.ReturnNoteFiles.AsNoTracking();
            return dnFiles;
        }

        public IEnumerable<ReturnNoteModel> GetReturnNotes()
        {
            return _db.ReturnNotes
                .Include(dn => dn.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(dn => dn.Customer).AsNoTracking();
        }

        public async Task UpdateFileAsync(ReturnNoteFileModel dnFile)
        {
            _db.ReturnNoteFiles.Update(dnFile);
            await _db.SaveChangesAsync();
        }
    }
}
