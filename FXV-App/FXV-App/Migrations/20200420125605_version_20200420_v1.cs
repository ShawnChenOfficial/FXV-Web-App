using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class version_20200420_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_Combine_Categories_Weights_Combines_C_ID",
                table: "Combine_Categories_Weights");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Categories_Weights_Test_Categories_TC_ID",
                table: "Combine_Categories_Weights");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Events_E_ID",
                table: "Test_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Tests_Test_ID",
                table: "Test_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Tests_Combines_C_ID",
                table: "Splits_Tests");

            migrationBuilder.DropTable(
                name: "Splits_Tests");

            migrationBuilder.DropTable(
                name: "AthleteAchievements");

            migrationBuilder.DropTable(
                name: "Combine_Results");

            migrationBuilder.DropTable(
                name: "Event_Assigned_Attendees");

            migrationBuilder.DropTable(
                name: "Event_Builders");

            migrationBuilder.DropTable(
                name: "Event_Results");

            migrationBuilder.DropTable(
                name: "Organization_Relationships");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Team_Memberships");

            migrationBuilder.DropTable(
                name: "Test_Org_Relationships");

            migrationBuilder.DropTable(
                name: "Test_Team_Relationships");

            migrationBuilder.DropTable(
                name: "Combines");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Test_Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempEventTables",
                table: "TempEventTables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Combine_Categories_Weights",
                table: "Combine_Categories_Weights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Combine_Builders",
                table: "Combine_Builders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity_Scheduleds",
                table: "Activity_Scheduleds");

            migrationBuilder.RenameTable(
                name: "TempEventTables",
                newName: "TempEventTable");

            migrationBuilder.RenameTable(
                name: "Combine_Categories_Weights",
                newName: "Combine_Categories_Weight");

            migrationBuilder.RenameTable(
                name: "Combine_Builders",
                newName: "Combine_Builder");

            migrationBuilder.RenameTable(
                name: "Activity_Scheduleds",
                newName: "Activity_Scheduled");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Categories_Weights_TC_ID",
                table: "Combine_Categories_Weight",
                newName: "IX_Combine_Categories_Weight_TC_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Categories_Weights_C_ID",
                table: "Combine_Categories_Weight",
                newName: "IX_Combine_Categories_Weight_C_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Builders_Test_ID",
                table: "Combine_Builder",
                newName: "IX_Combine_Builder_Test_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Builders_C_ID",
                table: "Combine_Builder",
                newName: "IX_Combine_Builder_C_ID");

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "Test_Result",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "AspNetUsers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Categories_Weight",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "Combine_Builder",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "Activity_Scheduled",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempEventTable",
                table: "TempEventTable",
                column: "Tem_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Combine_Categories_Weight",
                table: "Combine_Categories_Weight",
                column: "CC_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Combine_Builder",
                table: "Combine_Builder",
                column: "CB_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity_Scheduled",
                table: "Activity_Scheduled",
                column: "AS_ID");

            migrationBuilder.CreateTable(
                name: "AthleteAchievement",
                columns: table => new
                {
                    Achievement_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<int>(nullable: false),
                    Achievement = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteAchievement", x => x.Achievement_ID);
                    table.ForeignKey(
                        name: "FK_AthleteAchievement_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Combine",
                columns: table => new
                {
                    C_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combine", x => x.C_ID);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    E_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    Img_Path = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.E_ID);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Org_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    Num_Of_Teams = table.Column<int>(nullable: false),
                    Img_Path = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Org_ID);
                    table.ForeignKey(
                        name: "FK_Organization_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    Sport_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sport_Name = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport", x => x.Sport_ID);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPermission",
                columns: table => new
                {
                    P_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Permission = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPermission", x => x.P_ID);
                });

            migrationBuilder.CreateTable(
                name: "Test_Category",
                columns: table => new
                {
                    TC_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Category", x => x.TC_id);
                });

            migrationBuilder.CreateTable(
                name: "Combine_Result",
                columns: table => new
                {
                    CR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Point = table.Column<int>(nullable: false),
                    E_ID = table.Column<int>(nullable: false),
                    C_ID = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Standing_Reach = table.Column<double>(nullable: false),
                    Wingspan = table.Column<double>(nullable: false),
                    Handspan = table.Column<double>(nullable: false),
                    Dominant_Hand = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combine_Result", x => x.CR_ID);
                    table.ForeignKey(
                        name: "FK_Combine_Result_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Combine_Result_Event_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Event",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Assigned_Attendee",
                columns: table => new
                {
                    EA_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    E_ID = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Assigned_Attendee", x => x.EA_ID);
                    table.ForeignKey(
                        name: "FK_Event_Assigned_Attendee_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Assigned_Attendee_Event_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Event",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Builder",
                columns: table => new
                {
                    EB_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    C_ID = table.Column<int>(nullable: false),
                    E_ID = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Builder", x => x.EB_ID);
                    table.ForeignKey(
                        name: "FK_Event_Builder_Combine_C_ID",
                        column: x => x.C_ID,
                        principalTable: "Combine",
                        principalColumn: "C_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Builder_Event_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Event",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Result",
                columns: table => new
                {
                    ER_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    E_ID = table.Column<int>(nullable: false),
                    Final_Point = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Result", x => x.ER_ID);
                    table.ForeignKey(
                        name: "FK_Event_Result_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Result_Event_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Event",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organization_Relationship",
                columns: table => new
                {
                    OR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<int>(nullable: false),
                    Org_ID = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization_Relationship", x => x.OR_ID);
                    table.ForeignKey(
                        name: "FK_Organization_Relationship_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Relationship_Organization_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organization",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Team_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Org_ID = table.Column<int>(nullable: false),
                    Sport_ID = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    Num_Of_Members = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Team_ID);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_Organization_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organization",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Sport_Sport_ID",
                        column: x => x.Sport_ID,
                        principalTable: "Sport",
                        principalColumn: "Sport_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Test_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Measurement_Type = table.Column<string>(nullable: true),
                    Visible = table.Column<bool>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    Reverse = table.Column<bool>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    LowerResult = table.Column<double>(nullable: false),
                    HigherResult = table.Column<double>(nullable: false),
                    LowerScore = table.Column<int>(nullable: false),
                    HigherScore = table.Column<int>(nullable: false),
                    TC_id = table.Column<int>(nullable: false),
                    LowerCalc = table.Column<double>(nullable: false),
                    HigherCalc = table.Column<double>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Test_ID);
                    table.ForeignKey(
                        name: "FK_Test_Test_Category_TC_id",
                        column: x => x.TC_id,
                        principalTable: "Test_Category",
                        principalColumn: "TC_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team_Membership",
                columns: table => new
                {
                    TM_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team_ID = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Membership", x => x.TM_ID);
                    table.ForeignKey(
                        name: "FK_Team_Membership_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_Membership_Team_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Team",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test_Org_Relationship",
                columns: table => new
                {
                    TOR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    Org_ID = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Org_Relationship", x => x.TOR);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationship_Organization_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organization",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationship_Test_Test_ID",
                        column: x => x.Test_ID,
                        principalTable: "Test",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test_Team_Relationship",
                columns: table => new
                {
                    TTR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    Team_ID = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Team_Relationship", x => x.TTR);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationship_Team_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Team",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationship_Test_Test_ID",
                        column: x => x.Test_ID,
                        principalTable: "Test",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthleteAchievement_AppUserId",
                table: "AthleteAchievement",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Result_AppUserId",
                table: "Combine_Result",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Result_E_ID",
                table: "Combine_Result",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendee_AppUserId",
                table: "Event_Assigned_Attendee",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendee_E_ID",
                table: "Event_Assigned_Attendee",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builder_C_ID",
                table: "Event_Builder",
                column: "C_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Builder_E_ID",
                table: "Event_Builder",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Result_AppUserId",
                table: "Event_Result",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Result_E_ID",
                table: "Event_Result",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_AppUserId",
                table: "Organization",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationship_AppUserId",
                table: "Organization_Relationship",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationship_Org_ID",
                table: "Organization_Relationship",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_AppUserId",
                table: "Team",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Org_ID",
                table: "Team",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Sport_ID",
                table: "Team",
                column: "Sport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Membership_AppUserId",
                table: "Team_Membership",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Membership_Team_ID",
                table: "Team_Membership",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TC_id",
                table: "Test",
                column: "TC_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationship_Org_ID",
                table: "Test_Org_Relationship",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationship_Test_ID",
                table: "Test_Org_Relationship",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationship_Team_ID",
                table: "Test_Team_Relationship",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationship_Test_ID",
                table: "Test_Team_Relationship",
                column: "Test_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SubscriptionPermission_P_ID",
                table: "AspNetUsers",
                column: "P_ID",
                principalTable: "SubscriptionPermission",
                principalColumn: "P_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builder_Combine_C_ID",
                table: "Combine_Builder",
                column: "C_ID",
                principalTable: "Combine",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Builder_Test_Test_ID",
                table: "Combine_Builder",
                column: "Test_ID",
                principalTable: "Test",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Categories_Weight_Combine_C_ID",
                table: "Combine_Categories_Weight",
                column: "C_ID",
                principalTable: "Combine",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Categories_Weight_Test_Category_TC_ID",
                table: "Combine_Categories_Weight",
                column: "TC_ID",
                principalTable: "Test_Category",
                principalColumn: "TC_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_Event_E_ID",
                table: "Test_Result",
                column: "E_ID",
                principalTable: "Event",
                principalColumn: "E_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Result_Test_Test_ID",
                table: "Test_Result",
                column: "Test_ID",
                principalTable: "Test",
                principalColumn: "Test_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SubscriptionPermission_P_ID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builder_Combine_C_ID",
                table: "Combine_Builder");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Builder_Test_Test_ID",
                table: "Combine_Builder");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Categories_Weight_Combine_C_ID",
                table: "Combine_Categories_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_Combine_Categories_Weight_Test_Category_TC_ID",
                table: "Combine_Categories_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Event_E_ID",
                table: "Test_Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Result_Test_Test_ID",
                table: "Test_Result");

            migrationBuilder.DropTable(
                name: "AthleteAchievement");

            migrationBuilder.DropTable(
                name: "Combine_Result");

            migrationBuilder.DropTable(
                name: "Event_Assigned_Attendee");

            migrationBuilder.DropTable(
                name: "Event_Builder");

            migrationBuilder.DropTable(
                name: "Event_Result");

            migrationBuilder.DropTable(
                name: "Organization_Relationship");

            migrationBuilder.DropTable(
                name: "SubscriptionPermission");

            migrationBuilder.DropTable(
                name: "Team_Membership");

            migrationBuilder.DropTable(
                name: "Test_Org_Relationship");

            migrationBuilder.DropTable(
                name: "Test_Team_Relationship");

            migrationBuilder.DropTable(
                name: "Combine");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Test_Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempEventTable",
                table: "TempEventTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Combine_Categories_Weight",
                table: "Combine_Categories_Weight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Combine_Builder",
                table: "Combine_Builder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity_Scheduled",
                table: "Activity_Scheduled");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Test_Result");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Combine_Categories_Weight");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Combine_Builder");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Activity_Scheduled");

            migrationBuilder.RenameTable(
                name: "TempEventTable",
                newName: "TempEventTables");

            migrationBuilder.RenameTable(
                name: "Combine_Categories_Weight",
                newName: "Combine_Categories_Weights");

            migrationBuilder.RenameTable(
                name: "Combine_Builder",
                newName: "Combine_Builders");

            migrationBuilder.RenameTable(
                name: "Activity_Scheduled",
                newName: "Activity_Scheduleds");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Categories_Weight_TC_ID",
                table: "Combine_Categories_Weights",
                newName: "IX_Combine_Categories_Weights_TC_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Categories_Weight_C_ID",
                table: "Combine_Categories_Weights",
                newName: "IX_Combine_Categories_Weights_C_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Builder_Test_ID",
                table: "Combine_Builders",
                newName: "IX_Combine_Builders_Test_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Combine_Builder_C_ID",
                table: "Combine_Builders",
                newName: "IX_Combine_Builders_C_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempEventTables",
                table: "TempEventTables",
                column: "Tem_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Combine_Categories_Weights",
                table: "Combine_Categories_Weights",
                column: "CC_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Combine_Builders",
                table: "Combine_Builders",
                column: "CB_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity_Scheduleds",
                table: "Activity_Scheduleds",
                column: "AS_ID");

            migrationBuilder.CreateTable(
                name: "AthleteAchievements",
                columns: table => new
                {
                    Achievement_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Achievement = table.Column<string>(nullable: true),
                    AppUserId = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteAchievements", x => x.Achievement_ID);
                    table.ForeignKey(
                        name: "FK_AthleteAchievements_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Combines",
                columns: table => new
                {
                    C_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combines", x => x.C_ID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    E_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.E_ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Org_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Num_Of_Teams = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Org_ID);
                    table.ForeignKey(
                        name: "FK_Organizations_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    P_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Permission = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.P_ID);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Sport_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sport_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Sport_ID);
                });

            migrationBuilder.CreateTable(
                name: "Test_Categories",
                columns: table => new
                {
                    TC_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Categories", x => x.TC_id);
                });

            migrationBuilder.CreateTable(
                name: "Combine_Results",
                columns: table => new
                {
                    CR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    C_ID = table.Column<int>(nullable: false),
                    Dominant_Hand = table.Column<string>(nullable: true),
                    E_ID = table.Column<int>(nullable: false),
                    Handspan = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    Standing_Reach = table.Column<double>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Wingspan = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combine_Results", x => x.CR_ID);
                    table.ForeignKey(
                        name: "FK_Combine_Results_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Combine_Results_Events_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Assigned_Attendees",
                columns: table => new
                {
                    EA_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    E_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Assigned_Attendees", x => x.EA_ID);
                    table.ForeignKey(
                        name: "FK_Event_Assigned_Attendees_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Assigned_Attendees_Events_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Builders",
                columns: table => new
                {
                    EB_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    C_ID = table.Column<int>(nullable: false),
                    E_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Builders", x => x.EB_ID);
                    table.ForeignKey(
                        name: "FK_Event_Builders_Combines_C_ID",
                        column: x => x.C_ID,
                        principalTable: "Combines",
                        principalColumn: "C_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Builders_Events_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Results",
                columns: table => new
                {
                    ER_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    E_ID = table.Column<int>(nullable: false),
                    Final_Point = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Results", x => x.ER_ID);
                    table.ForeignKey(
                        name: "FK_Event_Results_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Results_Events_E_ID",
                        column: x => x.E_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organization_Relationships",
                columns: table => new
                {
                    OR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    Org_ID = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization_Relationships", x => x.OR_ID);
                    table.ForeignKey(
                        name: "FK_Organization_Relationships_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Relationships_Organizations_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Team_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Num_Of_Members = table.Column<int>(nullable: false),
                    Org_ID = table.Column<int>(nullable: false),
                    Sport_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Team_ID);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Organizations_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Test_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    HigherCalc = table.Column<double>(nullable: false),
                    HigherResult = table.Column<double>(nullable: false),
                    HigherScore = table.Column<int>(nullable: false),
                    Img_Path = table.Column<string>(nullable: true),
                    LowerCalc = table.Column<double>(nullable: false),
                    LowerResult = table.Column<double>(nullable: false),
                    LowerScore = table.Column<int>(nullable: false),
                    Measurement_Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Reverse = table.Column<bool>(nullable: false),
                    TC_id = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    Visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Test_ID);
                    table.ForeignKey(
                        name: "FK_Tests_Test_Categories_TC_id",
                        column: x => x.TC_id,
                        principalTable: "Test_Categories",
                        principalColumn: "TC_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team_Memberships",
                columns: table => new
                {
                    TM_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Team_ID = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Memberships", x => x.TM_ID);
                    table.ForeignKey(
                        name: "FK_Team_Memberships_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_Memberships_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test_Org_Relationships",
                columns: table => new
                {
                    TOR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Org_ID = table.Column<int>(nullable: false),
                    Test_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Org_Relationships", x => x.TOR);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationships_Organizations_Org_ID",
                        column: x => x.Org_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationships_Tests_Test_ID",
                        column: x => x.Test_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test_Team_Relationships",
                columns: table => new
                {
                    TTR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team_ID = table.Column<int>(nullable: false),
                    Test_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Team_Relationships", x => x.TTR);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationships_Teams_Team_ID",
                        column: x => x.Team_ID,
                        principalTable: "Teams",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationships_Tests_Test_ID",
                        column: x => x.Test_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthleteAchievements_AppUserId",
                table: "AthleteAchievements",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_AppUserId",
                table: "Combine_Results",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_E_ID",
                table: "Combine_Results",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_AppUserId",
                table: "Event_Assigned_Attendees",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_E_ID",
                table: "Event_Assigned_Attendees",
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
                name: "IX_Event_Results_AppUserId",
                table: "Event_Results",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Results_E_ID",
                table: "Event_Results",
                column: "E_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_AppUserId",
                table: "Organization_Relationships",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_Org_ID",
                table: "Organization_Relationships",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AppUserId",
                table: "Organizations",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_AppUserId",
                table: "Team_Memberships",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_Team_ID",
                table: "Team_Memberships",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppUserId",
                table: "Teams",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Org_ID",
                table: "Teams",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_Org_ID",
                table: "Test_Org_Relationships",
                column: "Org_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_Test_ID",
                table: "Test_Org_Relationships",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_Team_ID",
                table: "Test_Team_Relationships",
                column: "Team_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_Test_ID",
                table: "Test_Team_Relationships",
                column: "Test_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TC_id",
                table: "Tests",
                column: "TC_id");

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
                name: "FK_Combine_Categories_Weights_Combines_C_ID",
                table: "Combine_Categories_Weights",
                column: "C_ID",
                principalTable: "Combines",
                principalColumn: "C_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combine_Categories_Weights_Test_Categories_TC_ID",
                table: "Combine_Categories_Weights",
                column: "TC_ID",
                principalTable: "Test_Categories",
                principalColumn: "TC_id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
