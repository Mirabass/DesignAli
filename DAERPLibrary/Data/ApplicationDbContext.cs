using DAERPLibrary.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace DAERPLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductColorDesignModel> ProductColorDesigns { get; set; }
        public DbSet<ProductDivisionModel> ProductDivisions { get; set; }
        public DbSet<ProductKindModel> ProductKinds { get; set; }
        public DbSet<ProductMaterialModel> ProductMaterials { get; set; }
        public DbSet<ProductStrapModel> ProductStraps { get; set; }
    }
}
