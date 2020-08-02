using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class OrganizationTeamsList
    {
        public bool AdminAccess { get; set; }
        public List<Team> OrganizationTeams { get; set; } = new List<Team>();
    }
}
