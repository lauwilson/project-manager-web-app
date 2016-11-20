using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hemlock.Models
{
    public class ProjectEntry
    {
        [Key]
        public Guid ProjectEntryID { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid ProjectID { get; set; }
        public string ChangeListNo { get; set; }
        public Guid? SREDCategoryID { get; set; }
        public int Hours { get; set; }
        public string Description { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual SREDCategory SREDCategory { get; set; }
        public virtual Project Project { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Employee Employee { get; set; }

    }
}