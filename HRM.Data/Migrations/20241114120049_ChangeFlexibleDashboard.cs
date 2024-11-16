using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFlexibleDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "PageFlexibleDashboards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageFlexibleDashboards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageFlexibleDashboardId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChartType = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Charts_PageFlexibleDashboards_PageFlexibleDashboardId",
                        column: x => x.PageFlexibleDashboardId,
                        principalTable: "PageFlexibleDashboards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_PageFlexibleDashboardId",
                table: "Charts",
                column: "PageFlexibleDashboardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.DropTable(
                name: "PageFlexibleDashboards");

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
    }
}
