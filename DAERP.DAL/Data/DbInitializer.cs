using DAERP.BL.Models;
using Helper;
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
            await InitializeCustomersAsync(context, paths[typeof(CustomerModel)]);
        }

        private static async Task InitializeCustomersAsync(ApplicationDbContext context, string path)
        {
            if (context.Customers.Any())
            {
                return;
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
            List<CustomerModel> customers = await CustomerModel.MapAsync(customerData, mapSettings, 1);
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }
}
