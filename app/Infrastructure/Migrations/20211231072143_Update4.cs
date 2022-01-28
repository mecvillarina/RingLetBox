using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenderTokens");

            migrationBuilder.CreateTable(
                name: "HolderStandardTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolderAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_HolderStandardTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolderStandardTokens_ContractAddress",
                table: "HolderStandardTokens",
                column: "ContractAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HolderStandardTokens_HolderAddress",
                table: "HolderStandardTokens",
                column: "HolderAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HolderStandardTokens_Partition",
                table: "HolderStandardTokens",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_HolderStandardTokens_Symbol",
                table: "HolderStandardTokens",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolderStandardTokens");

            migrationBuilder.CreateTable(
                name: "SenderTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Decimal = table.Column<int>(type: "int", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Partition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderTokens", x => x.Id);
                });

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
    }
}
