using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Combine_Result:IComparable<Combine_Result>
    {
        [Key]
        public int CR_ID { get; set; }

        [ConcurrencyCheck]
        public int Point { get; set; }

        [ConcurrencyCheck]
        public int E_ID { get; set; }

        public int C_ID { get; set; }
        
        [ConcurrencyCheck]
        public int Id { get; set; }

        [ConcurrencyCheck]
        public double Height { get; set; }

        [ConcurrencyCheck]
        public double Weight { get; set; }

        [ConcurrencyCheck]
        public double Standing_Reach { get; set; }

        [ConcurrencyCheck]
        public double Wingspan { get; set; }

        [ConcurrencyCheck]
        public double Handspan { get; set; }

        [ConcurrencyCheck]
        public string Dominant_Hand { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Event Event { get; set; }
        public AppUser AppUser { get; set; }
        public Combine Combine { get; set; }

        //Relationships


        public int CompareTo(Combine_Result other)
        {
            return this.Point.CompareTo(other.Point);
        }
    }
}
