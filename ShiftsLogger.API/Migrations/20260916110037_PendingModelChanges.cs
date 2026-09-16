using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftsLogger.API.Migrations
{
    /// <inheritdoc />
    public partial class PendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Shifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Shifts");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
