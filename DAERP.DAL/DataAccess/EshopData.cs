using DAERP.BL.Models;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class EshopData : IEshopData
    {
        private ApplicationDbContext _db;
        public EshopData(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddEshopAsync(EshopModel eshop)
        {
            await _db.Eshops.AddAsync(eshop);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<EshopModel> GetAllEshops()
        {
            return _db.Eshops.AsNoTracking();
        }

        public EshopModel GetEshopBy(int? id)
        {
            return _db.Eshops.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
        }

        public void RemoveEshop(EshopModel Eshop)
        {
            _db.Eshops.Remove(Eshop);
            _db.SaveChanges();
        }

        public void UpdateEshop(EshopModel Eshop)
        {
            _db.Eshops.Update(Eshop);
            _db.SaveChanges();
        }
    }
}
