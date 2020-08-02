using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Team_Membership
    {
        [Key]
        public int TM_ID { get; set; }

        [ConcurrencyCheck]
        public int Team_ID { get; set; }

        [ConcurrencyCheck]
        public string Role { get; set; }

        [ConcurrencyCheck]
        public string Position { get; set; }

        [Column("Id")]
        [ConcurrencyCheck]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public AppUser AppUser { get; set; }
        public Team Team { get; set; }

        //Relationships
    }
}
