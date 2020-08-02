using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Event_Builder
    {
        [Key]
        public int EB_ID { get; set; }

        [ConcurrencyCheck]
        public int C_ID { get; set; }

        [ConcurrencyCheck]
        public int E_ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Combine Combine { get; set; }
        public Event Event { get; set; }

        //Relationships
    }
}
