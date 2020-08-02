using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20191210074622_migration_version_20191212_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Individual_TestsIT_ID",
                table: "Splits_Tests");

            migrationBuilder.RenameColumn(
                name: "Individual_TestsIT_ID",
                table: "Splits_Tests",
                newName: "Tests_ResultIT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_Individual_TestsIT_ID",
                table: "Splits_Tests",
                newName: "IX_Splits_Tests_Tests_ResultIT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultIT_ID",
                table: "Splits_Tests",
                column: "Tests_ResultIT_ID",
                principalTable: "Test_Result",
                principalColumn: "IT_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultIT_ID",
                table: "Splits_Tests");

            migrationBuilder.RenameColumn(
                name: "Tests_ResultIT_ID",
                table: "Splits_Tests",
                newName: "Individual_TestsIT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_Tests_ResultIT_ID",
                table: "Splits_Tests",
                newName: "IX_Splits_Tests_Individual_TestsIT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Test_Result_Individual_TestsIT_ID",
                table: "Splits_Tests",
                column: "Individual_TestsIT_ID",
                principalTable: "Test_Result",
                principalColumn: "IT_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
