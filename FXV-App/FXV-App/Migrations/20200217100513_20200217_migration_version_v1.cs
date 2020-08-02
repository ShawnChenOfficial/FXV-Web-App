using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200217_migration_version_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TC_id",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Handspan",
                table: "Combine_Results",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Combine_Results",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Standing_Reach",
                table: "Combine_Results",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Combine_Results",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wingspan",
                table: "Combine_Results",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Test_Categories",
                columns: table => new
                {
                    TC_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Categories", x => x.TC_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TC_id",
                table: "Tests",
                column: "TC_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Test_Categories_TC_id",
                table: "Tests",
                column: "TC_id",
                principalTable: "Test_Categories",
                principalColumn: "TC_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Test_Categories_TC_id",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "Test_Categories");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TC_id",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TC_id",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Handspan",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "Standing_Reach",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "Wingspan",
                table: "Combine_Results");
        }
    }
}
