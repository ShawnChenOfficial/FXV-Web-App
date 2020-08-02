using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class EventsList
    {
        public bool IsAdmin { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
