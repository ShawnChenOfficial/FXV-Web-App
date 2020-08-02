using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Organization_Relationship
    {
        [Key]
        public int OR_ID { get; set; }

        [Column("Id")]
        [ConcurrencyCheck]
        public int Id { get; set; }

        [ConcurrencyCheck]
        public int Org_ID { get; set; }

        [ConcurrencyCheck]
        public string Role { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public AppUser AppUser { get; set; }
        public Organization Organization { get; set; }

        //Relationships
    }
}
