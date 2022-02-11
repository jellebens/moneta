using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstrumentService.Migrations
{
    public partial class RenameTickerToSymbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ticker",
                schema: "instruments",
                table: "Instrument",
                newName: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Symbol",
                schema: "instruments",
                table: "Instrument",
                newName: "Ticker");
        }
    }
}
