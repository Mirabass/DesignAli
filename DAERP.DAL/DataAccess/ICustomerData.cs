using DAERP.BL.Models;
using DAERP.BL.Models.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public interface ICustomerData
    {
        IEnumerable<CustomerModel> GetAllCustomers();
        Task AddCustomerAsync(CustomerModel customer);
        CustomerModel GetCustomerBy(int? id);
        void RemoveCustomer(CustomerModel customer);
        void UpdateCustomer(CustomerModel customer);
        IEnumerable<CustomerProductModel> GetCustomerProductsBy(int customerId);
    }
}