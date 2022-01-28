using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LockTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "LockTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Partition",
                table: "LockTransactions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LockTransactions_Partition",
                table: "LockTransactions",
                column: "Partition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LockTransactions_Partition",
                table: "LockTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LockTransactions");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "LockTransactions");

            migrationBuilder.DropColumn(
                name: "Partition",
                table: "LockTransactions");
        }
    }
}
