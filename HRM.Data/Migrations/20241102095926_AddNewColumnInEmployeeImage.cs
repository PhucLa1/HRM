using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnInEmployeeImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UUID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "EmployeeImages");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "EmployeeImages",
                newName: "Descriptor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descriptor",
                table: "EmployeeImages",
                newName: "Uri");

            migrationBuilder.AddColumn<string>(
                name: "UUID",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UUID",
                table: "EmployeeImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
