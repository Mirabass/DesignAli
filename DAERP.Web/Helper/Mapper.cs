using DAERP.BL.Models;
using DAERP.BL.Models.Product;
using DAERP.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Helper
{
    public static class Mapper
    {
        public static List<ProductCustomersReadViewModel> ToProductCustomerReadViewModelListFrom(List<CustomerProductModel> customerProducts)
        {
            Dictionary<int,string> orderOfCustomers = CreateOrderOfCustomers(customerProducts);

            List<ProductModel> products = customerProducts
                .Select(cp => cp.Product)
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();
            List<ProductCustomersReadViewModel> viewList = new List<ProductCustomersReadViewModel>();
            products.ForEach(p => 
            {
                Dictionary<int,int> customerStockAmounts = GetCustomerStockAmounts(orderOfCustomers, p, customerProducts);
                viewList.Add(new ProductCustomersReadViewModel()
                {
                    Product = p,
                    CustomerStockAmounts = customerStockAmounts,
                    AmountInSelectedCustomersStocks = customerStockAmounts.Values.Sum(),
                    CustomersNames = orderOfCustomers
                });
            });
            return viewList;
        }

        private static Dictionary<int,int> GetCustomerStockAmounts(Dictionary<int, string> orderOfCustomers, ProductModel product, List<CustomerProductModel> customerProducts)
        {
            Dictionary<int,int> customerStockAmounts = new Dictionary<int, int>();
            foreach (var customer in orderOfCustomers)
            {
                customerStockAmounts.Add(customer.Key,
                    customerProducts
                        .Where(cp => cp.Product == product && cp.CustomerId == customer.Key)
                        .Select(cp => cp.AmountInStock)
                        .FirstOrDefault()
                );
            }
            return customerStockAmounts;
        }

        private static Dictionary<int,string> CreateOrderOfCustomers(List<CustomerProductModel> customerProducts)
        {
            Dictionary<int, string> orderOfCustomers = new Dictionary<int, string>();
            customerProducts.ForEach(customerProduct =>
            {
                if (orderOfCustomers.Keys.Contains(customerProduct.CustomerId) == false)
                {
                    orderOfCustomers.Add(customerProduct.CustomerId, customerProduct.Customer.Designation + " " + customerProduct.Customer.Name);
                }
            });
            return orderOfCustomers;
        }
    }
}
