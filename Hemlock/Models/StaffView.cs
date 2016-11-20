using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hemlock.Models
{/* created by Luda Shu */
    public class StaffView
    {
        public DateTime to { get; set; }
        public DateTime from { get; set; }
        public double budget { get; set; }
        public double hours { get; set; }
        public double remaining { get; set; }
        public string toString { get; set; }
        public string fromString { get; set; }
        public string email { get; set; }
        public string pageSize { get; set; }
        public PagedList<Employee> list { get; set; }

        public IEnumerable<SelectListItem> ListOfPositions { get; set; }
        public Employee newEmployee { get; set; }
        public DateTime now { get; set; }
    }
}