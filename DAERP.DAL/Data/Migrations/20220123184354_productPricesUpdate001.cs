using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class productPricesUpdate001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPricesModel_ProductPricesId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPricesModel",
                table: "ProductPricesModel");

            migrationBuilder.RenameTable(
                name: "ProductPricesModel",
                newName: "ProductPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPrices_ProductPricesId",
                table: "Products",
                column: "ProductPricesId",
                principalTable: "ProductPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPrices_ProductPricesId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "ProductPricesModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPricesModel",
                table: "ProductPricesModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPricesModel_ProductPricesId",
                table: "Products",
                column: "ProductPricesId",
                principalTable: "ProductPricesModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
