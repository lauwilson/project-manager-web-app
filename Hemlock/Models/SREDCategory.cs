using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hemlock.Models
{
    public class SREDCategory
    {
        [Key]
        public Guid SREDCategoryID { get; set; }
        public Guid ProjectID { get; set; }
        public string CategoryName { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectEntry> ProjectEntries { get; set; }
    }
}