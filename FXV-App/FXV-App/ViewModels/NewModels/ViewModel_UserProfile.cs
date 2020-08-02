using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels.NewModels
{
    public class ViewModel_UserProfile
    {
        public int Id { get; set; }
        public bool Editable { get; set; }
        public string Nationality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string TopAchevenment { get; set; }
    }
}
