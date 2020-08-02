using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;

namespace FXV.Models
{
    public class Event_Result : IComparable<Event_Result>
    {
        [Key]
        public int ER_ID { get; set; }

        [ConcurrencyCheck]
        public int E_ID { get; set; }

        [ConcurrencyCheck]
        public int Final_Point { get; set; }

        [Column("Id")]
        [ConcurrencyCheck]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public AppUser AppUser { get; set; }
        public Event Event { get; set; }

        //Relationships


        public int CompareTo(Event_Result other)
        {
            return this.Final_Point.CompareTo(other.Final_Point);
        }
    }
}
