using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAWebERP1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductColorDesigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Orientation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPartRAL = table.Column<int>(type: "int", nullable: false),
                    MainPartColorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PocketRAL = table.Column<int>(type: "int", nullable: false),
                    PocketColorName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColorDesigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStraps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAL = table.Column<int>(type: "int", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStraps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductKindId = table.Column<int>(type: "int", nullable: true),
                    ProductMaterialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDivisions_ProductKinds_ProductKindId",
                        column: x => x.ProductKindId,
                        principalTable: "ProductKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDivisions_ProductMaterials_ProductMaterialId",
                        column: x => x.ProductMaterialId,
                        principalTable: "ProductMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    EAN = table.Column<long>(type: "bigint", maxLength: 13, nullable: false),
                    ProductDivisionId = table.Column<int>(type: "int", nullable: true),
                    ProductColorDesignId = table.Column<int>(type: "int", nullable: true),
                    ProductStrapId = table.Column<int>(type: "int", nullable: true),
                    Design = table.Column<decimal>(type: "numeric(4)", nullable: false),
                    Motive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accessories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductColorDesigns_ProductColorDesignId",
                        column: x => x.ProductColorDesignId,
                        principalTable: "ProductColorDesigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductDivisions_ProductDivisionId",
                        column: x => x.ProductDivisionId,
                        principalTable: "ProductDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductStraps_ProductStrapId",
                        column: x => x.ProductStrapId,
                        principalTable: "ProductStraps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDivisions_ProductKindId",
                table: "ProductDivisions",
                column: "ProductKindId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDivisions_ProductMaterialId",
                table: "ProductDivisions",
                column: "ProductMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductColorDesignId",
                table: "Products",
                column: "ProductColorDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductDivisionId",
                table: "Products",
                column: "ProductDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductStrapId",
                table: "Products",
                column: "ProductStrapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductColorDesigns");

            migrationBuilder.DropTable(
                name: "ProductDivisions");

            migrationBuilder.DropTable(
                name: "ProductStraps");

            migrationBuilder.DropTable(
                name: "ProductKinds");

            migrationBuilder.DropTable(
                name: "ProductMaterials");
        }
    }
}
