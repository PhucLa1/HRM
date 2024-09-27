using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteJobTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Jobs_JobId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Jobs_JobId",
                table: "JobPostings");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_JobId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_JobId",
                table: "Applicants");

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "JobPostings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_PositionId",
                table: "JobPostings",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_PositionId",
                table: "Applicants",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Positions_PositionId",
                table: "JobPostings",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Positions_PositionId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Positions_PositionId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_PositionId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_PositionId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Applicants");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_JobId",
                table: "JobPostings",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_JobId",
                table: "Applicants",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Jobs_JobId",
                table: "Applicants",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Jobs_JobId",
                table: "JobPostings",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
