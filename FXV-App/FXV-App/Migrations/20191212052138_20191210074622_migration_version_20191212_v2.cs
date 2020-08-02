using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20191210074622_migration_version_20191212_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Individual_Tests_AspNetUsers_AppUserId",
                table: "Individual_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Individual_Tests_Tests_Test_ID",
                table: "Individual_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Individual_Tests_Individual_TestsIT_ID",
                table: "Splits_Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Individual_Tests",
                table: "Individual_Tests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Individual_Tests");

            migrationBuilder.RenameTable(
                name: "Individual_Tests",
                newName: "Test_Result");

            migrationBuilder.RenameColumn(
                name: "C_ID",
                table: "Combine_Results",
                newName: "E_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results",
                newName: "IX_Combine_Results_E_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Individual_Tests_Test_ID",
                table: "Test_Result",
                newName: "IX_Test_Result_Test_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Individual_Tests_AppUserId",
                table: "Test_Result",
                newName: "IX_Test_Result_AppUserId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "E_ID",
                table: "Test_Result",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Test_Result",
                table: "Test_Result",
                column: "IT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Result_E_ID",
                table: "Test_Result",
                column: "E_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Results_Events_E_ID",
                table: "Combine_Results",
                column: "E_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Test_Result_Individual_TestsIT_ID",
                table: "Splits_Tests",
                column: "Individual_TestsIT_ID",
                principalTable: "Test_Result",
                principalColumn: "IT_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_AspNetUsers_AppUserId",
                table: "Test_Result",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_Events_E_ID",
                table: "Test_Result",
                column: "E_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_Tests_Test_ID",
                table: "Test_Result",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Events_E_ID",
                table: "Combine_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Test_Result_Individual_TestsIT_ID",
                table: "Splits_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_AspNetUsers_AppUserId",
                table: "Test_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Events_E_ID",
                table: "Test_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Tests_Test_ID",
                table: "Test_Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Test_Result",
                table: "Test_Result");

            migrationBuilder.DropIndex(
                name: "IX_Test_Result_E_ID",
                table: "Test_Result");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "E_ID",
                table: "Test_Result");

            migrationBuilder.RenameTable(
                name: "Test_Result",
                newName: "Individual_Tests");

            migrationBuilder.RenameColumn(
                name: "E_ID",
                table: "Combine_Results",
                newName: "C_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Results_E_ID",
                table: "Combine_Results",
                newName: "IX_Combine_Results_C_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Test_Result_Test_ID",
                table: "Individual_Tests",
                newName: "IX_Individual_Tests_Test_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Test_Result_AppUserId",
                table: "Individual_Tests",
                newName: "IX_Individual_Tests_AppUserId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Individual_Tests",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Individual_Tests",
                table: "Individual_Tests",
                column: "IT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Individual_Tests_AspNetUsers_AppUserId",
                table: "Individual_Tests",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Individual_Tests_Tests_Test_ID",
                table: "Individual_Tests",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Tests_Individual_Tests_Individual_TestsIT_ID",
                table: "Splits_Tests",
                column: "Individual_TestsIT_ID",
                principalTable: "Individual_Tests",
                principalColumn: "IT_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
