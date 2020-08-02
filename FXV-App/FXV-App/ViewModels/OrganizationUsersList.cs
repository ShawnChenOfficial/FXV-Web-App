using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class OrganizationUsersList
    {
        public bool AdminAccess { get; set; }
        public List<AppUser> OrganizationUsers { get; set; } = new List<AppUser>();
    }
}
