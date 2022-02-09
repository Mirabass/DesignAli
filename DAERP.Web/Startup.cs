using AutoMapper;
using DAERP.DAL.Data;
using DAERP.DAL.DataAccess;
using DAERP.DAL.Services;
using DAERP.Web.Helper;
using DAERP.Web.ViewModels.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DAERP.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("BusinessDataConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>();

            // Personal services:
            services.AddAutoMapper(typeof(ProductProfile));

            services.AddSingleton<IColorProvider, ColorProvider>();
            services.AddSingleton<IPathProvider, PathProvider>();
            
            services.AddTransient<ICustomerData, CustomerData>();
            services.AddTransient<IEshopData, EshopData>();
            services.AddTransient<IProductData, ProductData>();
            services.AddTransient<IProductSelectService, ProductSelectService>();
            services.AddTransient<IDeliveryNoteSelectService,DeliveryNoteSelectService>();
            services.AddTransient<IProductReceiptData, ProductReceiptData>();
            services.AddTransient<IDeliveryNoteData, DeliveryNoteData>();
            services.AddTransient<IReturnNoteData, ReturnNoteData>();
            services.AddTransient<IIssuedInvoiceData, IssuedInvoiceData>();
            services.AddTransient<IEshopIssueNoteData, EshopIssueNoteData>();
            services.AddTransient<ICustomerProductData, CustomerProductData>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
