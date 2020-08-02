using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class SignUp
    {
        [Required]
        [Display(Name ="Email: ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = ("The length of password must meet the limit of 8 ~ 40"))]
        [MaxLength(40, ErrorMessage = ("The length of password must meet the limit of 8 ~ 40"))]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = ("The length of password must meet the limit of 8 ~ 40"))]
        [MaxLength(40, ErrorMessage = ("The length of password must meet the limit of 8 ~ 40"))]
        public string Re_Password { get; set; }
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
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z"))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Lastname")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z"))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string City { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Select your nationality")]
        [Display(Name = "Nationtality")]
        public int Nationality_ID { get; set; }
    }
}
