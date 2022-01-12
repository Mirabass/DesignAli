using DAERP.BL.Models;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface ICustomerData
    {
        IEnumerable<CustomerModel> GetAllCustomers();
        void AddCustomer(CustomerModel customer);
        CustomerModel GetCustomerBy(int? id);
        void RemoveCustomer(CustomerModel customer);
        void UpdateCustomer(CustomerModel customer);
    }
}