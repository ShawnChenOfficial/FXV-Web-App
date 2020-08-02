using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;

namespace FXV.ViewModels
{
    public class EventResultCollection
    {
        [Required]
        public int E_ID { get; set; }
        [Required(ErrorMessage ="Please enter attendee")]
        public string AttendeeFullName { get; set; }
        public int AttendeeId { get; set; }

        public List<Test_Result> Test_Results { get; set; }

        public Test_Result Test_Result { get; set; }
    }
}
