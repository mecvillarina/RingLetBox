using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StandardTokens_ContractAddress",
                table: "StandardTokens");

            migrationBuilder.DropIndex(
                name: "IX_StandardTokens_Sender",
                table: "StandardTokens");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "StandardTokens");

            migrationBuilder.CreateTable(
                name: "SenderTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Decimal = table.Column<int>(type: "int", nullable: false),
                    Partition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_ContractAddress",
                table: "StandardTokens",
                column: "ContractAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SenderTokens_ContractAddress",
                table: "SenderTokens",
                column: "ContractAddress");

            migrationBuilder.CreateIndex(
                name: "IX_SenderTokens_Partition",
                table: "SenderTokens",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_SenderTokens_Sender",
                table: "SenderTokens",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_SenderTokens_Symbol",
                table: "SenderTokens",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenderTokens");

            migrationBuilder.DropIndex(
                name: "IX_StandardTokens_ContractAddress",
                table: "StandardTokens");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "StandardTokens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_ContractAddress",
                table: "StandardTokens",
                column: "ContractAddress");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_Sender",
                table: "StandardTokens",
                column: "Sender");
        }
    }
}
