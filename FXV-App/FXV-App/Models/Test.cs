using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class Test
    {
        [Key]
        public int Test_ID { get; set; }

        [ConcurrencyCheck]
        [Required]
        public string Name {get;set; }

        [ConcurrencyCheck]
        public string Gender { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public bool Public { get; set; }

        [ConcurrencyCheck]
        public bool Reverse { get; set; }

        [ConcurrencyCheck]
        public string Unit { get; set; }

        [ConcurrencyCheck]
        public double LowerResult { get; set; }

        [ConcurrencyCheck]
        public double HigherResult { get; set; }

        [ConcurrencyCheck]
        public int LowerScore { get; set; }

        [ConcurrencyCheck]
        public int HigherScore { get; set; }

        [ConcurrencyCheck]
        public int TC_id { get; set; }

        //2 attribute just remain, until next upgrade.
        //[ConcurrencyCheck]
        //public double LowerCalc { get; set; }

        //[ConcurrencyCheck]
        //public double HigherCalc { get; set; }

        
        public bool IsVerified { get; set; }

        public bool IsSplittable { get; set; }
        public bool UsedAsSplit { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Test_Category Test_Category { get; set; }

        //Relationships
        ICollection<Combine_Builder> Combine_Builders { get; set; }
        ICollection<Test_Org_Relationship> Test_Org_Relationships { get; set; }
        ICollection<Test_Result> Test_Results { get; set; }
        ICollection<Test_Team_Relationship> Test_Team_Relationships { get; set; }
    }
}
