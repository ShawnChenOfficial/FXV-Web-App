using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class LoginInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Account")]
        [DataType(DataType.EmailAddress)]
        public string Account { get; set; }

        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = ("The length of password must meet the limit 8 ~ 40"))]
        [MaxLength(40, ErrorMessage = ("The length of password must meet the limit 8 ~ 40"))]
        public string Password { get; set; }
    }
}
