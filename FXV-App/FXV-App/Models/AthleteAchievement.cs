using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXV.Models
{
    public class AthleteAchievement
    {
        [Key]
        public int Achievement_ID { get; set; }

        [ConcurrencyCheck]
        public int Id { get; set; }

        [ConcurrencyCheck]
        public string Achievement { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public AppUser AppUser { get; set; }

        //Relationships
    }
}
