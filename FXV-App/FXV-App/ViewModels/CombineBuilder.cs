using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class CombineBuilder
    {
        public int C_ID { get; set; }
        [Required]
        [Display(Name = "Combine Name")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$",ErrorMessage =("Invalid Combine Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Gender")]
        [DataType(DataType.MultilineText)]
        public string Gender { get; set; }

        public List<Test> Tests { get; set; } = new List<Test>();

        [Display(Name = "Upload Test Image")]
        public IFormFile Image { get; set; }

        public string Img_Path { get; set; }

        public bool Editable { get; set; }

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
