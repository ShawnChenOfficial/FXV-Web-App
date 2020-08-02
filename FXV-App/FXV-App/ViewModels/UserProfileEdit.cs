using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class UserProfileEdit
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile Profile_Image { get; set; }
        public string Profile_Img_Path { get; set; }

        [Display(Name = "Achievement(s)")]
        public List<string> AthleteAchievements { get; set; } = new List<string>();

        [Required(ErrorMessage = "Select your nationality")]
        [Display(Name = "Nationtality")]
        public int Nationality_ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string RowVersionBase64
        {
            get
            {
                return Convert.ToBase64String(RowVersion);
            }
        }
    }
}

