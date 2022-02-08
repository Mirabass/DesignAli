using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class returnNoteUpdate003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductReceiptModel_Amount",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReturnNoteModel_DeliveryNoteId",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnNoteModel_DeliveryNoteNumber",
                table: "Movements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductReceiptModel_Amount",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "ReturnNoteModel_DeliveryNoteId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "ReturnNoteModel_DeliveryNoteNumber",
                table: "Movements");
        }
    }
}
