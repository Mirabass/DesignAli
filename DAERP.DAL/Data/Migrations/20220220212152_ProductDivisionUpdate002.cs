using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class ProductDivisionUpdate002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDivisions_ProductDivisionId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductDivisionId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDivisions_ProductDivisionId",
                table: "Products",
                column: "ProductDivisionId",
                principalTable: "ProductDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDivisions_ProductDivisionId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductDivisionId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDivisions_ProductDivisionId",
                table: "Products",
                column: "ProductDivisionId",
                principalTable: "ProductDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
