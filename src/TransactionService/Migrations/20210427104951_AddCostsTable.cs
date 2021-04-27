using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionService.Migrations
{
    public partial class AddCostsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                schema: "transactions",
                name: "costs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    BuyorderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cost_buyorder_BuyorderId",
                        column: x => x.BuyorderId,
                        principalSchema: "transactions",
                        principalTable: "buyorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cost_BuyorderId",
                schema: "transactions",
                table: "costs",
                column: "BuyorderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cost");
        }
    }
}
