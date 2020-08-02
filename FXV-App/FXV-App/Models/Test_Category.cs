using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Test_Category
    {
        [Key]
        public int TC_id { get; set; }

        [ConcurrencyCheck]
        public string Category { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key

        //Relationships
        ICollection<Combine_Categories_Weight> Combine_Categories_Weights { get; set; }
        ICollection<Test> Tests { get; set; }
    }
}
