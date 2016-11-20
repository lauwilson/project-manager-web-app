using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace Hemlock.Models
{
    public class ProjectsViewModel
    {
        public Project SelectedProject { get; set; }
        public IEnumerable<Project> Projects { get; set; }

        public Dictionary<SREDCategory, Dictionary<Employee, int>> EmployeeHoursWorkedPerCategory;
        public Dictionary<SREDCategory, int> TotalHoursPerCategory;

        public Dictionary<SREDCategory, Dictionary<Employee, string>> EmployeePercentageContributionPerCategory_String;
        public Dictionary<SREDCategory, string> TotalPercentageOfContributionPerCategory_String;
        public int TotalLoggedHours;

        public Project NewProject { get; set; }
        public SREDCategory NewCategory { get; set; }
        public string NewCategoryProjectID { get; set; }

        public SREDCategory UpdatedCategory { get; set; }

        public IEnumerable<SREDCategory> ListOfCategories { get; set; }

        public IEnumerable<SelectListItem> ListOfManagers { get; set; }

        public string FromDateString { get; set; }
        public string ToDateString { get; set; }

        public string CurrentFilter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }

        public ProjectsViewModel()
        {
            EmployeeHoursWorkedPerCategory = new Dictionary<SREDCategory, Dictionary<Employee, int>>();
            TotalHoursPerCategory = new Dictionary<SREDCategory, int>();
            EmployeePercentageContributionPerCategory_String = new Dictionary<SREDCategory, Dictionary<Employee, string>>();
            TotalPercentageOfContributionPerCategory_String = new Dictionary<SREDCategory, string>();
            TotalLoggedHours = 0;
        }
    }
}