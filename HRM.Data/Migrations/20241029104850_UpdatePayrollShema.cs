using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayrollShema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPeriod",
                table: "Payrolls");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Payrolls");

            migrationBuilder.AddColumn<string>(
                name: "PayPeriod",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
