using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20191210074622_migration_version_20191212_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultIT_ID",
                table: "Splits_Tests");

            migrationBuilder.RenameColumn(
                name: "IT_ID",
                table: "Test_Result",
                newName: "TR_ID");

            migrationBuilder.RenameColumn(
                name: "IT_ID",
                table: "Splits_Tests",
                newName: "ChildTest");

            migrationBuilder.RenameColumn(
                name: "Tests_ResultIT_ID",
                table: "Splits_Tests",
                newName: "Tests_ResultTR_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_Tests_ResultIT_ID",
                table: "Splits_Tests",
                newName: "IX_Splits_Tests_Tests_ResultTR_ID");

            migrationBuilder.AddColumn<int>(
                name: "ParrentTest",
                table: "Splits_Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultTR_ID",
                table: "Splits_Tests",
                column: "Tests_ResultTR_ID",
                principalTable: "Test_Result",
                principalColumn: "TR_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Tests_ResultTR_ID",
                table: "Splits_Tests");

            migrationBuilder.DropColumn(
                name: "ParrentTest",
                table: "Splits_Tests");

            migrationBuilder.RenameColumn(
                name: "TR_ID",
                table: "Test_Result",
                newName: "IT_ID");

            migrationBuilder.RenameColumn(
                name: "ChildTest",
                table: "Splits_Tests",
                newName: "IT_ID");

            migrationBuilder.RenameColumn(
                name: "Tests_ResultTR_ID",
                table: "Splits_Tests",
                newName: "Tests_ResultIT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Splits_Tests_Tests_ResultTR_ID",
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
    }
}
