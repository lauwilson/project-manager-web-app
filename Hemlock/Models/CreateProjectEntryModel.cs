using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hemlock.Models
{
    public class CreateProjectEntryModel
    {
        public string ChangeListNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SelectProject { get; set; }

        public string SelectCategory { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int EntryHours { get; set; } = 1;

        public bool Recurrence { get; set; } = false;

        public bool RepeatMonday { get; set; } = false;

        public bool RepeatTuesday { get; set; } = false;

        public bool RepeatWednesday { get; set; } = false;

        public bool RepeatThursday { get; set; } = false;

        public bool RepeatFriday { get; set; } = false;

        public Guid CreatedBy { get; set; }
    }
}