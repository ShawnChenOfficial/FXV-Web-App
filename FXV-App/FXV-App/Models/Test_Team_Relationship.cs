using System;
using System.ComponentModel.DataAnnotations;
using FXV.Models;

namespace FXV.Models
{
    public class Test_Team_Relationship
    {
        [Key]
        public int TTR { get; set; }

        [ConcurrencyCheck]
        public int Test_ID { get; set; }

        [ConcurrencyCheck]
        public int Team_ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Test Test { get; set; }
        public Team Team { get; set; }

        //Relationships
    }
}
