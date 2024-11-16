using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class FlexibleDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "JobPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_DepartmentId",
                table: "JobPostings",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Departments_DepartmentId",
                table: "JobPostings",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Departments_DepartmentId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_DepartmentId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "JobPostings");
        }
    }
}
