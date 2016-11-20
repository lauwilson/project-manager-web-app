using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hemlock.Models
{
    public class ProjectEntryTable
    {
        public UpdatedProjectEntry UpdatedProjectEntryModel { get; set; }
        public CreateProjectEntryModel CreateProjectEntryModel { get; set; }
        public Guid DeleteEntryId { get; set; }
        public Guid EmployeeID { get; set; }
        public IPagedList<ProjectEntry> Entries { get; set; }
        public string CurrentSort { get; set; }
        public string ChangeListNoSortParam { get; set; }
        public string ProjectSortParam { get; set; }
        public string CategorySortParm { get; set; }
        public string HoursSortParm { get; set; }
        public string DateSortParm { get; set; }
        public string CurrentFilter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string startString { get; set; }
        public string endString { get; set; }
        public string UserName { get; set; }
        public bool PendingCategories { get; set; }

        public SelectList PageSizeSelect = new SelectList(PageSizeDict, "Key", "Value");

        private static Dictionary<string, int> PageSizeDict = new Dictionary<string, int>
        {
            { "20", 20 },
            { "50", 50 },
            { "100", 100 }
        };

        public string SelectedProject { get; set; }
        public SelectList ProjectNames { get; set; }

        public string SelectedCategory { get; set; }
        public SelectList CategoryNames { get; set; }

        private int _defaultPageSize = 25;

        public ProjectEntryTable()
        {

        }

        public ProjectEntryTable(string sortOrder, int? pageSize)
        {
            CurrentSort = sortOrder;
            ChangeListNoSortParam = sortOrder == "changeListNo" ? "changeListNo_desc" : "changeListNo";
            ProjectSortParam = sortOrder == "project" ? "project_desc" : "project";
            CategorySortParm = (string.IsNullOrEmpty(sortOrder) | sortOrder == "category") ?
                "category_desc" : "category";
            HoursSortParm = sortOrder == "hours" ? "hours_desc" : "hours";
            DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            PageSize = pageSize ?? _defaultPageSize;
        }
    }
}