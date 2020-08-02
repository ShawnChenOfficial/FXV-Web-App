using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200218_version_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combine_Categories_Weights",
                columns: table => new
                {
                    CC_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TC_ID = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combine_Categories_Weights", x => x.CC_ID);
                    table.ForeignKey(
                        name: "FK_Combine_Categories_Weights_Test_Categories_TC_ID",
                        column: x => x.TC_ID,
                        principalTable: "Test_Categories",
                        principalColumn: "TC_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Categories_Weights_TC_ID",
                table: "Combine_Categories_Weights",
                column: "TC_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Combine_Categories_Weights");
        }
    }
}
