using FXV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FXV.Models
{
    public class Event
    {
        [Key]
        public int E_ID { get; set; }

        [ConcurrencyCheck]
        public DateTime Date { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public string Location { get; set; }

        [ConcurrencyCheck]
        public DateTime Time { get; set; }

        [ConcurrencyCheck]
        public string Img_Path { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key
        public Event_Status Event_Status { get; set; }
        //Relationships
        ICollection<Combine_Result> Combine_Results { get; set; }
        ICollection<Event_Assigned_Attendee> event_Assigned_Attendees { get; set; }
        ICollection<Event_Builder> Event_Builders { get; set; }
        ICollection<Event_Result> Event_Results { get; set; }
        
        ICollection<Test_Result> Test_Results { get; set; }
    }
}
