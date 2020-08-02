using System;
namespace FXV.ViewModels
{
    public class Leaderboard_TestResultsList : IComparable<Leaderboard_TestResultsList>
    {
        public int User_ID { get; set; }
        public string Runner_Name { get; set; }
        public string Result { get; set; }
        public int Point { get; set; }

        public int CompareTo(Leaderboard_TestResultsList other)
        {
            return other.Point.CompareTo(this.Point);
        }
    }
    public class Leaderboard_CombineResultsList : IComparable<Leaderboard_CombineResultsList>
    {
        public int User_ID { get; set; }
        public string Runner_Name { get; set; }
        public int Point { get; set; }

        public int CompareTo(Leaderboard_CombineResultsList other)
        {
            return other.Point.CompareTo(this.Point);
        }
    }
    public class Leaderboard_EventResultsList : IComparable<Leaderboard_EventResultsList>
    {
        public int User_ID { get; set; }
        public string Runner_Name { get; set; }
        public int Point { get; set; }

        public int CompareTo(Leaderboard_EventResultsList other)
        {
            return other.Point.CompareTo(this.Point);
        }
    }
}
