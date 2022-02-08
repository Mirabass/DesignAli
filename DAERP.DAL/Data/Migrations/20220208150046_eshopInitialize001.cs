using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class eshopInitialize001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SFCountry",
                table: "Customers",
                type: "nvarchar(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DFCountry",
                table: "Customers",
                type: "nvarchar(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Eshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Web = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    SFName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    SFStreetAndNo = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    SFZIP = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    SFCity = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    SFCountry = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    SFIN = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    SFTIN = table.Column<string>(type: "nvarchar(12)", nullable: true),
                    FVDiscountPercentValue = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    Maturity = table.Column<decimal>(type: "numeric(2,0)", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    RoundPriceWithVAT = table.Column<bool>(type: "bit", nullable: true),
                    ContractDANumber = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractONumber = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractContent = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractPoPro = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractPoUm = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractDateSigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractDateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractDateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractRent = table.Column<decimal>(type: "money", nullable: true),
                    ContractPeriod = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractProvisionPercentValue = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eshops", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eshops");

            migrationBuilder.AlterColumn<string>(
                name: "SFCountry",
                table: "Customers",
                type: "nvarchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DFCountry",
                table: "Customers",
                type: "nvarchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldNullable: true);
        }
    }
}
