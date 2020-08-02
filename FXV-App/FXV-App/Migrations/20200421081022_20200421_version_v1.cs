using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class _20200421_version_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AthleteAchievement_AspNetUsers_AppUserId",
                table: "AthleteAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Result_AspNetUsers_AppUserId",
                table: "Combine_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Assigned_Attendee_AspNetUsers_AppUserId",
                table: "Event_Assigned_Attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Result_AspNetUsers_AppUserId",
                table: "Event_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_AspNetUsers_AppUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Relationship_AspNetUsers_AppUserId",
                table: "Organization_Relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_AppUserId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Membership_AspNetUsers_AppUserId",
                table: "Team_Membership");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_AspNetUsers_AppUserId",
                table: "Test_Result");

            migrationBuilder.DropIndex(
                name: "IX_Test_Result_AppUserId",
                table: "Test_Result");

            migrationBuilder.DropIndex(
                name: "IX_Team_Membership_AppUserId",
                table: "Team_Membership");

            migrationBuilder.DropIndex(
                name: "IX_Team_AppUserId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Relationship_AppUserId",
                table: "Organization_Relationship");

            migrationBuilder.DropIndex(
                name: "IX_Organization_AppUserId",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Event_Result_AppUserId",
                table: "Event_Result");

            migrationBuilder.DropIndex(
                name: "IX_Event_Assigned_Attendee_AppUserId",
                table: "Event_Assigned_Attendee");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Result_AppUserId",
                table: "Combine_Result");

            migrationBuilder.DropIndex(
                name: "IX_AthleteAchievement_AppUserId",
                table: "AthleteAchievement");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Test_Result");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Team_Membership");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Organization_Relationship");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Event_Result");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Event_Assigned_Attendee");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Combine_Result");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AthleteAchievement");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Team_Relationship",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Result",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Org_Relationship",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Category",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Team_Membership",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Team",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SubscriptionPermission",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Sport",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Organization_Relationship",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Organization",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Result",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Builder",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Assigned_Attendee",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Result",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Categories_Weight",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Builder",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "AthleteAchievement",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "AspNetUsers",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Activity_Scheduled",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_Result_Id",
                table: "Test_Result",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Membership_Id",
                table: "Team_Membership",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Id",
                table: "Team",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationship_Id",
                table: "Organization_Relationship",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Id",
                table: "Organization",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Result_Id",
                table: "Event_Result",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendee_Id",
                table: "Event_Assigned_Attendee",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Result_Id",
                table: "Combine_Result",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteAchievement_Id",
                table: "AthleteAchievement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AthleteAchievement_AspNetUsers_Id",
                table: "AthleteAchievement",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Result_AspNetUsers_Id",
                table: "Combine_Result",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Assigned_Attendee_AspNetUsers_Id",
                table: "Event_Assigned_Attendee",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Result_AspNetUsers_Id",
                table: "Event_Result",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_AspNetUsers_Id",
                table: "Organization",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Relationship_AspNetUsers_Id",
                table: "Organization_Relationship",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_Id",
                table: "Team",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Membership_AspNetUsers_Id",
                table: "Team_Membership",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_AspNetUsers_Id",
                table: "Test_Result",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AthleteAchievement_AspNetUsers_Id",
                table: "AthleteAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Result_AspNetUsers_Id",
                table: "Combine_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Assigned_Attendee_AspNetUsers_Id",
                table: "Event_Assigned_Attendee");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Result_AspNetUsers_Id",
                table: "Event_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_AspNetUsers_Id",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Relationship_AspNetUsers_Id",
                table: "Organization_Relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_Id",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Membership_AspNetUsers_Id",
                table: "Team_Membership");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_AspNetUsers_Id",
                table: "Test_Result");

            migrationBuilder.DropIndex(
                name: "IX_Test_Result_Id",
                table: "Test_Result");

            migrationBuilder.DropIndex(
                name: "IX_Team_Membership_Id",
                table: "Team_Membership");

            migrationBuilder.DropIndex(
                name: "IX_Team_Id",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Relationship_Id",
                table: "Organization_Relationship");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Id",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Event_Result_Id",
                table: "Event_Result");

            migrationBuilder.DropIndex(
                name: "IX_Event_Assigned_Attendee_Id",
                table: "Event_Assigned_Attendee");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Result_Id",
                table: "Combine_Result");

            migrationBuilder.DropIndex(
                name: "IX_AthleteAchievement_Id",
                table: "AthleteAchievement");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Team_Relationship",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Result",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Test_Result",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Org_Relationship",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Category",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Test",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Team_Membership",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Team_Membership",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Team",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Team",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SubscriptionPermission",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Sport",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Organization_Relationship",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Organization_Relationship",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Organization",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Organization",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Result",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Event_Result",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Builder",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event_Assigned_Attendee",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Event_Assigned_Attendee",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Event",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Result",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Combine_Result",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Categories_Weight",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Builder",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Combine",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "AthleteAchievement",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "AthleteAchievement",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Activity_Scheduled",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_Result_AppUserId",
                table: "Test_Result",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Membership_AppUserId",
                table: "Team_Membership",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_AppUserId",
                table: "Team",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationship_AppUserId",
                table: "Organization_Relationship",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_AppUserId",
                table: "Organization",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Result_AppUserId",
                table: "Event_Result",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendee_AppUserId",
                table: "Event_Assigned_Attendee",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Result_AppUserId",
                table: "Combine_Result",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteAchievement_AppUserId",
                table: "AthleteAchievement",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AthleteAchievement_AspNetUsers_AppUserId",
                table: "AthleteAchievement",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Result_AspNetUsers_AppUserId",
                table: "Combine_Result",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Assigned_Attendee_AspNetUsers_AppUserId",
                table: "Event_Assigned_Attendee",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Result_AspNetUsers_AppUserId",
                table: "Event_Result",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_AspNetUsers_AppUserId",
                table: "Organization",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Relationship_AspNetUsers_AppUserId",
                table: "Organization_Relationship",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_AppUserId",
                table: "Team",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Membership_AspNetUsers_AppUserId",
                table: "Team_Membership",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_AspNetUsers_AppUserId",
                table: "Test_Result",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
