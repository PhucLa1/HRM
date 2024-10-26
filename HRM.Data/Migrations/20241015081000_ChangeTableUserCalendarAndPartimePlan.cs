using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableUserCalendarAndPartimePlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "PartimePlans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PartimePlans");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "PartimePlans");

            migrationBuilder.AddColumn<DateOnly>(
                name: "PresentShift",
                table: "UserCalendars",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "TimeEnd",
                table: "PartimePlans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "TimeStart",
                table: "PartimePlans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresentShift",
                table: "UserCalendars");

            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "PartimePlans");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "PartimePlans");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "PartimePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PartimePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "PartimePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
