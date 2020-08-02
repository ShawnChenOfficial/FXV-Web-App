using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_Organization
    {
        public int OrgId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ManagerName { get; set; }
        public int NumberOfTeams { get; set; }
        public int NumberOfMembers { get; set; }
        public string ImgPath { get; set; }
    }
}
