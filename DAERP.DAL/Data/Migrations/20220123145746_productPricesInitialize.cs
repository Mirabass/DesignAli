using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class productPricesInitialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductPricesId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryNotePrice",
                table: "CustomersProducts",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IssuedInvoicePrice",
                table: "CustomersProducts",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "CustomersProducts",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductPricesModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatedCostPrice = table.Column<decimal>(type: "money", nullable: false),
                    OperatedSellingPrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPricesModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductPricesId",
                table: "Products",
                column: "ProductPricesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPricesModel_ProductPricesId",
                table: "Products",
                column: "ProductPricesId",
                principalTable: "ProductPricesModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPricesModel_ProductPricesId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductPricesModel");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductPricesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPricesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryNotePrice",
                table: "CustomersProducts");

            migrationBuilder.DropColumn(
                name: "IssuedInvoicePrice",
                table: "CustomersProducts");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "CustomersProducts");
        }
    }
}
