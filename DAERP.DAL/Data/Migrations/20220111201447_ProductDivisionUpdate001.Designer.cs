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
    [Migration("20220111201447_ProductDivisionUpdate001")]
    partial class ProductDivisionUpdate001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<string>("Motive")
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int?>("ProductColorDesignId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductDivisionId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductStrapId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductColorDesignId");

                    b.HasIndex("ProductDivisionId");

                    b.HasIndex("ProductStrapId");

                    b.ToTable("Products");
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

                    b.HasOne("DAERP.BL.Models.Product.ProductStrapModel", "ProductStrap")
                        .WithMany()
                        .HasForeignKey("ProductStrapId");

                    b.Navigation("ProductColorDesign");

                    b.Navigation("ProductDivision");

                    b.Navigation("ProductStrap");
                });
#pragma warning restore 612, 618
        }
    }
}
