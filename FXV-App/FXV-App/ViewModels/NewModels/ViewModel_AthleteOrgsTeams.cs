using System;
using System.Collections.Generic;

namespace FXV.ViewModels.NewModels
{
    public class ViewModel_AthleteOrgsTeams
    {
        public int Org_ID { get; set; }
        public string Organization { get; set; }
        public List<ViewModel_Team> Teams { get; set; } = new List<ViewModel_Team>();
    }
}
