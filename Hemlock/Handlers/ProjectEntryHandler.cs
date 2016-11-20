using System;
using System.Collections.Generic;
using System.Linq;
using Hemlock.Models;
using Hemlock.Models.Enum;
using PagedList;
using System.Web.Mvc;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Hemlock.Handlers
{
    public class ProjectEntryHandler
    {
        private readonly string _defaultSort = "date_desc";
        private readonly int _maxCharacters = 8;
        private readonly int _maxWeekHours = 40;
        private ISREDContext _context;
        private IProjectEntryRepository _projectEntryRepository;
        private ProjectEntryTable _projectEntryTable;

        public ProjectEntryHandler(ISREDContext context, IProjectEntryRepository projectEntryRepository)
        {
            _context = context;
            _projectEntryRepository = projectEntryRepository;
        }

        public ProjectEntryTable HandleGetProjectEntry(IList<ProjectEntry> entries, 
            string sortOrder, 
            string currentFilter, 
            string searchString, 
            int? page, 
            int? pageSize,
            DateTime startDate,
            DateTime endDate,
            Guid EmployeeID)
        {
            _projectEntryTable = new ProjectEntryTable(sortOrder, pageSize);
            _projectEntryTable.StartDate = startDate;
            _projectEntryTable.EndDate = endDate;
            _projectEntryTable.EmployeeID = EmployeeID;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            _projectEntryTable.CurrentFilter = searchString;

            var searchedEntries = SearchProjectEntries(searchString, entries);
            var sortedEntries = SortEntries(sortOrder, searchedEntries);
            _projectEntryTable.startString = startDate.ToString(@"MM\/dd\/yyyy");
            _projectEntryTable.endString = endDate.ToString(@"MM\/dd\/yyyy");

            var pageNumber = page ?? 1;

            _projectEntryTable.Entries = sortedEntries.ToPagedList(pageNumber, _projectEntryTable.PageSize);

            return _projectEntryTable;
        }

        public UpdatedProjectEntry HandleGetSingleProjectEntry(string id)
        {
            var existingEntry = _context.ProjectEntries.Where(entry =>
                entry.ProjectEntryID.ToString() == id).
                SingleOrDefault();

            return CreateEditableProjectEntry(existingEntry);
        }

        private UpdatedProjectEntry CreateEditableProjectEntry(ProjectEntry existingEntry)
        {
            var entry = new UpdatedProjectEntry();
            entry.Date = existingEntry.DateCreated;
            entry.ProjectEntryID = existingEntry.ProjectEntryID.ToString();
            entry.Hours = existingEntry.Hours;
            entry.ProjectID = existingEntry.ProjectID.ToString();
            entry.SREDCategoryID = existingEntry.SREDCategoryID.ToString();
            entry.Description = existingEntry.Description;

            return entry;
        }

        public IList<ProjectEntry> SortEntries(string sortOrder, IList<ProjectEntry> entries)
        {
            var enumSortValue = ToSortEnum(sortOrder);
            var sortedEntries = sortProjectEntries(enumSortValue, entries);

            return sortedEntries;
        }

        private PermissionSortOrderEnum ToSortEnum(string sortOrder)
        {
            sortOrder = sortOrder == null ? _defaultSort : sortOrder;
            PermissionSortOrderEnum permissionEnum;

            return Enum.TryParse(sortOrder, out permissionEnum) ?
                (PermissionSortOrderEnum)Enum.Parse(typeof(PermissionSortOrderEnum), sortOrder) :
                (PermissionSortOrderEnum)Enum.Parse(typeof(PermissionSortOrderEnum), _defaultSort);
        }

        private IList<ProjectEntry> sortProjectEntries(PermissionSortOrderEnum enumSortValue, IList<ProjectEntry> entries)
        {
            switch (enumSortValue)
            {
                case PermissionSortOrderEnum.changeListNo:
                    return entries.OrderBy(entry => entry.ChangeListNo).ToList();
                case PermissionSortOrderEnum.changeListNo_desc:
                    return entries.OrderByDescending(entry => entry.ChangeListNo).ToList();
                case PermissionSortOrderEnum.project:
                    return entries.OrderBy(entry => entry.Project.ProjectName).ToList();
                case PermissionSortOrderEnum.project_desc:
                    return entries.OrderByDescending(entry => entry.Project.ProjectName).ToList();
                case PermissionSortOrderEnum.category:
                    return entries.OrderBy(entry =>
                        (entry.SREDCategory != null && entry.SREDCategory.CategoryName != null) ?
                        entry.SREDCategory.CategoryName : string.Empty).
                        ToList();
                case PermissionSortOrderEnum.category_desc:
                    return entries.OrderByDescending(entry => 
                        (entry.SREDCategory != null && entry.SREDCategory.CategoryName != null) ? 
                        entry.SREDCategory.CategoryName : "ZZZ").
                        ToList();
                case PermissionSortOrderEnum.hours:
                    return entries.OrderBy(entry => entry.Hours).ToList();
                case PermissionSortOrderEnum.hours_desc:
                    return entries.OrderByDescending(entry => entry.Hours).ToList();
                case PermissionSortOrderEnum.date_desc:
                    return entries.OrderByDescending(entry => entry.DateCreated).ToList();
                default:
                    return entries.OrderBy(entry => entry.DateCreated).ToList(); 
            }
        }

        public IList<ProjectEntry> SearchProjectEntries(string searchString, IList<ProjectEntry> entries)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return entries;
            } else
            {
                var trimmedSearchString = searchString.Trim().ToLower();
                return entries.Where(entry =>
                    (entry.Description != null && entry.Description.ToLower().Contains(trimmedSearchString)) ||
                    entry.ChangeListNo.ToLower().Substring(0, _maxCharacters).Contains(trimmedSearchString) ||
                    entry.Project.ProjectName.ToLower().Contains(trimmedSearchString) ||
                    entry.SREDCategoryID != null && entry.SREDCategory.CategoryName.ToLower().Contains(trimmedSearchString)).
                    ToList();
            }
        }

        public SelectList GetProjectNameList(ISREDContext context)
        {
            var projectNames = context.Projects.Select(project =>
                new SelectListItem
                {
                    Text = project.ProjectName,
                    Value = project.ProjectID.ToString()
                }).
                ToList();

            projectNames = projectNames.OrderBy(project => project.Text).ToList();

            return new SelectList(projectNames, "Value", "Text");
        }

        public SelectList GetCategoryNameList(ISREDContext context)
        {
            var categoryNames = context.SREDCategories.Select(category =>
                new SelectListItem
                {
                    Text = category.CategoryName,
                    Value = category.SREDCategoryID.ToString()
                }).
                ToList();

            categoryNames = categoryNames.OrderBy(category => category.Text).ToList();

            return new SelectList(categoryNames, "Value", "Text");
        }

        public JsonResult GetCategoriesByProject(Guid projectId)
        {
            var results = _context.SREDCategories.
                Where(c => projectId == c.ProjectID).
                Select(category => new {
                    CategoryId = category.SREDCategoryID,
                    Category = category.CategoryName
                });

            var sortedResults = results.OrderBy(category => category.Category);

            return new JsonResult
            {
                Data = sortedResults,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public bool HandlePostProjectEntry(CreateProjectEntryModel newEntry, bool? externalEntry = false)
        {
            bool result = true;
            ProjectEntry DbProjectEntry;

            if (externalEntry == true)
            {
                DbProjectEntry = CreateProjectEntry(newEntry.StartDate, newEntry);
                return _projectEntryRepository.PostProjectEntry(DbProjectEntry);
            }

            if (!newEntry.Recurrence)
            {
                return CreateEntriesWithSplitHours(newEntry);
            }

            var startDate = newEntry.StartDate;
            var endDate = newEntry.EndDate;

            var days = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            if (days.Count > 1)
            {
                if (newEntry.RepeatMonday)
                {
                    result = RepeatPostProjectEntry(days, 
                        DayOfWeek.Monday, 
                        newEntry);
                }

                if (newEntry.RepeatTuesday)
                {
                    result = RepeatPostProjectEntry(days, 
                        DayOfWeek.Tuesday, 
                        newEntry);
                }

                if (newEntry.RepeatWednesday)
                {
                    result = RepeatPostProjectEntry(days, 
                        DayOfWeek.Wednesday, 
                        newEntry);
                }

                if (newEntry.RepeatThursday)
                {
                    result = RepeatPostProjectEntry(days, 
                        DayOfWeek.Thursday, 
                        newEntry);
                }

                if (newEntry.RepeatFriday)
                {
                    result = RepeatPostProjectEntry(days, 
                        DayOfWeek.Friday, 
                        newEntry);
                }
            } else
            {
                if (checkHoursForWeek(newEntry.StartDate, newEntry))
                {
                    DbProjectEntry = CreateProjectEntry(newEntry.StartDate.Add(DateTime.Now.TimeOfDay), newEntry);
                    result = _projectEntryRepository.PostProjectEntry(DbProjectEntry);
                }
            }

            return result;
        }

        private bool CreateEntriesWithSplitHours(CreateProjectEntryModel newEntry)
        {
            var result = false;
            var startDate = newEntry.StartDate;
            var endDate = newEntry.EndDate;

            var days = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            var hoursPerDay = newEntry.EntryHours / days.Count;
            var remainderHours = newEntry.EntryHours - (hoursPerDay * days.Count);

            foreach (var day in days) {
                var paddedHoursPerDay = 0;

                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                {
                    days.Remove(day);
                }

                if (remainderHours != 0)
                {
                    paddedHoursPerDay = hoursPerDay + 1;
                    remainderHours--;
                }
                else
                {
                    paddedHoursPerDay = hoursPerDay;
                }

                newEntry.EntryHours = paddedHoursPerDay;

                if (newEntry.EntryHours != 0)
                {
                    var DbProjectEntry = CreateProjectEntry(day, newEntry);
                    if (DbProjectEntry.DateCreated == DateTime.Today.Date)
                    {
                        DbProjectEntry.DateCreated = DbProjectEntry.
                            DateCreated.Add(DateTime.Now.TimeOfDay);
                    }

                    result = _projectEntryRepository.PostProjectEntry(DbProjectEntry);
                }
            }

            return result;
        }

        private bool RepeatPostProjectEntry(List<DateTime> days, 
            DayOfWeek dayOfWeek, 
            CreateProjectEntryModel newEntry)
        {
            ProjectEntry DbEntry;
            var specificDays = days.Where(d => 
                d.DayOfWeek.ToString() == dayOfWeek.ToString());

            foreach (DateTime day in specificDays)
            {
                if (checkHoursForWeek(day, newEntry))
                {
                    DbEntry = CreateProjectEntry(day, newEntry);
                    if (!_projectEntryRepository.PostProjectEntry(DbEntry))
                    {
                        return false;
                    }
                }
                
            }

            return true;
        }

        private bool checkHoursForWeek(DateTime day, CreateProjectEntryModel newEntry)
        {
            var entries = _context.ProjectEntries.Where(entry =>
                entry.Employee.EmployeeID == newEntry.CreatedBy);

            var baseDate = new DateTime(day.Year,
                day.Month,
                day.Day,
                0, 0, 0);
            var startOfWeek = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);

            var loggedHours = entries.Where(entry =>
                entry.DateCreated >= startOfWeek &&
                entry.DateCreated <= endOfWeek).
                AsEnumerable().
                Sum(entry => entry.Hours);

            return (loggedHours + newEntry.EntryHours) <= _maxWeekHours;
        }

        private ProjectEntry CreateProjectEntry(DateTime day, CreateProjectEntryModel newEntry)
        {
            var newDbEntry = new ProjectEntry();

            newDbEntry.ProjectEntryID = Guid.NewGuid();
            newDbEntry.CreatedBy = newEntry.CreatedBy;
            newDbEntry.DateCreated = day;
            newDbEntry.ProjectID = Guid.Parse(newEntry.SelectProject);
            newDbEntry.ChangeListNo = newEntry.ChangeListNo ?? 
                newDbEntry.ProjectEntryID.ToString().Substring(0, _maxCharacters);
            newDbEntry.Hours = newEntry.EntryHours;
            newDbEntry.Description = newEntry.Description ?? string.Empty;
            newDbEntry.ModifiedBy = newEntry.CreatedBy;
            newDbEntry.ModifiedDate = day;
            newDbEntry.SREDCategoryID = newEntry.SelectCategory == null ? 
                null : (Guid?)Guid.Parse(newEntry.SelectCategory);

            return newDbEntry;   
        }

        public bool HandlePatchProjectEntry(UpdatedProjectEntry entryToUpdate)
        {
            var existingEntry = _context.ProjectEntries.Where(entry =>
                entry.ProjectEntryID.ToString() == entryToUpdate.ProjectEntryID).
                SingleOrDefault();

            var updatedEntry = UpdateProjectEntry(existingEntry, entryToUpdate);

            return  _projectEntryRepository.PatchProjectEntry(updatedEntry);
        }

        private ProjectEntry UpdateProjectEntry(ProjectEntry existingEntry, UpdatedProjectEntry updatedEntry)
        {
            existingEntry.ProjectID = Guid.Parse(updatedEntry.ProjectID);
            existingEntry.SREDCategoryID = Guid.Parse(updatedEntry.SREDCategoryID);
            existingEntry.Description = updatedEntry.Description;
            existingEntry.Hours = updatedEntry.Hours;
            existingEntry.ModifiedBy = updatedEntry.ModifiedBy;
            existingEntry.ModifiedDate = DateTime.Now;

            return existingEntry;
        }

        public bool handleDeleteProjectEntry(Guid id)
        {
            var entryToDelete = _context.ProjectEntries.Where(entry => 
                entry.ProjectEntryID == id).
                FirstOrDefault();


            return _projectEntryRepository.DeleteProjectEntry(entryToDelete);
        }
    }
}