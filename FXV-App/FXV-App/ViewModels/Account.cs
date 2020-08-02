using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class Account
    {
        [Required(ErrorMessage = "Please enter a valid email address")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string UserAccount { get; set; }
    }
}
