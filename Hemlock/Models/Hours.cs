using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models
{
    public class Hours
    {
        public int BudgetHours { get; set; }

        public int LoggedHours { get; set; }

        public int RemainingHours { get; set; }

        public int PendingCategories { get; set; }
    }
}