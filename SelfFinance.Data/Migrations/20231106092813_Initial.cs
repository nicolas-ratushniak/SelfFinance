using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfFinance.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsIncome = table.Column<bool>(type: "bit", nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(2,2)", precision: 2, nullable: false),
                    IncomeTagId = table.Column<int>(type: "int", nullable: true),
                    ExpenseTagId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialOperations_ExpenseTags_ExpenseTagId",
                        column: x => x.ExpenseTagId,
                        principalTable: "ExpenseTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialOperations_IncomeTags_IncomeTagId",
                        column: x => x.IncomeTagId,
                        principalTable: "IncomeTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_ExpenseTagId",
                table: "FinancialOperations",
                column: "ExpenseTagId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialOperations_IncomeTagId",
                table: "FinancialOperations",
                column: "IncomeTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialOperations");

            migrationBuilder.DropTable(
                name: "ExpenseTags");

            migrationBuilder.DropTable(
                name: "IncomeTags");
        }
    }
}
