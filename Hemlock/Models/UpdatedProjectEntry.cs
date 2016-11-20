using System;
using System.ComponentModel.DataAnnotations;

namespace Hemlock.Models
{
    public class UpdatedProjectEntry
    {
        public Guid ModifiedBy { get; set; }

        public DateTime Date { get; set; }

        public string ProjectEntryID { get; set; }

        public string ProjectID { get; set; }

        public string SREDCategoryID { get; set; }

        public int Hours { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}