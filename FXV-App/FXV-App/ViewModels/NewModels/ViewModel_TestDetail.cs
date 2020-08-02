using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_TestDetail
    {
        public int TestId { get; set; }
        public string Sys_Permission { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public Top Top { get; set; }
        public Promiex Promiex { get; set; }
        public Bronze Bronze { get; set; }
    }
}
