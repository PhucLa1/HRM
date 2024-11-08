using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixtestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Tests_TestId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TestId",
                table: "Applicants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Applicants",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Tests_TestId",
                table: "Applicants",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Tests_TestId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TestResults");

            migrationBuilder.AddColumn<double>(
                name: "Point",
                table: "Questions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "TestId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Applicants",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Tests_TestId",
                table: "Applicants",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
