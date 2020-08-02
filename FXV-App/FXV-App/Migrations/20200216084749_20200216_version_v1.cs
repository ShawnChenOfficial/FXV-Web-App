using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200216_version_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age_Group",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Test_Result");

            migrationBuilder.AddColumn<double>(
                name: "HigherResult",
                table: "Tests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "HigherScore",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LowerResult",
                table: "Tests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "LowerScore",
                table: "Tests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HigherResult",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "HigherScore",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LowerResult",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LowerScore",
                table: "Tests");

            migrationBuilder.AddColumn<string>(
                name: "Age_Group",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Test_Result",
                nullable: true);
        }
    }
}
