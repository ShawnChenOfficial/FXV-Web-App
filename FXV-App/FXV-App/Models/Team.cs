using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Team
    {
        [Key]
        public int Team_ID { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        [ConcurrencyCheck]
        public int Org_ID { get; set; }

        [ConcurrencyCheck]
        public int Sport_ID { get; set; }

        [ConcurrencyCheck]
        public string Location { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public string Img_Path { get; set; }

        [Column("Id")]
        [ConcurrencyCheck]
        public int Id { get; set; }

        [ConcurrencyCheck]
        public int Num_Of_Members { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Organization Organization { get; set; }
        public Sport Sport { get; set; }
        public AppUser AppUser { get; set; }

        //Relationships
        ICollection<Team_Membership> Team_Memberships { get; set; }
        ICollection<Test_Team_Relationship> test_Team_Relationships { get; set; }
    }
}
