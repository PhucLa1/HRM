using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Payrolls_PayrollId",
                table: "Histories");

            migrationBuilder.AlterColumn<int>(
                name: "PayrollId",
                table: "Histories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Histories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Histories_EmployeeId",
                table: "Histories",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Employees_EmployeeId",
                table: "Histories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Payrolls_PayrollId",
                table: "Histories",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Employees_EmployeeId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Payrolls_PayrollId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_EmployeeId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Histories");

            migrationBuilder.AlterColumn<int>(
                name: "PayrollId",
                table: "Histories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Payrolls_PayrollId",
                table: "Histories",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
