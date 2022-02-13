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
    public class IssuedInvoiceData : IIssuedInvoiceData
    {
        private ApplicationDbContext _db;
        public IssuedInvoiceData(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(IssuedInvoiceFileModel IssuedInvoiceFile)
        {
            await _db.IssuedInvoiceFiles.AddAsync(IssuedInvoiceFile);
            await _db.SaveChangesAsync();
        }

        public void AddRangeOfIssuedInvoices(List<IssuedInvoiceModel> IssuedInvoices)
        {
            _db.IssuedInvoices.AddRange(IssuedInvoices);
            _db.SaveChanges();
        }

        public IssuedInvoiceFileModel GetIssuedInvoiceFileBy(int fileId)
        {
            return _db.IssuedInvoiceFiles.First(dnf => dnf.Id == fileId);
        }

        public IEnumerable<IssuedInvoiceFileModel> GetIssuedInvoiceFiles()
        {
            var dnFiles = _db.IssuedInvoiceFiles.AsNoTracking();
            return dnFiles;
        }

        public IEnumerable<IssuedInvoiceModel> GetIssuedInvoices()
        {
            return _db.IssuedInvoices
                .Include(dn => dn.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(dn => dn.Customer).AsNoTracking();
        }

        public async Task UpdateFileAsync(IssuedInvoiceFileModel iiFile)
        {
            _db.IssuedInvoiceFiles.Update(iiFile);
            await _db.SaveChangesAsync();
        }
    }
}
