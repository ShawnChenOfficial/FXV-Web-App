using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FXV.Models
{
    public class SubscriptionPermission
    {
        [Key]
        public int P_ID { get; set; }

        [ConcurrencyCheck]
        public string Permission { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Foreign key

        //Relationships
        ICollection<AppUser> AppUsers { get; set; }
    }
}
