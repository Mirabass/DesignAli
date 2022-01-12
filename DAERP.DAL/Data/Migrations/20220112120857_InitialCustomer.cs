using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class InitialCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Franchise = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    SFName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    SFStreetAndNo = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    SFZIP = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    SFCity = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    SFCountry = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    SFIN = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    SFTIN = table.Column<string>(type: "nvarchar(12)", nullable: true),
                    ProvisionFor60PercentValue = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    FVDiscountPercentValue = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    Maturity = table.Column<decimal>(type: "numeric(2,0)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    RoundPriceWithVAT = table.Column<bool>(type: "bit", nullable: false),
                    DFName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    DFContactPerson = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFStreetAndNo = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFZIP = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    DFCity = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFCountry = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    DFPhone = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFMobile = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFEmail = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFIN = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    DFTIN = table.Column<string>(type: "nvarchar(12)", nullable: true),
                    DFBank = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFAccountNumber = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    DFBIC = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    MDName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    MDContactPerson = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    MDStreetAndNo = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    MDZIP = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    MDCity = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractDANumber = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractONumber = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractContent = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractPoPro = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractPoUm = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractDateSigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractDateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractDateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractRent = table.Column<decimal>(type: "money", nullable: false),
                    ContractPeriod = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ContractProvisionPercentValue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
