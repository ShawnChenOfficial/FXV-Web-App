using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FXV_App.Migrations
{
    public partial class migration_version_20191209_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity_Scheduleds",
                columns: table => new
                {
                    AS_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_Scheduleds", x => x.AS_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Combines",
                columns: table => new
                {
                    C_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true)
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    Img_Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.E_ID);
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
                name: "Tests",
                columns: table => new
                {
                    Test_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Age_Group = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Measurement_Type = table.Column<string>(nullable: true),
                    Splites = table.Column<string>(nullable: true),
                    Visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Test_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event_Builders",
                columns: table => new
                {
                    EB_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    C_ID = table.Column<int>(nullable: false),
                    CombinesC_ID = table.Column<int>(nullable: true),
                    E_ID = table.Column<int>(nullable: false),
                    EventsE_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Builders", x => x.EB_ID);
                    table.ForeignKey(
                        name: "FK_Event_Builders_Combines_CombinesC_ID",
                        column: x => x.CombinesC_ID,
                        principalTable: "Combines",
                        principalColumn: "C_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Builders_Events_EventsE_ID",
                        column: x => x.EventsE_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    Salt_1 = table.Column<string>(nullable: true),
                    Salt_2 = table.Column<string>(nullable: true),
                    Discription = table.Column<string>(nullable: true),
                    P_ID = table.Column<int>(nullable: false),
                    PermissionsP_ID = table.Column<int>(nullable: true),
                    Profile_Img_Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Permissions_PermissionsP_ID",
                        column: x => x.PermissionsP_ID,
                        principalTable: "Permissions",
                        principalColumn: "P_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Combine_Builders",
                columns: table => new
                {
                    CB_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    TestsTest_ID = table.Column<int>(nullable: true),
                    C_ID = table.Column<int>(nullable: false),
                    CombinesC_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combine_Builders", x => x.CB_ID);
                    table.ForeignKey(
                        name: "FK_Combine_Builders_Combines_CombinesC_ID",
                        column: x => x.CombinesC_ID,
                        principalTable: "Combines",
                        principalColumn: "C_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Combine_Builders_Tests_TestsTest_ID",
                        column: x => x.TestsTest_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Combine_Results",
                columns: table => new
                {
                    CR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Point = table.Column<int>(nullable: false),
                    C_ID = table.Column<int>(nullable: false),
                    CombinesC_ID = table.Column<int>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true)
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
                        name: "FK_Combine_Results_Combines_CombinesC_ID",
                        column: x => x.CombinesC_ID,
                        principalTable: "Combines",
                        principalColumn: "C_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Assigned_Attendees",
                columns: table => new
                {
                    EA_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    E_ID = table.Column<int>(nullable: false),
                    EventsE_ID = table.Column<int>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true)
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
                        name: "FK_Event_Assigned_Attendees_Events_EventsE_ID",
                        column: x => x.EventsE_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Results",
                columns: table => new
                {
                    ER_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    E_ID = table.Column<int>(nullable: false),
                    EventsE_ID = table.Column<int>(nullable: true),
                    Final_Point = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true)
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
                        name: "FK_Event_Results_Events_EventsE_ID",
                        column: x => x.EventsE_ID,
                        principalTable: "Events",
                        principalColumn: "E_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Individual_Tests",
                columns: table => new
                {
                    IT_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    TestsTest_ID = table.Column<int>(nullable: true),
                    Result = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Point = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individual_Tests", x => x.IT_ID);
                    table.ForeignKey(
                        name: "FK_Individual_Tests_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Individual_Tests_Tests_TestsTest_ID",
                        column: x => x.TestsTest_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Org_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true),
                    Num_Of_Teams = table.Column<int>(nullable: false),
                    Img_Path = table.Column<string>(nullable: true)
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
                name: "Organization_Relationships",
                columns: table => new
                {
                    OR_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true),
                    Org_ID = table.Column<int>(nullable: false),
                    OrganizationsOrg_ID = table.Column<int>(nullable: true),
                    Role = table.Column<string>(nullable: true)
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
                        name: "FK_Organization_Relationships_Organizations_OrganizationsOrg_ID",
                        column: x => x.OrganizationsOrg_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Team_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Org_ID = table.Column<int>(nullable: false),
                    OrganizationsOrg_ID = table.Column<int>(nullable: true),
                    Sport_ID = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Img_Path = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true),
                    Num_Of_Members = table.Column<int>(nullable: false)
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
                        name: "FK_Teams_Organizations_OrganizationsOrg_ID",
                        column: x => x.OrganizationsOrg_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Test_Org_Relationships",
                columns: table => new
                {
                    TOR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    TestsTest_ID = table.Column<int>(nullable: true),
                    Org_ID = table.Column<int>(nullable: false),
                    OrganizationsOrg_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Org_Relationships", x => x.TOR);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationships_Organizations_OrganizationsOrg_ID",
                        column: x => x.OrganizationsOrg_ID,
                        principalTable: "Organizations",
                        principalColumn: "Org_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Org_Relationships_Tests_TestsTest_ID",
                        column: x => x.TestsTest_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team_Memberships",
                columns: table => new
                {
                    TM_ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team_ID = table.Column<int>(nullable: false),
                    TeamsTeam_ID = table.Column<int>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    AppUserId = table.Column<int>(nullable: true)
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
                        name: "FK_Team_Memberships_Teams_TeamsTeam_ID",
                        column: x => x.TeamsTeam_ID,
                        principalTable: "Teams",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Test_Team_Relationships",
                columns: table => new
                {
                    TTR = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Test_ID = table.Column<int>(nullable: false),
                    TestsTest_ID = table.Column<int>(nullable: true),
                    Team_ID = table.Column<int>(nullable: false),
                    TeamsTeam_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Team_Relationships", x => x.TTR);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationships_Teams_TeamsTeam_ID",
                        column: x => x.TeamsTeam_ID,
                        principalTable: "Teams",
                        principalColumn: "Team_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Team_Relationships_Tests_TestsTest_ID",
                        column: x => x.TestsTest_ID,
                        principalTable: "Tests",
                        principalColumn: "Test_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PermissionsP_ID",
                table: "AspNetUsers",
                column: "PermissionsP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_CombinesC_ID",
                table: "Combine_Builders",
                column: "CombinesC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Builders_TestsTest_ID",
                table: "Combine_Builders",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_AppUserId",
                table: "Combine_Results",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Combine_Results_CombinesC_ID",
                table: "Combine_Results",
                column: "CombinesC_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_AppUserId",
                table: "Event_Assigned_Attendees",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Assigned_Attendees_EventsE_ID",
                table: "Event_Assigned_Attendees",
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
                name: "IX_Event_Results_AppUserId",
                table: "Event_Results",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Results_EventsE_ID",
                table: "Event_Results",
                column: "EventsE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Individual_Tests_AppUserId",
                table: "Individual_Tests",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Individual_Tests_TestsTest_ID",
                table: "Individual_Tests",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_AppUserId",
                table: "Organization_Relationships",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Relationships_OrganizationsOrg_ID",
                table: "Organization_Relationships",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AppUserId",
                table: "Organizations",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_AppUserId",
                table: "Team_Memberships",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Memberships_TeamsTeam_ID",
                table: "Team_Memberships",
                column: "TeamsTeam_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppUserId",
                table: "Teams",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationsOrg_ID",
                table: "Teams",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_OrganizationsOrg_ID",
                table: "Test_Org_Relationships",
                column: "OrganizationsOrg_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Org_Relationships_TestsTest_ID",
                table: "Test_Org_Relationships",
                column: "TestsTest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_TeamsTeam_ID",
                table: "Test_Team_Relationships",
                column: "TeamsTeam_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Team_Relationships_TestsTest_ID",
                table: "Test_Team_Relationships",
                column: "TestsTest_ID");


            migrationBuilder.Sql("INSERT INTO Permissions (`P_ID`, `Permission`) VALUES (1,'All');");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (`Name`,`NormalizedName`) VALUES ('Admin','ADMIN');");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (`Name`,`NormalizedName`) VALUES ('Organization','ORGANIZATION');");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (`Name`,`NormalizedName`) VALUES ('Manager','MANAGER');");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (`Name`,`NormalizedName`) VALUES ('Athlete','ATHLETE');");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity_Scheduleds");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Combine_Builders");

            migrationBuilder.DropTable(
                name: "Combine_Results");

            migrationBuilder.DropTable(
                name: "Event_Assigned_Attendees");

            migrationBuilder.DropTable(
                name: "Event_Builders");

            migrationBuilder.DropTable(
                name: "Event_Results");

            migrationBuilder.DropTable(
                name: "Individual_Tests");

            migrationBuilder.DropTable(
                name: "Organization_Relationships");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Team_Memberships");

            migrationBuilder.DropTable(
                name: "Test_Org_Relationships");

            migrationBuilder.DropTable(
                name: "Test_Team_Relationships");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

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
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
