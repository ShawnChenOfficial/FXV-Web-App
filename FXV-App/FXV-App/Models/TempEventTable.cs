using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.Models
{
    public class TempEventTable
    {
        [Key]
        public int Tem_ID { get; set; }
        [ConcurrencyCheck]
        public int Event_ID { get; set; }
        [ConcurrencyCheck]
        public int Athlete_ID { get; set; }
        [ConcurrencyCheck]
        public int Test_ID { get; set; }
        [ConcurrencyCheck]
        public bool IsFinished { get; set; }
        //Foreign key
        //Relationships
    }
}
