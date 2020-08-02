using System;
namespace FXV.ViewModels
{
    public class ResetPwdTokenParams
    {
        public int UID { get; set; }
        public string Email { get; set; }
        public string UserManagerToken { get; set; }

    }
}
