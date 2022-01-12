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

        public void AddCustomer(CustomerModel customer)
        {
            _db.Customers.Add(customer);
            _db.SaveChanges();
        }

        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            return _db.Customers;
        }

        public CustomerModel GetCustomerBy(int? id)
        {
            return _db.Customers.Where(c => c.Id == id).FirstOrDefault();
        }

        public void RemoveCustomer(CustomerModel customer)
        {
            _db.Customers.Remove(customer);
            _db.SaveChanges();
        }

        public void UpdateCustomer(CustomerModel customer)
        {
            _db.Customers.Update(customer);
            _db.SaveChanges();
        }
    }
}
