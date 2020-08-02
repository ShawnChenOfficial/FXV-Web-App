using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Organization
    {
        [Key]
        public int Org_ID { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        [ConcurrencyCheck]
        public string Location { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public int Id { get; set; }

        [ConcurrencyCheck]
        public int Num_Of_Teams { get; set; }

        [ConcurrencyCheck]
        public string Img_Path { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public AppUser AppUser { get; set; }

        //Relationships

        ICollection<Organization_Relationship> organization_Relationships { get; set; }
        ICollection<Team> Teams { get; set; }
        ICollection<Test_Org_Relationship> test_Org_Relationships { get; set; }
    }
}
