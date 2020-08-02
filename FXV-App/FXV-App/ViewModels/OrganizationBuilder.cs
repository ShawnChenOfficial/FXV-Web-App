using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Models;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class OrganizationBuilder
    {
        public int Org_ID { get; set; }
        [Required]
        [Display(Name =("Organization Name"))]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$", ErrorMessage = ("Invalid Organization Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = ("Location"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Location { get; set; }
        [Required]
        [Display(Name = ("Description"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }

        public AppUser Owner { get; set; }

        [Required(ErrorMessage = "Must have an organization manager")]
        [Display(Name = ("Manager Name"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Please select from the drop down list"))]
        public string Manager_Name {get;set;}

        public IFormFile Image { get; set; }

        public string Image_Path { get; set; }

        public byte[] RowVersion { get; set; }

        public string RowVersionBase64
        {
            get
            {
                return RowVersion == null ? "" : Convert.ToBase64String(RowVersion);
            }
        }
    }
}

