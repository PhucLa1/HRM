using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBEmployeeAndContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxDeductions_TaxDeductions_TaxDeductionId",
                table: "TaxDeductions");

            migrationBuilder.DropIndex(
                name: "IX_TaxDeductions_TaxDeductionId",
                table: "TaxDeductions");

            migrationBuilder.DropColumn(
                name: "TaxDeductionId",
                table: "TaxDeductions");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DateHired",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "NationalID",
                table: "Employees",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Contracts",
                newName: "NationalID");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountrySide",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FileUrlSigned",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FireUrlBase",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "NationalAddress",
                table: "Contracts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "NationalStartDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_TaxDeductionDetails_TaxDeductionId",
                table: "TaxDeductionDetails",
                column: "TaxDeductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDeductionDetails_TaxDeductions_TaxDeductionId",
                table: "TaxDeductionDetails",
                column: "TaxDeductionId",
                principalTable: "TaxDeductions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxDeductionDetails_TaxDeductions_TaxDeductionId",
                table: "TaxDeductionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TaxDeductionDetails_TaxDeductionId",
                table: "TaxDeductionDetails");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CountrySide",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FileUrlSigned",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FireUrlBase",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "NationalAddress",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "NationalStartDate",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Employees",
                newName: "NationalID");

            migrationBuilder.RenameColumn(
                name: "NationalID",
                table: "Contracts",
                newName: "FileUrl");

            migrationBuilder.AddColumn<int>(
                name: "TaxDeductionId",
                table: "TaxDeductions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateHired",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaxDeductions_TaxDeductionId",
                table: "TaxDeductions",
                column: "TaxDeductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDeductions_TaxDeductions_TaxDeductionId",
                table: "TaxDeductions",
                column: "TaxDeductionId",
                principalTable: "TaxDeductions",
                principalColumn: "id");
        }
    }
}
