using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.RenameColumn(
                name: "Contract",
                table: "AuditTokenCreations",
                newName: "ContractAddress");

            migrationBuilder.RenameIndex(
                name: "IX_AuditTokenCreations_Contract",
                table: "AuditTokenCreations",
                newName: "IX_AuditTokenCreations_ContractAddress");

            migrationBuilder.CreateTable(
                name: "StandardTokens",
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
                    table.PrimaryKey("PK_StandardTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_ContractAddress",
                table: "StandardTokens",
                column: "ContractAddress");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_Partition",
                table: "StandardTokens",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_Sender",
                table: "StandardTokens",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTokens_Symbol",
                table: "StandardTokens",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardTokens");

            migrationBuilder.RenameColumn(
                name: "ContractAddress",
                table: "AuditTokenCreations",
                newName: "Contract");

            migrationBuilder.RenameIndex(
                name: "IX_AuditTokenCreations_ContractAddress",
                table: "AuditTokenCreations",
                newName: "IX_AuditTokenCreations_Contract");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contract = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Contract",
                table: "Tokens",
                column: "Contract");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Partition",
                table: "Tokens",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Sender",
                table: "Tokens",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Symbol",
                table: "Tokens",
                column: "Symbol");
        }
    }
}
