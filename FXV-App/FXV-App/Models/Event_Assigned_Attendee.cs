using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;
using FXV.Models;

namespace FXV.Models
{
    public class Event_Assigned_Attendee
    {
        [Key]
        public int EA_ID {get;set;}

        [ConcurrencyCheck]
        public int E_ID { get; set; }

        [ConcurrencyCheck]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Event Event { get; set; }
        public AppUser AppUser { get; set; }

        //Relationships
    }
}
