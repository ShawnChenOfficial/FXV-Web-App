using System;
using System.Collections.Generic;
using FXV.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FXV.ViewModels
{
    public class TestBuilder
    {
        public int Test_ID { get; set; }
        [Required]
        [Display(Name = "Test Name")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$", ErrorMessage = ("Invalid Test Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Measurement Type")]
        public string Measurement_Type { get; set; }
        [Required]
        [Display(Name = "Visible/Hidden")]
        public bool Visible { get; set; }

        [Display(Name = "Reverse Order")]
        public bool Reverse { get; set; }

        [Required]
        [Display(Name = "Test Category")]
        public int CategoryId { get; set; }
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

        public IList<Organization> Organizations { get; set; }
        public IList<Team> Teams { get; set; }

        [Display(Name ="Upload Test Image")]
        public IFormFile Image { get; set; }

        public string Img_Path { get; set; }

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
