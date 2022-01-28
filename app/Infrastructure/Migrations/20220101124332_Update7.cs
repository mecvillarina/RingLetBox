using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LockTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LockTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClaimedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<long>(type: "bigint", nullable: false),
                    EndTime = table.Column<long>(type: "bigint", nullable: false),
                    InitiatorAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsClaimed = table.Column<bool>(type: "bit", nullable: false),
                    IsRefunded = table.Column<bool>(type: "bit", nullable: false),
                    IsRevocable = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    LockContractAddress = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LockTransactionIndex = table.Column<long>(type: "bigint", nullable: false),
                    RecipientAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<long>(type: "bigint", nullable: false),
                    TokenAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_InitiatorAddress",
                table: "LockTransactions",
                column: "InitiatorAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_LockContractAddress",
                table: "LockTransactions",
                column: "LockContractAddress");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_RecipientAddress",
                table: "LockTransactions",
                column: "RecipientAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_TokenAddress",
                table: "LockTransactions",
                column: "TokenAddress");
        }
    }
}
