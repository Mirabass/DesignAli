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
        private static string _customerDataFilePath = "static_files/GoogleSheetData/01.00.01 - Číselník odběratelů - ČO - to export.tsv";
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            await InitializeCustomersAsync(context);
        }

        private static async Task InitializeCustomersAsync(ApplicationDbContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }
            Dictionary<(int, int), string> customerData = FileProcessor.LoadDataFromFile_tableWithTabs(_customerDataFilePath);
            Dictionary<string, int> mapSettings = new Dictionary<string, int>
            {
                { nameof(CustomerModel.Designation), 1 }
            };
            List<CustomerModel> customers = await CustomerModel.MapAsync(customerData, mapSettings, 1);
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }
}
