using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hemlock.Models
{
    public class Project
    {
        [Key]
        public Guid ProjectID { get; set; }
        public Guid ProjectManagerID { get; set; }
        public string ProjectName { get; set; }
        public string PicturePath { get; set; }
        public Guid ProjectCreatorID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<ProjectEntry> ProjectEntries { get; set; }
        public virtual ICollection<SREDCategory> SREDCategories { get; set; }
    }
}