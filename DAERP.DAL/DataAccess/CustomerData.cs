using DAERP.BL.Models;
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
    public class CustomerData : ICustomerData
    {
        private ApplicationDbContext _db;
        public CustomerData(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddCustomerAsync(CustomerModel customer)
        {
            customer.CustomerProducts = new List<CustomerProductModel>();
            await _db.Products.ForEachAsync(p =>
            {
                customer.CustomerProducts.Add(new CustomerProductModel()
                {
                    ProductId = p.Id,
                    CustomerId = customer.Id
                });
            });
            _db.Customers.Add(customer);
            _db.SaveChanges();
        }

        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            return _db.Customers;
        }

        public CustomerModel GetCustomerBy(int? id)
        {
            return _db.Customers.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
        }

        public void RemoveCustomer(CustomerModel customer)
        {
            var customerProductToRemove = _db.CustomersProducts.Where(cp => cp.Customer == customer);
            _db.CustomersProducts.RemoveRange(customerProductToRemove);
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
