using Microsoft.EntityFrameworkCore.Migrations;

namespace DAWebERP1.Migrations
{
    public partial class ProductsUpdate002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "ProductStraps",
                type: "numeric(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RAL",
                table: "ProductStraps",
                type: "numeric(4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "ProductStraps",
                type: "numeric(5,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,3)");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ProductMaterials",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ProductKinds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PocketRAL",
                table: "ProductColorDesigns",
                type: "numeric(4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MainPartRAL",
                table: "ProductColorDesigns",
                type: "numeric(4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "ProductStraps",
                type: "numeric(5,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RAL",
                table: "ProductStraps",
                type: "numeric(4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "ProductStraps",
                type: "numeric(5,3)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ProductMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "ProductKinds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PocketRAL",
                table: "ProductColorDesigns",
                type: "numeric(4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MainPartRAL",
                table: "ProductColorDesigns",
                type: "numeric(4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(4)",
                oldNullable: true);
        }
    }
}
