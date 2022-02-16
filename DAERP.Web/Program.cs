using DAERP.BL.Models;
using DAERP.DAL.Data;
using DAERP.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAERP.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await CreateDbIfNotExistsAsync(host);

            host.Run();
        }

        private static async Task CreateDbIfNotExistsAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var paths = GeneratePaths(services);
                    await DbInitializer.InitializeAsync(context, paths);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured creating the DB.");
                }
            }
        }
        private static Dictionary<Type, string> GeneratePaths(IServiceProvider services)
        {
            var pathProvider = services.GetRequiredService<IPathProvider>();
            string customerDataFilePath = "static_files/GoogleSheetData/01.00.01 - Èíselník odbìratelù - ÈO - to export.tsv";
            Dictionary<Type, string> paths = new Dictionary<Type, string>()
            {
                { typeof(CustomerModel), pathProvider.MapPath(customerDataFilePath)}
            };
            return paths;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
