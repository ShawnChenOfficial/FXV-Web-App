using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class NewSystemUser
    {
        [Required]
        [Display(Name = "Email: ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Gender")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = ("Invalid Firstname, Only Accept a-z A-Z"))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Lastname")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = ("Invalid Last Name, Only Accept a-z A-Z"))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z _ , . ~ ' -"))]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z _ , . ~ ' -"))]
        public string City { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
