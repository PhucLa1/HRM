using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInterviewer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterviewerId",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_InterviewerId",
                table: "Applicants",
                column: "InterviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Employees_InterviewerId",
                table: "Applicants",
                column: "InterviewerId",
                principalTable: "Employees",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Employees_InterviewerId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_InterviewerId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "InterviewerId",
                table: "Applicants");
        }
    }
}
