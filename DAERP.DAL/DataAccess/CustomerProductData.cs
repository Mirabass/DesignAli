using DAERP.BL.Models;
using DAERP.BL.Models.Movements;
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
                                                    .ThenInclude(cp => cp.Customer).AsNoTracking()
                                                .Where(p => p.Id == id)
                                                .SelectMany(p => p.ProductCustomers)
                                                .Where(cp => cp.AmountInStock > 0);
            return customersWithStockOfProduct;
        }

        public IEnumerable<CustomerProductModel> GetProductsInStockOfCustomerWithChildModelsIncludedBy(int? id)
        {
            var productsInStockOfCustomer = _db.Customers
                                                .Include(c => c.CustomerProducts)
                                                    .ThenInclude(cp => cp.Product).AsNoTracking()
                                                .Where(c => c.Id == id)
                                                .SelectMany(c => c.CustomerProducts)
                                                .Where(cp => cp.AmountInStock > 0);
            return productsInStockOfCustomer;
        }

        public IEnumerable<CustomerProductModel> GetProductsInStockOfCustomersWithChildModelsIncludedBy(int[] customersIds)
        {
            var customerProducts = _db.CustomersProducts
                .Include(cp => cp.Customer).AsNoTracking()
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductDivision).AsNoTracking()
                .Where(cp => customersIds.ToList().Contains(cp.CustomerId));
            return customerProducts;    
        }

        public CustomerProductModel GetCustomerProductBy(int customerId, int productId)
        {
            return _db.CustomersProducts.AsNoTracking()
                .Where(cp => cp.CustomerId == customerId && cp.ProductId == productId)
                .FirstOrDefault();
        }

        public void IncreaseStock(List<DeliveryNoteModel> deliveryNotes)
        {
            deliveryNotes.ForEach(dn =>
            {
                var productCustomerStock = _db.CustomersProducts
                .Where(cp => cp.ProductId == dn.ProductId && cp.CustomerId == dn.CustomerId)
                .FirstOrDefault();
                productCustomerStock.AmountInStock += dn.StartingAmount;
                productCustomerStock.Value += dn.StartingAmount * productCustomerStock.IssuedInvoicePrice;
            });
            _db.SaveChanges();
        }
    }
}
