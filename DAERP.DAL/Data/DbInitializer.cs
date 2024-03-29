﻿using DAERP.BL;
using DAERP.BL.Models;
using DAERP.BL.Models.Product;
using Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.DAL.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, Dictionary<Type, string> paths)
        {
            context.Database.EnsureCreated();
            List<CustomerModel> customers = await InitializeCustomersAsync(context, paths[typeof(CustomerModel)]);
            await InitializeEshopsAsync(context, paths[typeof(EshopModel)]);
            await InitializeProductDivisionsAsync(context, paths[typeof(ProductDivisionModel)]);
            await InitializeProductAsync(context, paths[typeof(ProductModel)]);
            await InitializeProductCustomerStockAsync(context, customers);
            await InitializeProductCustomerPricesAsync(context);
        }

        private static async Task InitializeProductCustomerPricesAsync(ApplicationDbContext context)
        {
            await context.CustomersProducts
                .Include(cp => cp.Product)
                    .ThenInclude(cp => cp.ProductPrices)
                .Include(cp => cp.Customer)
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
            var customersProducts = context.CustomersProducts;
            context.CustomersProducts.UpdateRange(customersProducts);
            await context.SaveChangesAsync();
        }

        private static async Task InitializeProductCustomerStockAsync(ApplicationDbContext context, List<CustomerModel> customers)
        {
            if (context.CustomersProducts.Any())
            {
                return;
            }
            List<CustomerProductModel> customerProducts = new();
            await context.Products.ForEachAsync(p =>
            {
                foreach (CustomerModel customer in customers)
                {
                    CustomerProductModel customerProduct = new CustomerProductModel
                    {
                        ProductId = p.Id,
                        CustomerId = customer.Id
                    };
                    customerProducts.Add(customerProduct);
                }
            });
            await context.CustomersProducts.AddRangeAsync(customerProducts);
            await context.SaveChangesAsync();
        }

        private static async Task InitializeProductAsync(ApplicationDbContext context, string path)
        {
            if (context.Products.Any())
            {
                return;
            }
            Dictionary<(int, int), string> productData = FileProcessor.LoadDataFromFile_tableWithTabs(path);
            Dictionary<string, int> pcdMapSettings = new Dictionary<string, int>
            {
                { nameof(ProductColorDesignModel.Quantity), 8 },
                { nameof(ProductColorDesignModel.Orientation), 9 },
                { nameof(ProductColorDesignModel.MainPartRAL), 10 },
                { nameof(ProductColorDesignModel.MainPartColorName), 11 },
                { nameof(ProductColorDesignModel.PocketRAL), 13 },
                { nameof(ProductColorDesignModel.PocketColorName), 14 }
            };
            Dictionary<string, int> psMapSettings = new Dictionary<string, int>()
            {
                { nameof(ProductStrapModel.Type), 16 },
                { nameof(ProductStrapModel.Material), 17 },
                { nameof(ProductStrapModel.Length), 18 },
                { nameof(ProductStrapModel.Width), 19 },
                { nameof(ProductStrapModel.RAL), 20 },
                { nameof(ProductStrapModel.ColorName), 21 },
                { nameof(ProductStrapModel.Attachment), 23 }
            };
            Dictionary<string, int> ppMapSettings = new Dictionary<string, int>()
            {
                { nameof(ProductPricesModel.OperatedCostPrice), 26 },
                { nameof(ProductPricesModel.OperatedSellingPrice), 27 }
            };
            Dictionary<string, int> pMapSettings = new Dictionary<string, int>()
            {
                { nameof(ProductModel.EAN), 2 },
                { nameof(ProductModel.Design), 5 },
                { nameof(ProductModel.Motive), 26 },
                { nameof(ProductModel.Accessories), 27 }
            };
            int lastRow = productData.Select(pd => pd.Key.Item1).Max() + 1;
            List<ProductModel> products = new();
            for (int row = 2; row < lastRow; row++)
            {
                Dictionary<int, string> productDataRow = productData
                        .Where(pd => pd.Key.Item1 == row)
                        .ToDictionary(pd => pd.Key.Item2, pd => pd.Value);
                ProductDivisionModel productDivision = GetProductDivision(context, productDataRow, 1);
                ProductColorDesignModel productColorDesign = ProductColorDesignModel.Map(productDataRow, pcdMapSettings);
                ProductStrapModel productStrap = ProductStrapModel.Map(productDataRow, psMapSettings);
                ProductPricesModel productPrices = ProductPricesModel.Map(productDataRow, ppMapSettings);
                // TODO: Add Image
                ProductModel product = ProductModel.Map(productDataRow, pMapSettings, productDivision, productColorDesign, productStrap, productPrices);
                product.ProductPrices.GainPercentValue = BL.PriceCalculation.GainPercentValue(product.ProductPrices.OperatedCostPrice, product.ProductPrices.OperatedSellingPrice);
                CustomOperations.CreateAndAsignDesignationFor(product);
                product.ProductDivision = null;
                products.Add(product);
            }
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        private static ProductDivisionModel GetProductDivision(ApplicationDbContext context, Dictionary<int, string> productDataRow, int productDesignationPosition)
        {
            string designation = productDataRow[productDesignationPosition];
            string divisionNumberString = designation.Substring(0, 3);
            string kindNumberString = designation.Substring(4, 2);
            string materialNumberString = designation.Substring(7, 2);
            int divisionNumber = Convert.ToInt32(divisionNumberString);
            int kindNumber = Convert.ToInt32(kindNumberString);
            int materialNumber = Convert.ToInt32(materialNumberString);
            ProductDivisionModel productDivision = context.ProductDivisions.AsNoTracking()
                .Include(pd => pd.ProductKind).AsNoTracking()
                .Include(pd => pd.ProductMaterial).AsNoTracking()
                .FirstOrDefault(pd => pd.ProductKind.Number == kindNumber && pd.ProductMaterial.Number == materialNumber && pd.Number == divisionNumber);
            return productDivision;
        }

        private static async Task InitializeProductDivisionsAsync(ApplicationDbContext context, string path)
        {
            if (context.ProductDivisions.Any())
            {
                return;
            }
            Dictionary<(int, int), string> productDivisionData = FileProcessor.LoadDataFromFile_tableWithTabs(path);
            Dictionary<string, int> pkMapSettings = new Dictionary<string, int>
            {
                { nameof(ProductKindModel.Number), 2 },
                { nameof(ProductKindModel.Name), 3 }
            };
            Dictionary<string, int> pmMapSettings = new Dictionary<string, int>
            {
                { nameof(ProductMaterialModel.Number), 4 },
                { nameof(ProductMaterialModel.Name), 5 }
            };
            Dictionary<string, int> pdMapSettings = new Dictionary<string, int>
            {
                { nameof(ProductDivisionModel.Number), 0 },
                { nameof(ProductDivisionModel.Name), 1 },
                { nameof(ProductDivisionModel.ProductType), 6 },
                { nameof(ProductDivisionModel.Comment), 7 }
            };
            int lastRow = productDivisionData.Select(_ => _.Key.Item1).Max() + 1;
            List<ProductDivisionModel> productDivisions = new();
            for (int row = 2; row < lastRow; row++)
            {
                Dictionary<int, string> productDivisionDataRow = productDivisionData
                    .Where(pd => pd.Key.Item1 == row)
                    .ToDictionary(pd => pd.Key.Item2, pd => pd.Value);
                ProductKindModel productKind = ProductKindModel.Map(productDivisionDataRow, pkMapSettings);
                ProductMaterialModel productMaterial = ProductMaterialModel.Map(productDivisionDataRow, pmMapSettings);
                ProductDivisionModel productDivisionModel = ProductDivisionModel.Map(productDivisionDataRow, pdMapSettings, productKind, productMaterial);
                productDivisions.Add(productDivisionModel);
            }
            await context.ProductDivisions.AddRangeAsync(productDivisions);
            await context.SaveChangesAsync();
        }

        private static async Task InitializeEshopsAsync(ApplicationDbContext context, string path)
        {
            if (context.Eshops.Any())
            {
                return;
            }
            Dictionary<(int, int), string> eshopData = FileProcessor.LoadDataFromFile_tableWithTabs(path);
            Dictionary<string, int> mapSettings = new Dictionary<string, int>
            {
                { nameof(EshopModel.Designation), 1 },
                { nameof(EshopModel.State), 3 },
                { nameof(EshopModel.Name), 4 },
                { nameof(EshopModel.Web), 5 },
                { nameof(EshopModel.ContactPerson), 6 },
                { nameof(EshopModel.Phone), 7 },
                { nameof(EshopModel.Mobile), 8 },
                { nameof(EshopModel.Email), 9 },
                { nameof(EshopModel.SFName), 10 },
                { nameof(EshopModel.SFStreetAndNo), 11 },
                { nameof(EshopModel.SFZIP), 12 },
                { nameof(EshopModel.SFCity), 13 },
                { nameof(EshopModel.SFCountry), 14 },
                { nameof(EshopModel.SFIN), 15 },
                { nameof(EshopModel.SFTIN), 16 },
                { nameof(EshopModel.FVDiscountPercentValue), 17 },
                { nameof(EshopModel.Maturity), 18 },
                { nameof(EshopModel.CurrencyCode), 19 },
                { nameof(EshopModel.RoundPriceWithVAT), 20 },
                { nameof(EshopModel.ContractDANumber), 21 },
                { nameof(EshopModel.ContractONumber), 22 },
                { nameof(EshopModel.ContractContent), 23 },
                { nameof(EshopModel.ContractPoPro), 24 },
                { nameof(EshopModel.ContractPoUm), 25 },
                { nameof(EshopModel.ContractDateSigned), 26 },
                { nameof(EshopModel.ContractDateFrom), 27 },
                { nameof(EshopModel.ContractDateTo), 28 },
                { nameof(EshopModel.ContractRent), 29 },
                { nameof(EshopModel.ContractPeriod), 30 },
                { nameof(EshopModel.ContractProvisionPercentValue), 31 },
                { nameof(EshopModel.Comment), 34 }
            };
            List<EshopModel> eshops = EshopModel.Map(eshopData, mapSettings, 2);
            await context.Eshops.AddRangeAsync(eshops);
            await context.SaveChangesAsync();
        }

        private static async Task<List<CustomerModel>> InitializeCustomersAsync(ApplicationDbContext context, string path)
        {
            if (context.Customers.Any())
            {
                return context.Customers.ToList();
            }
            Dictionary<(int, int), string> customerData = FileProcessor.LoadDataFromFile_tableWithTabs(path);
            Dictionary<string, int> mapSettings = new Dictionary<string, int>
            {
                { nameof(CustomerModel.Designation), 1 },
                { nameof(CustomerModel.State), 3 },
                { nameof(CustomerModel.Name), 4 },
                { nameof(CustomerModel.Franchise), 5 },
                { nameof(CustomerModel.SFName), 6 },
                { nameof(CustomerModel.SFStreetAndNo), 7 },
                { nameof(CustomerModel.SFZIP), 8 },
                { nameof(CustomerModel.SFCity), 9 },
                { nameof(CustomerModel.SFCountry), 10 },
                { nameof(CustomerModel.SFIN), 11 },
                { nameof(CustomerModel.SFTIN), 12 },
                { nameof(CustomerModel.ProvisionFor60PercentValue), 13 },
                { nameof(CustomerModel.FVDiscountPercentValue), 14 },
                { nameof(CustomerModel.Maturity), 15 },
                { nameof(CustomerModel.CurrencyCode), 16 },
                { nameof(CustomerModel.RoundPriceWithVAT), 17 },
                { nameof(CustomerModel.DFName), 18 },
                { nameof(CustomerModel.DFContactPerson), 19 },
                { nameof(CustomerModel.DFStreetAndNo), 20 },
                { nameof(CustomerModel.DFZIP), 21 },
                { nameof(CustomerModel.DFCity), 22 },
                { nameof(CustomerModel.DFCountry), 23 },
                { nameof(CustomerModel.DFPhone), 24 },
                { nameof(CustomerModel.DFMobile), 25 },
                { nameof(CustomerModel.DFEmail), 26 },
                { nameof(CustomerModel.DFIN), 27 },
                { nameof(CustomerModel.DFTIN), 28 },
                { nameof(CustomerModel.DFBank), 29 },
                { nameof(CustomerModel.DFAccountNumber), 30 },
                { nameof(CustomerModel.DFBIC), 31 },
                { nameof(CustomerModel.MDName), 32 },
                { nameof(CustomerModel.MDContactPerson), 33 },
                { nameof(CustomerModel.MDStreetAndNo), 34 },
                { nameof(CustomerModel.MDZIP), 35 },
                { nameof(CustomerModel.MDCity), 36 },
                { nameof(CustomerModel.ContractDANumber), 37 },
                { nameof(CustomerModel.ContractONumber), 38 },
                { nameof(CustomerModel.ContractContent), 39 },
                { nameof(CustomerModel.ContractPoPro), 40 },
                { nameof(CustomerModel.ContractPoUm), 41 },
                { nameof(CustomerModel.ContractDateSigned), 42 },
                { nameof(CustomerModel.ContractDateFrom), 43 },
                { nameof(CustomerModel.ContractDateTo), 44 },
                { nameof(CustomerModel.ContractRent), 45 },
                { nameof(CustomerModel.ContractPeriod), 46 },
                { nameof(CustomerModel.ContractProvisionPercentValue), 47 },
                { nameof(CustomerModel.Comment), 50 }
            };
            List<CustomerModel> customers = CustomerModel.Map(customerData, mapSettings, 1);
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
            return customers;
        }
    }
}
