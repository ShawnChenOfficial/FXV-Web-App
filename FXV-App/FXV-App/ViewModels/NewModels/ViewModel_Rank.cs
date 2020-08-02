using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_Rank
    {
        public string AthleteName { get; set; }
        public string ImgPath { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int Score { get; set; }
        public double Result { get; set; }
    }

    public class Top : ViewModel_Rank
    {
    }
    public class Promiex : ViewModel_Rank
    {
    }
    public class Bronze : ViewModel_Rank
    {
    }
}
