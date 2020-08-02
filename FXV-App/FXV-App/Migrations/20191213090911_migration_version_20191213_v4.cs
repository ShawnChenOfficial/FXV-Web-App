using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class migration_version_20191213_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Test_Result",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Test_Result");
        }
    }
}
