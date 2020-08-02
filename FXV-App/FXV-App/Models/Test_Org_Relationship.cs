using System;
using System.ComponentModel.DataAnnotations;
using FXV.Models;

namespace FXV.Models
{
    public class Test_Org_Relationship
    {
        [Key]
        public int TOR { get; set; }

        [ConcurrencyCheck]
        public int Test_ID { get; set; }

        [ConcurrencyCheck]
        public int Org_ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Organization Organization { get; set; }
        public Test Test { get; set; }

        //Relationships
    }
}
