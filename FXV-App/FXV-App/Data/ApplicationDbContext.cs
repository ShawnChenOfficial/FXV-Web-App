using System;
using FXV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FXV.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Combine_Builder> Combine_Builder { get; set; }
        public virtual DbSet<Combine_Result> Combine_Result { get; set; }
        public virtual DbSet<Combine> Combine { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Event_Builder> Event_Builder { get; set; }
        public virtual DbSet<Event_Result> Event_Result { get; set; }
        public virtual DbSet<Test_Result> Test_Result { get; set; }
        public virtual DbSet<Organization_Relationship> Organization_Relationship { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<SubscriptionPermission> SubscriptionPermission { get; set; }
        public virtual DbSet<Sport> Sport { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Team_Membership> Team_Membership { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Activity_Scheduled> Activity_Scheduled { get; set; }
        public virtual DbSet<Test_Org_Relationship> Test_Org_Relationship { get; set; }
        public virtual DbSet<Test_Team_Relationship> Test_Team_Relationship { get; set; }
        public virtual DbSet<Event_Assigned_Attendee> Event_Assigned_Attendee { get; set; }
        public virtual DbSet<AthleteAchievement> AthleteAchievement { get; set; }
        public virtual DbSet<TempEventTable> TempEventTable { get; set; }
        public virtual DbSet<Test_Category> Test_Category { get; set; }
        public virtual DbSet<Combine_Categories_Weight> Combine_Categories_Weight { get; set; }
        public virtual DbSet<Event_Status> Event_Statuses { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
    }
}
