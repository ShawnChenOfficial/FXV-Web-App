using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class migration_version_20191210_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Permissions_PermissionsP_ID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builders_Combines_CombinesC_ID",
                table: "Combine_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builders_Tests_TestsTest_ID",
                table: "Combine_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Combines_CombinesC_ID",
                table: "Combine_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Assigned_Attendees_Events_EventsE_ID",
                table: "Event_Assigned_Attendees");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Builders_Combines_CombinesC_ID",
                table: "Event_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Builders_Events_EventsE_ID",
                table: "Event_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Results_Events_EventsE_ID",
                table: "Event_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Individual_Tests_Tests_TestsTest_ID",
                table: "Individual_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Relationships_Organizations_OrganizationsOrg_ID",
                table: "Organization_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Memberships_Teams_TeamsTeam_ID",
                table: "Team_Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organizations_OrganizationsOrg_ID",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Org_Relationships_Organizations_OrganizationsOrg_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Org_Relationships_Tests_TestsTest_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Team_Relationships_Teams_TeamsTeam_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Team_Relationships_Tests_TestsTest_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Team_Relationships_TeamsTeam_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Team_Relationships_TestsTest_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Org_Relationships_OrganizationsOrg_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Org_Relationships_TestsTest_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Teams_OrganizationsOrg_ID",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Team_Memberships_TeamsTeam_ID",
                table: "Team_Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Relationships_OrganizationsOrg_ID",
                table: "Organization_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Individual_Tests_TestsTest_ID",
                table: "Individual_Tests");

            migrationBuilder.DropIndex(
                name: "IX_Event_Results_EventsE_ID",
                table: "Event_Results");

            migrationBuilder.DropIndex(
                name: "IX_Event_Builders_CombinesC_ID",
                table: "Event_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Event_Builders_EventsE_ID",
                table: "Event_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Event_Assigned_Attendees_EventsE_ID",
                table: "Event_Assigned_Attendees");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Results_CombinesC_ID",
                table: "Combine_Results");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Builders_CombinesC_ID",
                table: "Combine_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Builders_TestsTest_ID",
                table: "Combine_Builders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PermissionsP_ID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeamsTeam_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropColumn(
                name: "TestsTest_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropColumn(
                name: "OrganizationsOrg_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropColumn(
                name: "TestsTest_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropColumn(
                name: "OrganizationsOrg_ID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamsTeam_ID",
                table: "Team_Memberships");

            migrationBuilder.DropColumn(
                name: "OrganizationsOrg_ID",
                table: "Organization_Relationships");

            migrationBuilder.DropColumn(
                name: "TestsTest_ID",
                table: "Individual_Tests");

            migrationBuilder.DropColumn(
                name: "EventsE_ID",
                table: "Event_Results");

            migrationBuilder.DropColumn(
                name: "CombinesC_ID",
                table: "Event_Builders");

            migrationBuilder.DropColumn(
                name: "EventsE_ID",
                table: "Event_Builders");

            migrationBuilder.DropColumn(
                name: "EventsE_ID",
                table: "Event_Assigned_Attendees");

            migrationBuilder.DropColumn(
                name: "CombinesC_ID",
                table: "Combine_Results");

            migrationBuilder.DropColumn(
                name: "CombinesC_ID",
                table: "Combine_Builders");

            migrationBuilder.DropColumn(
                name: "TestsTest_ID",
                table: "Combine_Builders");

            migrationBuilder.DropColumn(
                name: "PermissionsP_ID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<bool>(
                name: "Visible",
                table: "Tests",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Splites",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Measurement_Type",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Age_Group",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Team_Memberships",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Team_Memberships",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sport_Name",
                table: "Sports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Permission",
                table: "Permissions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Organization_Relationships",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Individual_Tests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Combines",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Combines",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Combines",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserTokens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt_2",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt_1",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Profile_Img_Path",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discription",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_Team_ID",
                table: "Test_Team_Relationships",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_Test_ID",
                table: "Test_Team_Relationships",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_Org_ID",
                table: "Test_Org_Relationships",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_Test_ID",
                table: "Test_Org_Relationships",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Org_ID",
                table: "Teams",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_Team_ID",
                table: "Team_Memberships",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_Org_ID",
                table: "Organization_Relationships",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Individual_Tests_Test_ID",
                table: "Individual_Tests",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Results_E_ID",
                table: "Event_Results",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builders_C_ID",
                table: "Event_Builders",
                column: "C_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builders_E_ID",
                table: "Event_Builders",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_E_ID",
                table: "Event_Assigned_Attendees",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results",
                column: "C_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_C_ID",
                table: "Combine_Builders",
                column: "C_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_Test_ID",
                table: "Combine_Builders",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_P_ID",
                table: "AspNetUsers",
                column: "P_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Permissions_P_ID",
                table: "AspNetUsers",
                column: "P_ID",
                principalTable: "Permissions",
                principalColumn: "P_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builders_Combines_C_ID",
                table: "Combine_Builders",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builders_Tests_Test_ID",
                table: "Combine_Builders",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Assigned_Attendees_Events_E_ID",
                table: "Event_Assigned_Attendees",
                column: "E_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Builders_Combines_C_ID",
                table: "Event_Builders",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Builders_Events_E_ID",
                table: "Event_Builders",
                column: "E_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Results_Events_E_ID",
                table: "Event_Results",
                column: "E_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Individual_Tests_Tests_Test_ID",
                table: "Individual_Tests",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Relationships_Organizations_Org_ID",
                table: "Organization_Relationships",
                column: "Org_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Memberships_Teams_Team_ID",
                table: "Team_Memberships",
                column: "Team_ID",
                principalTable: "Teams",
                principalColumn: "Team_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organizations_Org_ID",
                table: "Teams",
                column: "Org_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Org_Relationships_Organizations_Org_ID",
                table: "Test_Org_Relationships",
                column: "Org_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Org_Relationships_Tests_Test_ID",
                table: "Test_Org_Relationships",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Team_Relationships_Teams_Team_ID",
                table: "Test_Team_Relationships",
                column: "Team_ID",
                principalTable: "Teams",
                principalColumn: "Team_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Team_Relationships_Tests_Test_ID",
                table: "Test_Team_Relationships",
                column: "Test_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Permissions_P_ID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builders_Combines_C_ID",
                table: "Combine_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builders_Tests_Test_ID",
                table: "Combine_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Results_Combines_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Assigned_Attendees_Events_E_ID",
                table: "Event_Assigned_Attendees");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Builders_Combines_C_ID",
                table: "Event_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Builders_Events_E_ID",
                table: "Event_Builders");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Results_Events_E_ID",
                table: "Event_Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Individual_Tests_Tests_Test_ID",
                table: "Individual_Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Relationships_Organizations_Org_ID",
                table: "Organization_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Memberships_Teams_Team_ID",
                table: "Team_Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organizations_Org_ID",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Org_Relationships_Organizations_Org_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Org_Relationships_Tests_Test_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Team_Relationships_Teams_Team_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Team_Relationships_Tests_Test_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Team_Relationships_Team_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Team_Relationships_Test_ID",
                table: "Test_Team_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Org_Relationships_Org_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Test_Org_Relationships_Test_ID",
                table: "Test_Org_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Teams_Org_ID",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Team_Memberships_Team_ID",
                table: "Team_Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Relationships_Org_ID",
                table: "Organization_Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Individual_Tests_Test_ID",
                table: "Individual_Tests");

            migrationBuilder.DropIndex(
                name: "IX_Event_Results_E_ID",
                table: "Event_Results");

            migrationBuilder.DropIndex(
                name: "IX_Event_Builders_C_ID",
                table: "Event_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Event_Builders_E_ID",
                table: "Event_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Event_Assigned_Attendees_E_ID",
                table: "Event_Assigned_Attendees");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Results_C_ID",
                table: "Combine_Results");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Builders_C_ID",
                table: "Combine_Builders");

            migrationBuilder.DropIndex(
                name: "IX_Combine_Builders_Test_ID",
                table: "Combine_Builders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_P_ID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Visible",
                table: "Tests",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "Splites",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Measurement_Type",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Age_Group",
                table: "Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamsTeam_ID",
                table: "Test_Team_Relationships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestsTest_ID",
                table: "Test_Team_Relationships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationsOrg_ID",
                table: "Test_Org_Relationships",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestsTest_ID",
                table: "Test_Org_Relationships",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Teams",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Teams",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Teams",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationsOrg_ID",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Team_Memberships",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Team_Memberships",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamsTeam_ID",
                table: "Team_Memberships",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sport_Name",
                table: "Sports",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Permission",
                table: "Permissions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Organizations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Organizations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Organizations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Organization_Relationships",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationsOrg_ID",
                table: "Organization_Relationships",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Individual_Tests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestsTest_ID",
                table: "Individual_Tests",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Events",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventsE_ID",
                table: "Event_Results",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CombinesC_ID",
                table: "Event_Builders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventsE_ID",
                table: "Event_Builders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventsE_ID",
                table: "Event_Assigned_Attendees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Combines",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img_Path",
                table: "Combines",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Combines",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CombinesC_ID",
                table: "Combine_Results",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CombinesC_ID",
                table: "Combine_Builders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestsTest_ID",
                table: "Combine_Builders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserTokens",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt_2",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt_1",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Profile_Img_Path",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discription",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionsP_ID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_TeamsTeam_ID",
                table: "Test_Team_Relationships",
                column: "TeamsTeam_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_TestsTest_ID",
                table: "Test_Team_Relationships",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_OrganizationsOrg_ID",
                table: "Test_Org_Relationships",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_TestsTest_ID",
                table: "Test_Org_Relationships",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationsOrg_ID",
                table: "Teams",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_TeamsTeam_ID",
                table: "Team_Memberships",
                column: "TeamsTeam_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_OrganizationsOrg_ID",
                table: "Organization_Relationships",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Individual_Tests_TestsTest_ID",
                table: "Individual_Tests",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Results_EventsE_ID",
                table: "Event_Results",
                column: "EventsE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builders_CombinesC_ID",
                table: "Event_Builders",
                column: "CombinesC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builders_EventsE_ID",
                table: "Event_Builders",
                column: "EventsE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_EventsE_ID",
                table: "Event_Assigned_Attendees",
                column: "EventsE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_CombinesC_ID",
                table: "Combine_Results",
                column: "CombinesC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_CombinesC_ID",
                table: "Combine_Builders",
                column: "CombinesC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_TestsTest_ID",
                table: "Combine_Builders",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PermissionsP_ID",
                table: "AspNetUsers",
                column: "PermissionsP_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Permissions_PermissionsP_ID",
                table: "AspNetUsers",
                column: "PermissionsP_ID",
                principalTable: "Permissions",
                principalColumn: "P_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builders_Combines_CombinesC_ID",
                table: "Combine_Builders",
                column: "CombinesC_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builders_Tests_TestsTest_ID",
                table: "Combine_Builders",
                column: "TestsTest_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Results_Combines_CombinesC_ID",
                table: "Combine_Results",
                column: "CombinesC_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Assigned_Attendees_Events_EventsE_ID",
                table: "Event_Assigned_Attendees",
                column: "EventsE_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Builders_Combines_CombinesC_ID",
                table: "Event_Builders",
                column: "CombinesC_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Builders_Events_EventsE_ID",
                table: "Event_Builders",
                column: "EventsE_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Results_Events_EventsE_ID",
                table: "Event_Results",
                column: "EventsE_ID",
                principalTable: "Events",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Individual_Tests_Tests_TestsTest_ID",
                table: "Individual_Tests",
                column: "TestsTest_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Relationships_Organizations_OrganizationsOrg_ID",
                table: "Organization_Relationships",
                column: "OrganizationsOrg_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Memberships_Teams_TeamsTeam_ID",
                table: "Team_Memberships",
                column: "TeamsTeam_ID",
                principalTable: "Teams",
                principalColumn: "Team_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organizations_OrganizationsOrg_ID",
                table: "Teams",
                column: "OrganizationsOrg_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Org_Relationships_Organizations_OrganizationsOrg_ID",
                table: "Test_Org_Relationships",
                column: "OrganizationsOrg_ID",
                principalTable: "Organizations",
                principalColumn: "Org_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Org_Relationships_Tests_TestsTest_ID",
                table: "Test_Org_Relationships",
                column: "TestsTest_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Team_Relationships_Teams_TeamsTeam_ID",
                table: "Test_Team_Relationships",
                column: "TeamsTeam_ID",
                principalTable: "Teams",
                principalColumn: "Team_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Team_Relationships_Tests_TestsTest_ID",
                table: "Test_Team_Relationships",
                column: "TestsTest_ID",
                principalTable: "Tests",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
