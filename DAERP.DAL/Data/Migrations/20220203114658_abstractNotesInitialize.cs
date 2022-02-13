using Microsoft.EntityFrameworkCore.Migrations;

namespace DAERP.DAL.Migrations
{
    public partial class abstractNotesInitialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNoteFiles_Customers_CustomerId",
                table: "DeliveryNoteFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNotes_Customers_CustomerId",
                table: "DeliveryNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNotes_DeliveryNoteFiles_DeliveryNoteFileModelId",
                table: "DeliveryNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNotes_Products_ProductId",
                table: "DeliveryNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryNotes",
                table: "DeliveryNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryNoteFiles",
                table: "DeliveryNoteFiles");

            migrationBuilder.RenameTable(
                name: "DeliveryNotes",
                newName: "Notes");

            migrationBuilder.RenameTable(
                name: "DeliveryNoteFiles",
                newName: "NoteFiles");

            migrationBuilder.RenameColumn(
                name: "DeliveryNoteFileModelId",
                table: "Notes",
                newName: "NoteFileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryNotes_ProductId",
                table: "Notes",
                newName: "IX_Notes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryNotes_DeliveryNoteFileModelId",
                table: "Notes",
                newName: "IX_Notes_NoteFileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryNotes_CustomerId",
                table: "Notes",
                newName: "IX_Notes_CustomerId");

            migrationBuilder.RenameColumn(
                name: "DeliveryNoteNumber",
                table: "NoteFiles",
                newName: "NoteNumber");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryNoteFiles_CustomerId",
                table: "NoteFiles",
                newName: "IX_NoteFiles_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "StartingAmount",
                table: "Notes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Remains",
                table: "Notes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainValueWithoutVAT",
                table: "Notes",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "NoteFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteFiles",
                table: "NoteFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteFiles_Customers_CustomerId",
                table: "NoteFiles",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Customers_CustomerId",
                table: "Notes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_NoteFiles_NoteFileModelId",
                table: "Notes",
                column: "NoteFileModelId",
                principalTable: "NoteFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Products_ProductId",
                table: "Notes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteFiles_Customers_CustomerId",
                table: "NoteFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Customers_CustomerId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_NoteFiles_NoteFileModelId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Products_ProductId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteFiles",
                table: "NoteFiles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "NoteFiles");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "DeliveryNotes");

            migrationBuilder.RenameTable(
                name: "NoteFiles",
                newName: "DeliveryNoteFiles");

            migrationBuilder.RenameColumn(
                name: "NoteFileModelId",
                table: "DeliveryNotes",
                newName: "DeliveryNoteFileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_ProductId",
                table: "DeliveryNotes",
                newName: "IX_DeliveryNotes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_NoteFileModelId",
                table: "DeliveryNotes",
                newName: "IX_DeliveryNotes_DeliveryNoteFileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_CustomerId",
                table: "DeliveryNotes",
                newName: "IX_DeliveryNotes_CustomerId");

            migrationBuilder.RenameColumn(
                name: "NoteNumber",
                table: "DeliveryNoteFiles",
                newName: "DeliveryNoteNumber");

            migrationBuilder.RenameIndex(
                name: "IX_NoteFiles_CustomerId",
                table: "DeliveryNoteFiles",
                newName: "IX_DeliveryNoteFiles_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "StartingAmount",
                table: "DeliveryNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Remains",
                table: "DeliveryNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainValueWithoutVAT",
                table: "DeliveryNotes",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryNotes",
                table: "DeliveryNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryNoteFiles",
                table: "DeliveryNoteFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNoteFiles_Customers_CustomerId",
                table: "DeliveryNoteFiles",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNotes_Customers_CustomerId",
                table: "DeliveryNotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNotes_DeliveryNoteFiles_DeliveryNoteFileModelId",
                table: "DeliveryNotes",
                column: "DeliveryNoteFileModelId",
                principalTable: "DeliveryNoteFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNotes_Products_ProductId",
                table: "DeliveryNotes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
