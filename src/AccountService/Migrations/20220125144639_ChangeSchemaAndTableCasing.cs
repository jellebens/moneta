using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Migrations
{
    public partial class ChangeSchemaAndTableCasing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "accounts");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "account",
                newName: "Account",
                newSchema: "accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "accounts",
                newName: "Account",
                newSchema: "account");
        }
    }
}
