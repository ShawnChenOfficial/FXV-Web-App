using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20191210074622_migration_version_20191212_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Splites",
                table: "Tests");

            migrationBuilder.AddColumn<bool>(
                name: "HasSplit",
                table: "Tests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Attempt",
                table: "Combine_Builders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Splits_Tests",
                columns: table => new
                {
                    ST_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IT_ID = table.Column<int>(nullable: false),
                    Individual_TestsIT_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Splits_Tests", x => x.ST_ID);
                    table.ForeignKey(
                        name: "FK_Splits_Tests_Individual_Tests_Individual_TestsIT_ID",
                        column: x => x.Individual_TestsIT_ID,
                        principalTable: "Individual_Tests",
                        principalColumn: "IT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Splits_Tests_Individual_TestsIT_ID",
                table: "Splits_Tests",
                column: "Individual_TestsIT_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Splits_Tests");

            migrationBuilder.DropColumn(
                name: "HasSplit",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Attempt",
                table: "Combine_Builders");

            migrationBuilder.AddColumn<string>(
                name: "Splites",
                table: "Tests",
                nullable: true);
        }
    }
}
