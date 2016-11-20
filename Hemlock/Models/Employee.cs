using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Hemlock.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeID { get; set; }
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
        public Guid positionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Permissions { get; set; }
        public DateTime? LastNotified { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<ProjectEntry> ProjectEntries { get; set; }
    }
}