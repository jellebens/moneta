using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transactions");

            migrationBuilder.CreateTable(
                schema: "transactions",
                name: "amount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,5)", nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: true),
                    Exchangerate = table.Column<decimal>(type: "decimal(19,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "buyorder",
                schema: "transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyorder", x => x.Id);
                    table.UniqueConstraint("AK_buyorder_AccountId_Number", x => new { x.AccountId, x.Number })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_buyorder_Amount_AmountId",
                        column: x => x.AmountId,
                        principalSchema: "transactions",
                        principalTable: "amount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_buyorder_AmountId",
                schema: "transactions",
                table: "buyorder",
                column: "AmountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "buyorder",
                schema: "transactions");

            migrationBuilder.DropTable(
                name: "Amount");
        }
    }
}
