using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCalendars_Calendars_CalendarId",
                table: "UserCalendars");

            migrationBuilder.DropIndex(
                name: "IX_UserCalendars_CalendarId",
                table: "UserCalendars");

            migrationBuilder.RenameColumn(
                name: "CalendarId",
                table: "UserCalendars",
                newName: "ShiftTime");

            migrationBuilder.AddColumn<int>(
                name: "WorkShiftStatus",
                table: "WorkShift",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkShiftStatus",
                table: "WorkShift");

            migrationBuilder.RenameColumn(
                name: "ShiftTime",
                table: "UserCalendars",
                newName: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendars_CalendarId",
                table: "UserCalendars",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCalendars_Calendars_CalendarId",
                table: "UserCalendars",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
