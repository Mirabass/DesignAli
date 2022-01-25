using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class ProductsUpdate005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainStockAmount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MainStockValue",
                table: "Products",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "GainPercentValue",
                table: "ProductPrices",
                type: "decimal(4,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainStockAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainStockValue",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "GainPercentValue",
                table: "ProductPrices",
                type: "decimal(3,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,1)");
        }
    }
}
