using System;
using System.Collections.Generic;

namespace FXV.ViewModels.NewModels
{
    public class ViewModel_CombineDetail
    {
        public string Sys_Permission { get; set; }
        public List<string> TestNames { get; set; }
        public Top Top { get; set; }
        public Promiex Promiex { get; set; }
        public Bronze Bronze { get; set; }
    }
}
