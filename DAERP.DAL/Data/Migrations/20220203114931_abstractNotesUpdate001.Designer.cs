﻿// <auto-generated />
using System;
using DAERP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAERP.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220203114931_abstractNotesUpdate001")]
    partial class abstractNotesUpdate001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAERP.BL.Models.CustomerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("ContractContent")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ContractDANumber")
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ContractDateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ContractDateSigned")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ContractDateTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("ContractONumber")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ContractPeriod")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ContractPoPro")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ContractPoUm")
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("ContractProvisionPercentValue")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal?>("ContractRent")
                        .HasColumnType("money");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("DFAccountNumber")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFBIC")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFBank")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFCity")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFContactPerson")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFCountry")
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("DFEmail")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFIN")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("DFMobile")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFPhone")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFStreetAndNo")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("DFTIN")
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("DFZIP")
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.Property<decimal>("FVDiscountPercentValue")
                        .HasColumnType("decimal(3,1)");

                    b.Property<string>("Franchise")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("MDCity")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("MDContactPerson")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("MDName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("MDStreetAndNo")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("MDZIP")
                        .HasColumnType("nvarchar(6)");

                    b.Property<decimal>("Maturity")
                        .HasColumnType("numeric(2,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("ProvisionFor60PercentValue")
                        .HasColumnType("decimal(3,1)");

                    b.Property<bool>("RoundPriceWithVAT")
                        .HasColumnType("bit");

                    b.Property<string>("SFCity")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("SFCountry")
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("SFIN")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("SFName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("SFStreetAndNo")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("SFTIN")
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("SFZIP")
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DAERP.BL.Models.CustomerProductModel", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("AmountInStock")
                        .HasColumnType("int");

                    b.Property<decimal>("DeliveryNotePrice")
                        .HasColumnType("money");

                    b.Property<decimal>("IssuedInvoicePrice")
                        .HasColumnType("money");

                    b.Property<decimal>("Value")
                        .HasColumnType("money");

                    b.HasKey("CustomerId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CustomersProducts");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.NoteFileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ExcelFile")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Finished")
                        .HasColumnType("bit");

                    b.Property<string>("NoteNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NoteFiles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("NoteFileModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.NoteModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DeliveryNotePrice")
                        .HasColumnType("money");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("IssuedInvoicePrice")
                        .HasColumnType("money");

                    b.Property<int?>("NoteFileModelId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderInCurrentYear")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("ValueWithVAT")
                        .HasColumnType("money");

                    b.Property<decimal>("ValueWithoutVAT")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("NoteFileModelId");

                    b.HasIndex("ProductId");

                    b.ToTable("Notes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("NoteModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.ProductReceiptModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<decimal>("CostPrice")
                        .HasColumnType("money");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderInCurrentYear")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("ValueWithVAT")
                        .HasColumnType("money");

                    b.Property<decimal>("ValueWithoutVAT")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductReceipts");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductColorDesignModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MainPartColorName")
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal?>("MainPartRAL")
                        .HasColumnType("numeric(4)");

                    b.Property<string>("Orientation")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PocketColorName")
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("PocketRAL")
                        .HasColumnType("numeric(4)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductColorDesigns");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductDivisionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Number")
                        .HasColumnType("numeric(3,0)");

                    b.Property<int?>("ProductKindId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductMaterialId")
                        .HasColumnType("int");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ProductKindId");

                    b.HasIndex("ProductMaterialId");

                    b.ToTable("ProductDivisions");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductImageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductKindModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductKinds");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductMaterialModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductMaterials");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Accessories")
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastModified")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Design")
                        .HasColumnType("numeric(4)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<long>("EAN")
                        .HasColumnType("bigint");

                    b.Property<int>("MainStockAmount")
                        .HasColumnType("int");

                    b.Property<decimal>("MainStockValue")
                        .HasColumnType("money");

                    b.Property<string>("Motive")
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int?>("ProductColorDesignId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductDivisionId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductImageId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductPricesId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductStrapId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductColorDesignId");

                    b.HasIndex("ProductDivisionId");

                    b.HasIndex("ProductImageId");

                    b.HasIndex("ProductPricesId");

                    b.HasIndex("ProductStrapId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductPricesModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("GainPercentValue")
                        .HasColumnType("decimal(4,1)");

                    b.Property<decimal>("OperatedCostPrice")
                        .HasColumnType("money");

                    b.Property<decimal>("OperatedSellingPrice")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductStrapModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attachment")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ColorName")
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal?>("Length")
                        .HasColumnType("numeric(5,3)");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("RAL")
                        .HasColumnType("numeric(4)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("Width")
                        .HasColumnType("numeric(5,3)");

                    b.HasKey("Id");

                    b.ToTable("ProductStraps");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.DeliveryNoteFileModel", b =>
                {
                    b.HasBaseType("DAERP.BL.Models.Files.NoteFileModel");

                    b.HasIndex("CustomerId");

                    b.HasDiscriminator().HasValue("DeliveryNoteFileModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.ReturnNoteFileModel", b =>
                {
                    b.HasBaseType("DAERP.BL.Models.Files.NoteFileModel");

                    b.HasIndex("CustomerId");

                    b.HasDiscriminator().HasValue("ReturnNoteFileModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.DeliveryNoteModel", b =>
                {
                    b.HasBaseType("DAERP.BL.Models.Movements.NoteModel");

                    b.Property<decimal>("RemainValueWithoutVAT")
                        .HasColumnType("money");

                    b.Property<int>("Remains")
                        .HasColumnType("int");

                    b.Property<int>("StartingAmount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("DeliveryNoteModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.ReturnNoteModel", b =>
                {
                    b.HasBaseType("DAERP.BL.Models.Movements.NoteModel");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("ReturnNoteModel");
                });

            modelBuilder.Entity("DAERP.BL.Models.CustomerProductModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.CustomerModel", "Customer")
                        .WithMany("CustomerProducts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAERP.BL.Models.Product.ProductModel", "Product")
                        .WithMany("ProductCustomers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.NoteModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.CustomerModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAERP.BL.Models.Files.NoteFileModel", null)
                        .WithMany("Notes")
                        .HasForeignKey("NoteFileModelId");

                    b.HasOne("DAERP.BL.Models.Product.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DAERP.BL.Models.Movements.ProductReceiptModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.Product.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductDivisionModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.Product.ProductKindModel", "ProductKind")
                        .WithMany()
                        .HasForeignKey("ProductKindId");

                    b.HasOne("DAERP.BL.Models.Product.ProductMaterialModel", "ProductMaterial")
                        .WithMany()
                        .HasForeignKey("ProductMaterialId");

                    b.Navigation("ProductKind");

                    b.Navigation("ProductMaterial");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.Product.ProductColorDesignModel", "ProductColorDesign")
                        .WithMany()
                        .HasForeignKey("ProductColorDesignId");

                    b.HasOne("DAERP.BL.Models.Product.ProductDivisionModel", "ProductDivision")
                        .WithMany()
                        .HasForeignKey("ProductDivisionId");

                    b.HasOne("DAERP.BL.Models.Product.ProductImageModel", "ProductImage")
                        .WithMany()
                        .HasForeignKey("ProductImageId");

                    b.HasOne("DAERP.BL.Models.Product.ProductPricesModel", "ProductPrices")
                        .WithMany()
                        .HasForeignKey("ProductPricesId");

                    b.HasOne("DAERP.BL.Models.Product.ProductStrapModel", "ProductStrap")
                        .WithMany()
                        .HasForeignKey("ProductStrapId");

                    b.Navigation("ProductColorDesign");

                    b.Navigation("ProductDivision");

                    b.Navigation("ProductImage");

                    b.Navigation("ProductPrices");

                    b.Navigation("ProductStrap");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.DeliveryNoteFileModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.CustomerModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.ReturnNoteFileModel", b =>
                {
                    b.HasOne("DAERP.BL.Models.CustomerModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DAERP.BL.Models.CustomerModel", b =>
                {
                    b.Navigation("CustomerProducts");
                });

            modelBuilder.Entity("DAERP.BL.Models.Files.NoteFileModel", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("DAERP.BL.Models.Product.ProductModel", b =>
                {
                    b.Navigation("ProductCustomers");
                });
#pragma warning restore 612, 618
        }
    }
}
