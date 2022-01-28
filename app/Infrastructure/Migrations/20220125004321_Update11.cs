using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationRemarks",
                table: "AuditTokenCreations");

            migrationBuilder.DropColumn(
                name: "CreationStatus",
                table: "AuditTokenCreations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreationRemarks",
                table: "AuditTokenCreations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreationStatus",
                table: "AuditTokenCreations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
