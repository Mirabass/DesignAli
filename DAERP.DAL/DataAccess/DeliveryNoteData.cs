using DAERP.BL;
using DAERP.BL.Models.Files;
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
    public class DeliveryNoteData : IDeliveryNoteData
    {
        private ApplicationDbContext _db;
        public DeliveryNoteData(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(DeliveryNoteFileModel deliveryNoteFile)
        {
            await _db.DeliveryNoteFiles.AddAsync(deliveryNoteFile);
            await _db.SaveChangesAsync();
        }

        public void AddRangeOfDeliveryNotes(List<DeliveryNoteModel> deliveryNotes)
        {
            _db.DeliveryNotes.AddRange(deliveryNotes);
            _db.SaveChanges();
        }

        public IEnumerable<DeliveryNoteModel> GetDeliveryNotes()
        {
            return _db.DeliveryNotes
                .Include(dn => dn.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(dn => dn.Customer).AsNoTracking();
        }
    }
}
