using DAERP.BL.Models;
using DAERP.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.DataAccess
{
    public class CustomerData : ICustomerData
    {
        private ApplicationDbContext _db;
        public CustomerData(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            return _db.Customers;
        }
    }
}
