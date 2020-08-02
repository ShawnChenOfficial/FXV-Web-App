﻿// <auto-generated />
using System;
using FXV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FXV_App.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200228235844_20200229_version_v1")]
    partial class _20200229_version_v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FXV.Models.Activity_Scheduled", b =>
                {
                    b.Property<int>("AS_ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("AS_ID");

                    b.ToTable("Activity_Scheduleds");
                });

            modelBuilder.Entity("FXV.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("FXV.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address")
                        .IsConcurrencyToken();

                    b.Property<string>("City")
                        .IsConcurrencyToken();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB")
                        .IsConcurrencyToken();

                    b.Property<string>("Discription")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsConcurrencyToken();

                    b.Property<string>("Gender")
                        .IsConcurrencyToken();

                    b.Property<string>("LastName")
                        .IsConcurrencyToken();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<int>("P_ID");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Profile_Img_Path")
                        .IsConcurrencyToken();

                    b.Property<string>("Salt_1");

                    b.Property<string>("Salt_2");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("P_ID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FXV.Models.AthleteAchievements", b =>
                {
                    b.Property<int>("Achievement_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Achievement")
                        .IsConcurrencyToken();

                    b.Property<int?>("AppUserId");

                    b.Property<int>("UserID");

                    b.HasKey("Achievement_ID");

                    b.HasIndex("AppUserId");

                    b.ToTable("AthleteAchievements");
                });

            modelBuilder.Entity("FXV.Models.Combine_Builder", b =>
                {
                    b.Property<int>("CB_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Attempt")
                        .IsConcurrencyToken();

                    b.Property<int>("C_ID")
                        .IsConcurrencyToken();

                    b.Property<bool>("HasSplit")
                        .IsConcurrencyToken();

                    b.Property<bool>("IsWeighted")
                        .IsConcurrencyToken();

                    b.Property<int>("Test_ID")
                        .IsConcurrencyToken();

                    b.HasKey("CB_ID");

                    b.HasIndex("C_ID");

                    b.HasIndex("Test_ID");

                    b.ToTable("Combine_Builders");
                });

            modelBuilder.Entity("FXV.Models.Combine_Categories_Weight", b =>
                {
                    b.Property<int>("CC_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("C_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("TC_ID")
                        .IsConcurrencyToken();

                    b.Property<double>("Weight")
                        .IsConcurrencyToken();

                    b.HasKey("CC_ID");

                    b.HasIndex("C_ID");

                    b.HasIndex("TC_ID");

                    b.ToTable("Combine_Categories_Weights");
                });

            modelBuilder.Entity("FXV.Models.Combine_Results", b =>
                {
                    b.Property<int>("CR_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int>("C_ID");

                    b.Property<string>("Dominant_Hand")
                        .IsConcurrencyToken();

                    b.Property<int>("E_ID")
                        .IsConcurrencyToken();

                    b.Property<double>("Handspan")
                        .IsConcurrencyToken();

                    b.Property<double>("Height")
                        .IsConcurrencyToken();

                    b.Property<int>("Point")
                        .IsConcurrencyToken();

                    b.Property<double>("Standing_Reach")
                        .IsConcurrencyToken();

                    b.Property<int>("User_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.Property<double>("Weight")
                        .IsConcurrencyToken();

                    b.Property<double>("Wingspan")
                        .IsConcurrencyToken();

                    b.HasKey("CR_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("E_ID");

                    b.ToTable("Combine_Results");
                });

            modelBuilder.Entity("FXV.Models.Combines", b =>
                {
                    b.Property<int>("C_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsConcurrencyToken();

                    b.Property<string>("Gender")
                        .IsConcurrencyToken();

                    b.Property<string>("Img_Path")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsConcurrencyToken();

                    b.HasKey("C_ID");

                    b.ToTable("Combines");
                });

            modelBuilder.Entity("FXV.Models.Event_Assigned_Attendees", b =>
                {
                    b.Property<int>("EA_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int>("Attendee_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.Property<int>("E_ID")
                        .IsConcurrencyToken();

                    b.HasKey("EA_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("E_ID");

                    b.ToTable("Event_Assigned_Attendees");
                });

            modelBuilder.Entity("FXV.Models.Event_Builders", b =>
                {
                    b.Property<int>("EB_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("C_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("E_ID")
                        .IsConcurrencyToken();

                    b.HasKey("EB_ID");

                    b.HasIndex("C_ID");

                    b.HasIndex("E_ID");

                    b.ToTable("Event_Builders");
                });

            modelBuilder.Entity("FXV.Models.Event_Results", b =>
                {
                    b.Property<int>("ER_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int>("E_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Final_Point")
                        .IsConcurrencyToken();

                    b.Property<int>("User_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.HasKey("ER_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("E_ID");

                    b.ToTable("Event_Results");
                });

            modelBuilder.Entity("FXV.Models.Events", b =>
                {
                    b.Property<int>("E_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date")
                        .IsConcurrencyToken();

                    b.Property<string>("Description")
                        .IsConcurrencyToken();

                    b.Property<string>("Img_Path")
                        .IsConcurrencyToken();

                    b.Property<string>("Location")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Time")
                        .IsConcurrencyToken();

                    b.HasKey("E_ID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("FXV.Models.Organization_Relationships", b =>
                {
                    b.Property<int>("OR_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int>("Org_ID")
                        .IsConcurrencyToken();

                    b.Property<string>("Role")
                        .IsConcurrencyToken();

                    b.Property<int>("User_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.HasKey("OR_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("Org_ID");

                    b.ToTable("Organization_Relationships");
                });

            modelBuilder.Entity("FXV.Models.Organizations", b =>
                {
                    b.Property<int>("Org_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Description")
                        .IsConcurrencyToken();

                    b.Property<string>("Img_Path")
                        .IsConcurrencyToken();

                    b.Property<string>("Location")
                        .IsConcurrencyToken();

                    b.Property<int>("Manager_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsConcurrencyToken();

                    b.Property<int>("Num_Of_Teams")
                        .IsConcurrencyToken();

                    b.HasKey("Org_ID");

                    b.HasIndex("AppUserId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("FXV.Models.Permissions", b =>
                {
                    b.Property<int>("P_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Permission")
                        .IsConcurrencyToken();

                    b.HasKey("P_ID");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("FXV.Models.Sports", b =>
                {
                    b.Property<int>("Sport_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Sport_Name")
                        .IsConcurrencyToken();

                    b.HasKey("Sport_ID");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("FXV.Models.Team_Memberships", b =>
                {
                    b.Property<int>("TM_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Position");

                    b.Property<string>("Role")
                        .IsConcurrencyToken();

                    b.Property<int>("Team_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("User_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.HasKey("TM_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("Team_ID");

                    b.ToTable("Team_Memberships");
                });

            modelBuilder.Entity("FXV.Models.Teams", b =>
                {
                    b.Property<int>("Team_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Description")
                        .IsConcurrencyToken();

                    b.Property<string>("Img_Path")
                        .IsConcurrencyToken();

                    b.Property<string>("Location")
                        .IsConcurrencyToken();

                    b.Property<int>("Manager_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsConcurrencyToken();

                    b.Property<int>("Num_Of_Members")
                        .IsConcurrencyToken();

                    b.Property<int>("Org_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Sport_ID")
                        .IsConcurrencyToken();

                    b.HasKey("Team_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("Org_ID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FXV.Models.TempEventTable", b =>
                {
                    b.Property<int>("Tem_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Athlete_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Event_ID")
                        .IsConcurrencyToken();

                    b.Property<bool>("IsFinished")
                        .IsConcurrencyToken();

                    b.Property<int>("Test_ID")
                        .IsConcurrencyToken();

                    b.HasKey("Tem_ID");

                    b.ToTable("TempEventTables");
                });

            modelBuilder.Entity("FXV.Models.Test_Categories", b =>
                {
                    b.Property<int>("TC_id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsConcurrencyToken();

                    b.HasKey("TC_id");

                    b.ToTable("Test_Categories");
                });

            modelBuilder.Entity("FXV.Models.Test_Org_Relationships", b =>
                {
                    b.Property<int>("TOR")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Org_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Test_ID")
                        .IsConcurrencyToken();

                    b.HasKey("TOR");

                    b.HasIndex("Org_ID");

                    b.HasIndex("Test_ID");

                    b.ToTable("Test_Org_Relationships");
                });

            modelBuilder.Entity("FXV.Models.Test_Result", b =>
                {
                    b.Property<int>("TR_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<DateTime>("Date")
                        .IsConcurrencyToken();

                    b.Property<int>("E_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Point")
                        .IsConcurrencyToken();

                    b.Property<double>("Result")
                        .IsConcurrencyToken();

                    b.Property<int>("Test_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("User_ID")
                        .IsConcurrencyToken()
                        .HasColumnName("Id");

                    b.HasKey("TR_ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("E_ID");

                    b.HasIndex("Test_ID");

                    b.ToTable("Test_Result");
                });

            modelBuilder.Entity("FXV.Models.Test_Team_Relationships", b =>
                {
                    b.Property<int>("TTR")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Team_ID")
                        .IsConcurrencyToken();

                    b.Property<int>("Test_ID")
                        .IsConcurrencyToken();

                    b.HasKey("TTR");

                    b.HasIndex("Team_ID");

                    b.HasIndex("Test_ID");

                    b.ToTable("Test_Team_Relationships");
                });

            modelBuilder.Entity("FXV.Models.Tests", b =>
                {
                    b.Property<int>("Test_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsConcurrencyToken();

                    b.Property<string>("Gender")
                        .IsConcurrencyToken();

                    b.Property<double>("HigherCalc")
                        .IsConcurrencyToken();

                    b.Property<double>("HigherResult")
                        .IsConcurrencyToken();

                    b.Property<int>("HigherScore")
                        .IsConcurrencyToken();

                    b.Property<string>("Img_Path")
                        .IsConcurrencyToken();

                    b.Property<double>("LowerCalc")
                        .IsConcurrencyToken();

                    b.Property<double>("LowerResult")
                        .IsConcurrencyToken();

                    b.Property<int>("LowerScore")
                        .IsConcurrencyToken();

                    b.Property<string>("Measurement_Type")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsConcurrencyToken();

                    b.Property<bool>("Reverse")
                        .IsConcurrencyToken();

                    b.Property<int>("TC_id")
                        .IsConcurrencyToken();

                    b.Property<string>("Unit")
                        .IsConcurrencyToken();

                    b.Property<int>("Value")
                        .IsConcurrencyToken();

                    b.Property<bool>("Visible")
                        .IsConcurrencyToken();

                    b.HasKey("Test_ID");

                    b.HasIndex("TC_id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FXV.Models.AppUser", b =>
                {
                    b.HasOne("FXV.Models.Permissions", "Permissions")
                        .WithMany()
                        .HasForeignKey("P_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.AthleteAchievements", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("FXV.Models.Combine_Builder", b =>
                {
                    b.HasOne("FXV.Models.Combines", "Combines")
                        .WithMany()
                        .HasForeignKey("C_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Tests", "Tests")
                        .WithMany()
                        .HasForeignKey("Test_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Combine_Categories_Weight", b =>
                {
                    b.HasOne("FXV.Models.Combines", "Combines")
                        .WithMany()
                        .HasForeignKey("C_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Test_Categories", "Test_Categories")
                        .WithMany()
                        .HasForeignKey("TC_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Combine_Results", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Events", "Events")
                        .WithMany()
                        .HasForeignKey("E_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Event_Assigned_Attendees", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Events", "Events")
                        .WithMany()
                        .HasForeignKey("E_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Event_Builders", b =>
                {
                    b.HasOne("FXV.Models.Combines", "Combines")
                        .WithMany()
                        .HasForeignKey("C_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Events", "Events")
                        .WithMany()
                        .HasForeignKey("E_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Event_Results", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Events", "Events")
                        .WithMany()
                        .HasForeignKey("E_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Organization_Relationships", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Organizations", "Organizations")
                        .WithMany()
                        .HasForeignKey("Org_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Organizations", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("FXV.Models.Team_Memberships", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Teams", "Teams")
                        .WithMany()
                        .HasForeignKey("Team_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Teams", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Organizations", "Organizations")
                        .WithMany()
                        .HasForeignKey("Org_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Test_Org_Relationships", b =>
                {
                    b.HasOne("FXV.Models.Organizations", "Organizations")
                        .WithMany()
                        .HasForeignKey("Org_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Tests", "Tests")
                        .WithMany()
                        .HasForeignKey("Test_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Test_Result", b =>
                {
                    b.HasOne("FXV.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("FXV.Models.Events", "Events")
                        .WithMany()
                        .HasForeignKey("E_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Tests", "Tests")
                        .WithMany()
                        .HasForeignKey("Test_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Test_Team_Relationships", b =>
                {
                    b.HasOne("FXV.Models.Teams", "Teams")
                        .WithMany()
                        .HasForeignKey("Team_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.Tests", "Tests")
                        .WithMany()
                        .HasForeignKey("Test_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FXV.Models.Tests", b =>
                {
                    b.HasOne("FXV.Models.Test_Categories", "Test_Categories")
                        .WithMany()
                        .HasForeignKey("TC_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("FXV.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("FXV.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("FXV.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("FXV.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FXV.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("FXV.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
