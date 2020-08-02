using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Combine_Builder
    {
        [Key]
        public int CB_ID { get; set; }

        [ConcurrencyCheck]
        public int Test_ID { get; set; }

        [ConcurrencyCheck]
        public int C_ID { get; set; }

        [ConcurrencyCheck]
        public int Attempt { get; set; }

        [ConcurrencyCheck]
        public bool HasSplit { get; set; }

        [ConcurrencyCheck]
        public bool IsWeighted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Test Test { get; set; }
        public Combine Combine { get; set; }

        //Relationships
    }
}
