using DAERP.BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IEshopData
    {
        Task AddEshopAsync(EshopModel eshop);
        IEnumerable<EshopModel> GetAllEshops();
        EshopModel GetEshopBy(int? id);
        void RemoveEshop(EshopModel Eshop);
        void UpdateEshop(EshopModel Eshop);
    }
}