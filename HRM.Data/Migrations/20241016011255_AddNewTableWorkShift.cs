using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableWorkShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCalendars_PartimePlans_PartimePlanId",
                table: "UserCalendars");

            migrationBuilder.DropIndex(
                name: "IX_UserCalendars_PartimePlanId",
                table: "UserCalendars");

            migrationBuilder.DropColumn(
                name: "PartimePlanId",
                table: "UserCalendars");

            migrationBuilder.CreateTable(
                name: "WorkShift",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartimePlanId = table.Column<int>(type: "int", nullable: false),
                    UserCalendarId = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShift", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkShift_PartimePlans_PartimePlanId",
                        column: x => x.PartimePlanId,
                        principalTable: "PartimePlans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkShift_UserCalendars_UserCalendarId",
                        column: x => x.UserCalendarId,
                        principalTable: "UserCalendars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkShift_PartimePlanId",
                table: "WorkShift",
                column: "PartimePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkShift_UserCalendarId",
                table: "WorkShift",
                column: "UserCalendarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkShift");

            migrationBuilder.AddColumn<int>(
                name: "PartimePlanId",
                table: "UserCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendars_PartimePlanId",
                table: "UserCalendars",
                column: "PartimePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCalendars_PartimePlans_PartimePlanId",
                table: "UserCalendars",
                column: "PartimePlanId",
                principalTable: "PartimePlans",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
