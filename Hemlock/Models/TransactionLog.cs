using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hemlock.Models
{
    public class TransactionLog
    {
        [Key]
        public Guid TransactionID { get; set; }
        public string ChangeDescription { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid ChangedBy { get; set; }
        public string ErrorMessage { get; set; }
        
        [ForeignKey("ChangedBy")]
        public Employee Employee { get; set; }
    }
}