using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class ResetPwd
    {
        public int UID { get; set; }
        [Required(ErrorMessage = "Please enter new password.")]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        public string NewPwd { get; set; }

        [Required(ErrorMessage = "Please confirm new password.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPwd { get; set; }

        public string Email { get; set; }
        public string UserManagerToken { get; set; }
    }
}
