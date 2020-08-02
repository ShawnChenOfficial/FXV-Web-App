using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;

namespace FXV.ViewModels
{
    public class EventAttendeeBuilder
    {

        [Required]
        [Display(Name = "Attendees")]
        public List<string> Members { get; set; }

        public List<AppUser> Members_Full { get; set; }
    }

}
