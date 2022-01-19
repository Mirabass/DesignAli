using DAERP.BL.Models;
using DAERP.BL.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace DAERP.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CustomerProductModel>()
                .HasKey(cp => new { cp.CustomerId, cp.ProductId });
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductColorDesignModel> ProductColorDesigns { get; set; }
        public DbSet<ProductDivisionModel> ProductDivisions { get; set; }
        public DbSet<ProductKindModel> ProductKinds { get; set; }
        public DbSet<ProductMaterialModel> ProductMaterials { get; set; }
        public DbSet<ProductStrapModel> ProductStraps { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<CustomerProductModel> CustomersProducts { get; set; }
    }
}
