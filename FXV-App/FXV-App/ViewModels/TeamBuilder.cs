using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class TeamBuilder
    {
        public int Team_ID { get; set; }
        [Required]
        [Display(Name = ("Team Name"))]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$", ErrorMessage = ("Invalid Team Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = ("Location"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Location { get; set; }
        [Required]
        [Display(Name = ("Description"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }

        [Required(ErrorMessage = "Must have a team manager")]
        [Display(Name = ("Owner"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Please select from the drop down list"))]
        public string Manager_Name { get; set; }
        public AppUser Owner { get; set; }

        public List<TeamMember> Members { get; set; }

        [Display(Name = ("Upload Organization Image"))]
        public IFormFile Image { get; set; }

        public string Img_Path { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string RowVersionBase64
        {
            get
            {
                return RowVersion == null ? "" : Convert.ToBase64String(RowVersion);
            }
        }
    }


    public class TeamMember
    {
        public Member Member { get; set; }
        public string Role { get; set; }
    }

    public class Member
    {
        public int Id { get; set; }
        [Display(Name = ("Members"))]
        public string FullName { get; set; }
    }
}
