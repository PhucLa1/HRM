using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbJobIdInTableApplicantToPositionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Applicants");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "Applicants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "id");
        }
    }
}
