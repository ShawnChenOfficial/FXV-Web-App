using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_User
    {
        public int UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Discription { get; set; }
        public DateTime DOB { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Profile_Img_Path { get; set; }
        public string Subscription { get; set; }
        public string SystemRole { get; set; }
    }
}
