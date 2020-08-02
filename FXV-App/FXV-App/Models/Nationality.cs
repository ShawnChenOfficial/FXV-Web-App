using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Nationality
    {
        [Key]
        public int Nationality_ID { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string CountryCode { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        //Foreign key

        //Relationships
        ICollection<AppUser> AppUsers { get; set; }
    }
}
