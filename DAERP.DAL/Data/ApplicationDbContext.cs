using DAERP.BL.Models;
using DAERP.BL.Models.Files;
using DAERP.BL.Models.Movements;
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
            modelBuilder
                .Entity<NoteModel>().ToTable("Notes")
                .HasDiscriminator<int>("NoteType")
                .HasValue<DeliveryNoteModel>(1)
                .HasValue<ReturnNoteModel>(2);
            modelBuilder
                .Entity<NoteFileModel>().ToTable("NoteFiles")
                .HasDiscriminator<int>("NoteFileType")
                .HasValue<DeliveryNoteFileModel>(1)
                .HasValue<ReturnNoteFileModel>(2);
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductPricesModel> ProductPrices { get; set; }
        public DbSet<ProductColorDesignModel> ProductColorDesigns { get; set; }
        public DbSet<ProductDivisionModel> ProductDivisions { get; set; }
        public DbSet<ProductKindModel> ProductKinds { get; set; }
        public DbSet<ProductMaterialModel> ProductMaterials { get; set; }
        public DbSet<ProductStrapModel> ProductStraps { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<CustomerProductModel> CustomersProducts { get; set; }
        public DbSet<ProductReceiptModel> ProductReceipts { get; set; }
        public DbSet<DeliveryNoteModel> DeliveryNotes { get; set; }
        public DbSet<DeliveryNoteFileModel> DeliveryNoteFiles { get; set; }
        public DbSet<ReturnNoteModel> ReturnNotes { get; set; }
        public DbSet<ReturnNoteFileModel> ReturnNoteFiles { get; set; }
    }
}
