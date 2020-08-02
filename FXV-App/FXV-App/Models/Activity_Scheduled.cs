using System;
using System.ComponentModel.DataAnnotations;
namespace FXV.Models
{
    public class Activity_Scheduled
    {
        [Key]
        public int AS_ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key

        //Relationships
    }
}
