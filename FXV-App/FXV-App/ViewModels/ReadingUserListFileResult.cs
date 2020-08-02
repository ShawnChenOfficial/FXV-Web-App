using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class ReadingUserListFileResult
    {
        public int OverAllUsers { get; set; }
        public int Success { get; set; }
        public List<string> ExistingUsers { get; set; }
        public List<string> FailedStoredUsers { get; set; }
    }

    public class ReadingUserListWithCombineFileResult
    {
        public int OverAllUsers { get; set; }
        public int Success { get; set; }
        public List<string> ExistingUsers { get; set; }
        public List<string> FailedStoredUsers { get; set; }
        public List<string> FailedSignInOrg { get; set; }
        public List<string> FailedSignInTeam { get; set; }
    }

    public class Tests_In_Category
    {
        public int TC_id { get; set; }
        public List<int> Test_ID { get; set; }
        public double Weight { get; set; }
    }
}
