using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class OrganizationTeamMembersList
    {
        public bool AdminAccess { get; set; }
        public List<AppUser> OrganizationTeamMembers { get; set; } = new List<AppUser>();
    }
}
