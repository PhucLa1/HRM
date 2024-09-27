using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmployeeIdToInteviewerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Employees_EmployeeId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_EmployeeId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Applicants");

            migrationBuilder.AddColumn<string>(
                name: "InterviewerName",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewerName",
                table: "Applicants");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_EmployeeId",
                table: "Applicants",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Employees_EmployeeId",
                table: "Applicants",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
