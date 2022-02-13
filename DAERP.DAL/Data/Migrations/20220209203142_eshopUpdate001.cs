using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class eshopUpdate001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EshopId",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EshopIssueNoteModel_Amount",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatedSellingPrice",
                table: "Movements",
                type: "money",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movements_EshopId",
                table: "Movements",
                column: "EshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Eshops_EshopId",
                table: "Movements",
                column: "EshopId",
                principalTable: "Eshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Eshops_EshopId",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_EshopId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "EshopId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "EshopIssueNoteModel_Amount",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "OperatedSellingPrice",
                table: "Movements");
        }
    }
}
