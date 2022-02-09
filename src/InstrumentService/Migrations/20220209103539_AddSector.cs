using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstrumentService.Migrations
{
    public partial class AddSector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                schema: "instruments",
                table: "Instrument");

            migrationBuilder.AddColumn<string>(
                name: "Exchange",
                schema: "instruments",
                table: "Instrument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SectorId",
                schema: "instruments",
                table: "Instrument",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Sector",
                schema: "instruments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 0, "None" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 1, "Energy" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 2, "Materials" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 3, "Industrials" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 4, "Utilities" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 5, "Healthcare" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 6, "Financials" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 7, "Consumer Discretionary" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 8, "Consumer Staples" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 9, "Information Technology" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 10, "Communication Services" }, "instruments");
            migrationBuilder.InsertData("Sector", new[] { "Id", "Name" }, new object[] { 11, "Real Estate" }, "instruments");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_SectorId",
                schema: "instruments",
                table: "Instrument",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instrument_Sector_SectorId",
                schema: "instruments",
                table: "Instrument",
                column: "SectorId",
                principalSchema: "instruments",
                principalTable: "Sector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instrument_Sector_SectorId",
                schema: "instruments",
                table: "Instrument");

            migrationBuilder.DropTable(
                name: "Sector",
                schema: "instruments");

            migrationBuilder.DropIndex(
                name: "IX_Instrument_SectorId",
                schema: "instruments",
                table: "Instrument");

            migrationBuilder.DropColumn(
                name: "Exchange",
                schema: "instruments",
                table: "Instrument");

            migrationBuilder.DropColumn(
                name: "SectorId",
                schema: "instruments",
                table: "Instrument");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "instruments",
                table: "Instrument",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
