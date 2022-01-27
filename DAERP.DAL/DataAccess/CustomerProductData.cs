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
    public class CustomerProductData : ICustomerProductData
    {
        private ApplicationDbContext _db;
        public CustomerProductData(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<CustomerProductModel> GetCustomersWithStockOfProductWithChildModelsIncludedBy(int? id)
        {
            var customersWithStockOfProduct = _db.Products
                                                .Include(p => p.ProductCustomers)
                                                    .ThenInclude(cp => cp.Customer)
                                                .Where(p => p.Id == id)
                                                .SelectMany(p => p.ProductCustomers)
                                                .Where(cp => cp.AmountInStock > 0);
            return customersWithStockOfProduct;
        }

        public IEnumerable<CustomerProductModel> GetProductsInStockOfCustomerWithChildModelsIncludedBy(int? id)
        {
            var productsInStockOfCustomer = _db.Customers
                                                .Include(c => c.CustomerProducts)
                                                    .ThenInclude(cp => cp.Product)
                                                .Where(c => c.Id == id)
                                                .SelectMany(c => c.CustomerProducts)
                                                .Where(cp => cp.AmountInStock > 0);
            return productsInStockOfCustomer;
        }

        public IEnumerable<CustomerProductModel> GetProductsInStockOfCustomersWithChildModelsIncludedBy(int[] customersIds)
        {
            var customerProducts = _db.CustomersProducts
                .Include(cp => cp.Customer)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductDivision)
                .Where(cp => customersIds.ToList().Contains(cp.CustomerId));
            return customerProducts;    
        }

        public CustomerProductModel GetCustomerProductBy(int customerId, int productId)
        {
            return _db.CustomersProducts
                .Where(cp => cp.CustomerId == customerId && cp.ProductId == productId)
                .FirstOrDefault();
        }
    }
}
