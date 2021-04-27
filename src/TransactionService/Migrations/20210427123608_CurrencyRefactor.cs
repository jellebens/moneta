using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionService.Migrations
{
    public partial class CurrencyRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "transactions",
                table: "amount");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "transactions",
                table: "buyorder",
                type: "char(3)",
                maxLength: 3,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "transactions",
                table: "buyorder");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema:"transactions",
                table: "amount",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
