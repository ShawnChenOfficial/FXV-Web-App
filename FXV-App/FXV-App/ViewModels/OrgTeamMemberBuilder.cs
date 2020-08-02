using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;

namespace FXV.ViewModels
{
    public class OrgTeamMemberBuilder
    {

        [Display(Name ="Members")]
        public List<string> Members { get; set; }
        public List<AppUser> Members_Full_Users { get; set; }
    }
}
