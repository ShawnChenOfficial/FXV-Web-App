using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_Team
    {
        public int TeamId { get; set; }
        public int OrgId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Img_Path { get; set; }
        public string ManagerName { get; set; }
        public int NumberOfMembers { get; set; }
        public string Organization { get; set; }
        public string Sport { get; set; }
    }
}
