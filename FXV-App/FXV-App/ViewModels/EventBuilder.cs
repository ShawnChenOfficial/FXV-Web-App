using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXV.Models;
using Microsoft.AspNetCore.Http;

namespace FXV.ViewModels
{
    public class EventBuilder
    {
        public int E_ID { get; set; }

        public bool IsPastEvent { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]*$", ErrorMessage = ("Invalid Event Name, Only Accept a-z A-Z 0-9 _ -"))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Event Location")]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Location { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        [Required]
        [Display(Name = "Event Description")]
        [RegularExpression(@"^[a-zA-Z0-9_,.~'\-\s]*$", ErrorMessage = ("Invalid Input, Only Accept a-z A-Z 0-9 _ , . ~ ' -"))]
        public string Description { get; set; }

        [Required(ErrorMessage = "Must have a combine")]
        [Display(Name = "Combine Name")]
        public Combine Combine { get; set; }

        public List<Attendee> Attendees { get; set; } = new List<Attendee>();

        [Display(Name = "Upload Event Image")]
        public IFormFile Image { get; set; }

        public string Image_Path { get; set; }

        public byte[] RowVersion { get; set; }

        public string RowVersionBase64
        {
            get
            {
                return RowVersion == null ? "" : Convert.ToBase64String(RowVersion);
            }
        }
    }

    public class Attendee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
