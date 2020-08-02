using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FXV.Data;
using FXV.Models;

namespace FXV.Models
{
    public class Event_Status
    {
        [Key]
        public int ES_ID { get; set; }
        [DataType(DataType.Text)]
        [ConcurrencyCheck]
        public string Status { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
