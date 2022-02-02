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
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            return _db.Customers.AsNoTracking();
        }

        public CustomerModel GetCustomerBy(int? id)
        {
            return _db.Customers.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<CustomerProductModel> GetCustomerProductsBy(int customerId)
        {
            var output = _db.Customers
                .Include(c => c.CustomerProducts)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Include(c => c.CustomerProducts)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductPrices).AsNoTracking()
                .Where(c => c.Id == customerId) // this is not probably neccessary
                .FirstOrDefault()
                .CustomerProducts
                .GroupBy(cp => cp.CustomerId).FirstOrDefault();
            return output;
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

        public async Task UpdateCustomerProductsPrices(CustomerModel customer)
        {
            await _db.CustomersProducts
                .Include(cp => cp.Product)
                    .ThenInclude(cp => cp.ProductPrices)
                .Include(cp => cp.Customer)
                .Where(cp => cp.CustomerId == customer.Id)
                .ForEachAsync(pc =>
                {
                    pc.DeliveryNotePrice = BL.PriceCalculation.DeliveryNotePrice(
                        pc.Product.ProductPrices.GainPercentValue,
                        pc.Customer.ProvisionFor60PercentValue,
                        pc.Product.ProductPrices.OperatedCostPrice);
                    pc.IssuedInvoicePrice = BL.PriceCalculation.IssuedInvoicePrice(
                        pc.DeliveryNotePrice,
                        pc.Customer.FVDiscountPercentValue);
                    pc.Value = BL.PriceCalculation.StockValue(
                        pc.AmountInStock,
                        pc.DeliveryNotePrice);
                });
            await _db.SaveChangesAsync();
        }
    }
}
