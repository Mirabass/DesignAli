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
            List<int> orderOfCustomers = CreateOrderOfCustomers(customerProducts);
            List<ProductModel> products = customerProducts
                .Select(cp => cp.Product)
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();
            List<ProductCustomersReadViewModel> viewList = new List<ProductCustomersReadViewModel>();
            products.ForEach(p => 
            {
                List<int> customerStockAmounts = GetCustomerStockAmounts(orderOfCustomers, p, customerProducts);
                viewList.Add(new ProductCustomersReadViewModel()
                {
                    Product = p,
                    CustomerStockAmounts = customerStockAmounts,
                    AmountInSelectedCustomersStocks = customerStockAmounts.Sum()
                });
            });
            return viewList;
        }

        private static List<int> GetCustomerStockAmounts(List<int> orderOfCustomers, ProductModel product, List<CustomerProductModel> customerProducts)
        {
            List<int> customerStockAmounts = new List<int>();
            foreach (var customerId in orderOfCustomers)
            {
                customerStockAmounts.Add(
                    customerProducts
                        .Where(cp => cp.Product == product && cp.CustomerId == customerId)
                        .Select(cp => cp.AmountInStock)
                        .FirstOrDefault()
                );
            }
            return customerStockAmounts;
        }

        private static List<int> CreateOrderOfCustomers(List<CustomerProductModel> customerProducts)
        {
            List<int> orderOfCustomers = new List<int>();
            customerProducts.ForEach(customerProduct =>
            {
                if (orderOfCustomers.Contains(customerProduct.CustomerId) == false)
                {
                    orderOfCustomers.Add(customerProduct.CustomerId);
                }
            });
            return orderOfCustomers;
        }
    }
}
