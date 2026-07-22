using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseLeaveManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnEmployeeUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");
        }
    }
}
