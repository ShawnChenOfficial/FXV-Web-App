using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FXV.Models
{
    public class Combine
    {
        [Key]
        public int C_ID { get; set; }

        [ConcurrencyCheck]
        [Required]
        public string Name { get; set; }

        [ConcurrencyCheck]
        public string Gender { get; set; }

        [ConcurrencyCheck]
        public string Description { get; set; }

        [ConcurrencyCheck]
        public string Img_Path { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Foreign key

        //Relationships
        ICollection<Combine_Builder> Combine_Builders { get; set; }
        ICollection<Combine_Categories_Weight> Combine_Categories_Weights { get; set; }
        ICollection<Event_Builder> Event_Builders { get; set; }
    }
}
