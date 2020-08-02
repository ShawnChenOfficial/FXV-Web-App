using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class migration_version_20191213_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultTR_ID",
                table: "Splits_Tests");

            migrationBuilder.DropColumn(
                name: "HasSplit",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "Tests_ResultTR_ID",
                table: "Splits_Tests",
                newName: "TestsTest_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_Tests_ResultTR_ID",
                table: "Splits_Tests",
                newName: "IX_Splits_Tests_TestsTest_ID");

            migrationBuilder.AddColumn<int>(
                name: "C_ID",
                table: "Splits_Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasSplit",
                table: "Combine_Builders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Splits_Tests_C_ID",
                table: "Splits_Tests",
                column: "C_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Combines_C_ID",
                table: "Splits_Tests",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Tests_TestsTest_ID",
                table: "Splits_Tests",
                column: "TestsTest_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Combines_C_ID",
                table: "Splits_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Tests_TestsTest_ID",
                table: "Splits_Tests");

            migrationBuilder.DropIndex(
                name: "IX_Splits_Tests_C_ID",
                table: "Splits_Tests");

            migrationBuilder.DropColumn(
                name: "C_ID",
                table: "Splits_Tests");

            migrationBuilder.DropColumn(
                name: "HasSplit",
                table: "Combine_Builders");

            migrationBuilder.RenameColumn(
                name: "TestsTest_ID",
                table: "Splits_Tests",
                newName: "Tests_ResultTR_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_TestsTest_ID",
                table: "Splits_Tests",
                newName: "IX_Splits_Tests_Tests_ResultTR_ID");

            migrationBuilder.AddColumn<bool>(
                name: "HasSplit",
                table: "Tests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultTR_ID",
                table: "Splits_Tests",
                column: "Tests_ResultTR_ID",
                principalTable: "Test_Result",
                principalColumn: "TR_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
