using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Models;
using Microsoft.AspNetCore.Identity;

namespace FXV.Models
{
    public class AppUser :IdentityUser<int>
    {
        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string Gender { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string City { get; set; }

        [DataType(DataType.Date)]
        [ConcurrencyCheck]
        public DateTime DOB { get; set; }

        [DataType(DataType.Text)]
        public string Salt_1 { get; set; }

        [DataType(DataType.Text)]
        public string Salt_2 { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string Description { get; set; } = "No information on this athlete.";


        public int Nationality_ID { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;
        
        public int P_ID { get; set; }

        [ConcurrencyCheck]
        public string Profile_Img_Path { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public SubscriptionPermission SubscriptionPermission { get; set; }
        public Nationality Nationality { get; set; }

        //Relationships
        ICollection<AthleteAchievement> AthleteAchievements { get; set; }
        ICollection<Combine_Result> Combine_Results { get; set; }
        ICollection<Event_Assigned_Attendee> event_Assigned_Attendees { get; set; }
        ICollection<Event_Result> Event_Results { get; set; }
        ICollection<Organization> Organizations { get; set; } // the manager of org
        ICollection<Organization_Relationship> Organization_Relationships { get; set; }
        ICollection<Team> Teams { get; set; }
        ICollection<Team_Membership> Team_Memberships { get; set; }
        ICollection<Test_Result> Test_Results { get; set; }

    }
}
