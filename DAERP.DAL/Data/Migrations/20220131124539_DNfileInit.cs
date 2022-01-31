using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class DNfileInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryNoteFileModelId",
                table: "DeliveryNotes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryNoteFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryNoteNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExcelFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryNoteFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryNoteFiles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNotes_DeliveryNoteFileModelId",
                table: "DeliveryNotes",
                column: "DeliveryNoteFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryNoteFiles_CustomerId",
                table: "DeliveryNoteFiles",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNotes_DeliveryNoteFiles_DeliveryNoteFileModelId",
                table: "DeliveryNotes",
                column: "DeliveryNoteFileModelId",
                principalTable: "DeliveryNoteFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNotes_DeliveryNoteFiles_DeliveryNoteFileModelId",
                table: "DeliveryNotes");

            migrationBuilder.DropTable(
                name: "DeliveryNoteFiles");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryNotes_DeliveryNoteFileModelId",
                table: "DeliveryNotes");

            migrationBuilder.DropColumn(
                name: "DeliveryNoteFileModelId",
                table: "DeliveryNotes");
        }
    }
}
