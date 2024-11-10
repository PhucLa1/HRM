using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaxDeduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FomulaType",
                table: "TaxDeductions");

            migrationBuilder.DropColumn(
                name: "ApplyAt",
                table: "TaxDeductionDetails");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "TaxDeductionDetails");

            migrationBuilder.DropColumn(
                name: "InUsed",
                table: "TaxDeductionDetails");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "TaxDeductionDetails",
                newName: "Factor");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "TaxDeductions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TaxDeductions");

            migrationBuilder.RenameColumn(
                name: "Factor",
                table: "TaxDeductionDetails",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "FomulaType",
                table: "TaxDeductions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplyAt",
                table: "TaxDeductionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "TaxDeductionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InUsed",
                table: "TaxDeductionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
