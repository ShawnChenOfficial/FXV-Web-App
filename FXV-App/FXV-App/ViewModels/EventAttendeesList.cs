using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class EventAttendeesList
    {
        public bool AdminAccess { get; set; }
        public bool NotPastEvent { get; set; }
        public List<AppUser> Attendees { get; set; } = new List<AppUser>();
    }
}
