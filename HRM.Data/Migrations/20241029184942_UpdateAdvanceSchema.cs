using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdvanceSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPeriod",
                table: "Advances");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Advances");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Advances");

            migrationBuilder.AddColumn<string>(
                name: "PayPeriod",
                table: "Advances",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
