using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hemlock.Models
{
    public class Position
    {
        [Key]
        public Guid PositionID { get; set; }
        public string PositionName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}