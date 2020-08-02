using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Test_Result : IComparable<Test_Result>
    {
        [Key]
        public int TR_ID { get; set; }

        [ConcurrencyCheck]
        public int Test_ID { get; set; }

        [ConcurrencyCheck]
        public int E_ID { get; set; }

        [ConcurrencyCheck]
        public double Result { get; set; }

        [ConcurrencyCheck]
        public int Point { get; set; }

        [ConcurrencyCheck]
        public DateTime Date { get; set; }

        [ConcurrencyCheck]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Test Test { get; set; }
        public Event Event { get; set; }
        public AppUser AppUser { get; set; }

        //Relationships

        public int CompareTo(Test_Result other)
        {
            return this.Point.CompareTo(other.Point);
        }
    }

}
