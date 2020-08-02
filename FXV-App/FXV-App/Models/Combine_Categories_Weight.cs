using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Combine_Categories_Weight
    {
        [Key]
        public int CC_ID { get; set; }

        [ConcurrencyCheck]
        public int C_ID { get; set; }

        [ConcurrencyCheck]
        public int TC_ID { get; set; }

        [ConcurrencyCheck]
        public double Weight { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Combine Combine { get; set; }
        public Test_Category Test_Category { get; set; }

        //Relationships
    }
}
