using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditTokenCreations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalSupply = table.Column<long>(type: "bigint", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Decimal = table.Column<int>(type: "int", nullable: false),
                    Contract = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationStatus = table.Column<int>(type: "int", nullable: false),
                    CreationRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Partition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTokenCreations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contract = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Decimal = table.Column<int>(type: "int", nullable: false),
                    Partition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditTokenCreations_Contract",
                table: "AuditTokenCreations",
                column: "Contract");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTokenCreations_Partition",
                table: "AuditTokenCreations",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTokenCreations_Sender",
                table: "AuditTokenCreations",
                column: "Sender");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTokenCreations_Symbol",
                table: "AuditTokenCreations",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTokenCreations_TransactionHash",
                table: "AuditTokenCreations",
                column: "TransactionHash");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTokenCreations");

            migrationBuilder.DropTable(
                name: "Tokens");
        }
    }
}
