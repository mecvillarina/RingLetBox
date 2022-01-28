using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LockTransactions_InitiatorAddress",
                table: "LockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_LockTransactions_RecipientAddress",
                table: "LockTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_InitiatorAddress",
                table: "LockTransactions",
                column: "InitiatorAddress");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_RecipientAddress",
                table: "LockTransactions",
                column: "RecipientAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LockTransactions_InitiatorAddress",
                table: "LockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_LockTransactions_RecipientAddress",
                table: "LockTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_InitiatorAddress",
                table: "LockTransactions",
                column: "InitiatorAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_RecipientAddress",
                table: "LockTransactions",
                column: "RecipientAddress",
                unique: true);
        }
    }
}
