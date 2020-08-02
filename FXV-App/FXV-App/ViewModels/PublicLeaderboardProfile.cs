using System;
using System.Collections.Generic;

namespace FXV.ViewModels
{
    public class PublicLeaderboardProfile
    {
        public string Event_Name { get; set; }
        public int Attendee_ID { get; set; }
        public string Attendee_FirstName { get; set; }
        public string Attendee_LastName { get; set; }
        public List<string> Achievement { get; set; }
        public bool IsFinish { get; set; }
    }
}
