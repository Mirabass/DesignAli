using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface IIssuedInvoiceData
    {
        Task AddAsync(IssuedInvoiceFileModel IssuedInvoiceFile);
        void AddRangeOfIssuedInvoices(List<IssuedInvoiceModel> IssuedInvoices);
        IssuedInvoiceFileModel GetIssuedInvoiceFileBy(int fileId);
        IEnumerable<IssuedInvoiceFileModel> GetIssuedInvoiceFiles();
        IEnumerable<IssuedInvoiceModel> GetIssuedInvoices();
        Task UpdateFileAsync(IssuedInvoiceFileModel iiFile);
    }
}