using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200219_version_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWeighted",
                table: "Combine_Categories_Weights");

            migrationBuilder.AddColumn<bool>(
                name: "IsWeighted",
                table: "Combine_Builders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWeighted",
                table: "Combine_Builders");

            migrationBuilder.AddColumn<bool>(
                name: "IsWeighted",
                table: "Combine_Categories_Weights",
                nullable: false,
                defaultValue: false);
        }
    }
}
