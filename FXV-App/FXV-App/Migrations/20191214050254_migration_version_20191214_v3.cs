using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class migration_version_20191214_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HigherCalc",
                table: "Tests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowerCalc",
                table: "Tests",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HigherCalc",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LowerCalc",
                table: "Tests");
        }
    }
}
