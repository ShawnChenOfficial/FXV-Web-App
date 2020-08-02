using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200220_version_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Wingspan",
                table: "Combine_Results",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Combine_Results",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Standing_Reach",
                table: "Combine_Results",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "Combine_Results",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Handspan",
                table: "Combine_Results",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "C_ID",
                table: "Combine_Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Dominant_Hand",
                table: "Combine_Results",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "C_ID",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "Dominant_Hand",
                table: "Combine_Results");

            migrationBuilder.AlterColumn<string>(
                name: "Wingspan",
                table: "Combine_Results",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Weight",
                table: "Combine_Results",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Standing_Reach",
                table: "Combine_Results",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Height",
                table: "Combine_Results",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Handspan",
                table: "Combine_Results",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
