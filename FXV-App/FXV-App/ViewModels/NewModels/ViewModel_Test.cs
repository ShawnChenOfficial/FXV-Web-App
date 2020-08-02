using System;
namespace FXV.ViewModels.NewModels
{
    public class ViewModel_Test
    {
        public int TestId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Img_Path { get; set; }
        public bool Visible { get; set; }
        public bool Reverse { get; set; }
        public string Unit { get; set; }
        public double LowerResult { get; set; }
        public double HigherResult { get; set; }
        public int LowerScore { get; set; }
        public int HigherScore { get; set; }
        public string Category { get; set; }
        public int Tested { get; set; }
    }
}
