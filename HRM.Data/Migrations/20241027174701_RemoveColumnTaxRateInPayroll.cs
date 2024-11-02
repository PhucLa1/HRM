using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnTaxRateInPayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payrolls_TaxRates_TaxRateId",
                table: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Payrolls_TaxRateId",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "TaxRateId",
                table: "Payrolls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxRateId",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_TaxRateId",
                table: "Payrolls",
                column: "TaxRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payrolls_TaxRates_TaxRateId",
                table: "Payrolls",
                column: "TaxRateId",
                principalTable: "TaxRates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
