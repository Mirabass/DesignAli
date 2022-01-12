using DAERP.BL.Models;
using System.Collections.Generic;

namespace DAERP.DAL.DataAccess
{
    public interface ICustomerData
    {
        IEnumerable<CustomerModel> GetAllCustomers();
    }
}