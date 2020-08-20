using System;
using System.Collections.Generic;

namespace FXV.ViewModels.NewModels
{
    public class ViewModel_TestActivity
    {
        public int TestId { get; set; }
        public List<int> SplitTestIds { get; set; }
        public List<int> AttendeeIds { get; set; }
    }
}
