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
                .Entity<MovementModel>().ToTable("Movements")
                .HasDiscriminator<int>("MovementType")
                .HasValue<DeliveryNoteModel>(1)
                .HasValue<ReturnNoteModel>(2)
                .HasValue<ProductReceiptModel>(3)
                .HasValue<IssuedInvoiceModel>(4)
                .HasValue<EshopIssueNoteModel>(5);
            modelBuilder
                .Entity<NoteFileModel>().ToTable("NoteFiles")
                .HasDiscriminator<int>("NoteFileType")
                .HasValue<DeliveryNoteFileModel>(1)
                .HasValue<ReturnNoteFileModel>(2)
                .HasValue<IssuedInvoiceFileModel>(3);
        }

        public DbSet<ProductPricesModel> ProductPrices { get; set; }
        public DbSet<ProductColorDesignModel> ProductColorDesigns { get; set; }
        public DbSet<ProductDivisionModel> ProductDivisions { get; set; }
        public DbSet<ProductKindModel> ProductKinds { get; set; }
        public DbSet<ProductMaterialModel> ProductMaterials { get; set; }
        public DbSet<ProductStrapModel> ProductStraps { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<EshopModel> Eshops { get; set; }
        public DbSet<CustomerProductModel> CustomersProducts { get; set; }
        public DbSet<ProductReceiptModel> ProductReceipts { get; set; }
        public DbSet<DeliveryNoteModel> DeliveryNotes { get; set; }
        public DbSet<DeliveryNoteFileModel> DeliveryNoteFiles { get; set; }
        public DbSet<ReturnNoteModel> ReturnNotes { get; set; }
        public DbSet<ReturnNoteFileModel> ReturnNoteFiles { get; set; }
        public DbSet<IssuedInvoiceModel> IssuedInvoices { get; set; }
        public DbSet<IssuedInvoiceFileModel> IssuedInvoiceFiles { get; set; }
        public DbSet<EshopIssueNoteModel> EshopIssueNotes { get; set; }
    }
}
