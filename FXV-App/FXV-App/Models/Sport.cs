using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Sport
    {
        [Key]
        public int Sport_ID { get; set; }

        [ConcurrencyCheck]
        public string Sport_Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key

        //Relationships
        ICollection<Team> Teams { get; set; }
    }
}
