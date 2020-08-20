using System;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels.NewModels
{
    public enum Status { HasNoRunningActivity,  HasRunningActivities }
    public class ViewModel_Test
    {
        public int TestId { get; set; }
        [Required]
        [Display(Name = ("Team Name"))]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$", ErrorMessage = ("Invalid Team Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = ("Description"))]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Public/Private Test")]
        public bool Public { get; set; }
        [Required]
        [Display(Name = "Score Order")]
        public bool Reverse { get; set; }
        [Required]
        [Display(Name = "Measurement Type")]
        public string Unit { get; set; }
        [Required]
        [Display(Name = "Lowerest Result")]
        [RegularExpression(@"([0-9]*[.])?[0-9]+")]
        public double LowerResult { get; set; }
        [Required]
        [Display(Name = "Highest Result")]
        [RegularExpression(@"([0-9]*[.])?[0-9]+")]
        public double HigherResult { get; set; }
        [Required]
        [Display(Name = "Lowerest Score")]
        [RegularExpression(@"[0-9]*")]
        public int LowerScore { get; set; }
        [Required]
        [Display(Name = "Highest Score")]
        [RegularExpression(@"[0-9]*")]
        public int HigherScore { get; set; }
        [Required]
        [Display(Name = "Test Category")]
        [Range(1,int.MaxValue, ErrorMessage = "Please select the test category")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Verified Test?")]
        public bool IsVerified { get; set; }
        [Required]
        [Display(Name = "Splittable")]
        public bool IsSplittable { get; set; }
        [Required]
        [Display(Name = "Used As Split")]
        public bool UsedAsSplit { get; set; }

        public Status Status { get; set; } = Status.HasNoRunningActivity;
        public string Category { get; set; }
        public int Tested { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string RowVersionBase64
        {
            get
            {
                return RowVersion == null ? "" : Convert.ToBase64String(RowVersion);
            }
        }
    }
}
