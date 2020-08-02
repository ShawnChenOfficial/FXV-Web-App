using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class ResetPwdAccount
    {
        [Required(ErrorMessage ="You must enter your email account")]
        [Display(Name = "Account")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int UID { get; set; }
    }
}
