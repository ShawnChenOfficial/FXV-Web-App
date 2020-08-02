using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200218_version_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Combine_Categories_Weights",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "C_ID",
                table: "Combine_Categories_Weights",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Categories_Weights_C_ID",
                table: "Combine_Categories_Weights",
                column: "C_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Categories_Weights_Combines_C_ID",
                table: "Combine_Categories_Weights",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Categories_Weights_Combines_C_ID",
                table: "Combine_Categories_Weights");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Categories_Weights_C_ID",
                table: "Combine_Categories_Weights");

            migrationBuilder.DropColumn(
                name: "C_ID",
                table: "Combine_Categories_Weights");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Combine_Categories_Weights",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
