using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class abstractNotesUpdate003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReceipts_Products_ProductId",
                table: "ProductReceipts");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReceipts",
                table: "ProductReceipts");

            migrationBuilder.RenameTable(
                name: "ProductReceipts",
                newName: "Movements");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReceipts_ProductId",
                table: "Movements",
                newName: "IX_Movements_ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "Movements",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Movements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryNotePrice",
                table: "Movements",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IssuedInvoicePrice",
                table: "Movements",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovementType",
                table: "Movements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoteFileModelId",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainValueWithoutVAT",
                table: "Movements",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Remains",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReturnNoteModel_Amount",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartingAmount",
                table: "Movements",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movements",
                table: "Movements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CustomerId",
                table: "Movements",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_NoteFileModelId",
                table: "Movements",
                column: "NoteFileModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Customers_CustomerId",
                table: "Movements",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_NoteFiles_NoteFileModelId",
                table: "Movements",
                column: "NoteFileModelId",
                principalTable: "NoteFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Products_ProductId",
                table: "Movements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Customers_CustomerId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_NoteFiles_NoteFileModelId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Products_ProductId",
                table: "Movements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movements",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_CustomerId",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_NoteFileModelId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "DeliveryNotePrice",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "IssuedInvoicePrice",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "MovementType",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "NoteFileModelId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "RemainValueWithoutVAT",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "Remains",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "ReturnNoteModel_Amount",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "StartingAmount",
                table: "Movements");

            migrationBuilder.RenameTable(
                name: "Movements",
                newName: "ProductReceipts");

            migrationBuilder.RenameIndex(
                name: "IX_Movements_ProductId",
                table: "ProductReceipts",
                newName: "IX_ProductReceipts_ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "ProductReceipts",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "ProductReceipts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReceipts",
                table: "ProductReceipts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryNotePrice = table.Column<decimal>(type: "money", nullable: false),
                    IssuedInvoicePrice = table.Column<decimal>(type: "money", nullable: false),
                    NoteFileModelId = table.Column<int>(type: "int", nullable: true),
                    NoteType = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderInCurrentYear = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ValueWithVAT = table.Column<decimal>(type: "money", nullable: false),
                    ValueWithoutVAT = table.Column<decimal>(type: "money", nullable: false),
                    RemainValueWithoutVAT = table.Column<decimal>(type: "money", nullable: true),
                    Remains = table.Column<int>(type: "int", nullable: true),
                    StartingAmount = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_NoteFiles_NoteFileModelId",
                        column: x => x.NoteFileModelId,
                        principalTable: "NoteFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CustomerId",
                table: "Notes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NoteFileModelId",
                table: "Notes",
                column: "NoteFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ProductId",
                table: "Notes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReceipts_Products_ProductId",
                table: "ProductReceipts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
