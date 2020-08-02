using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200220_version_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results",
                column: "C_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
