using Hemlock.DAL;
using Hemlock.Handlers;
using Hemlock.Models;
using Hemlock.Models.Enum;
using Hemlock.Models.Interfaces;
using System.Web.Mvc;
using Hemlock.Controllers.ActionFilters;
using System.Linq;
using Hemlock.Models.Interfaces.Repositories;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using PagedList;

namespace Hemlock.Controllers
{
    public class MyActivityController : Controller
    {
        private ISREDContext _Context;
        private ProjectEntryHandler _projectEntryHandler;
        private IProjectEntryRepository _projectEntryRepository;
        private ProjectEntryTable _projectEntryTableModel;
        private Hours _hours;

        public MyActivityController()
        {
            _Context = new SREDContext();
            _projectEntryRepository = new ProjectEntryRepository(_Context);
            _projectEntryHandler = new ProjectEntryHandler(_Context, _projectEntryRepository);
            _projectEntryTableModel = new ProjectEntryTable();
            _hours = new Hours();
        }

        [Permissions(PermissionsEnum.CanViewOwnActivity)]
        public ActionResult Index(string sortOrder, 
            string currentFilter, 
            string searchString, 
            int? page, 
            int? pageSize, 
            DateTime? startDate, 
            DateTime? endDate,
            string employeeID,
            bool? getPendingCategoryEntries)
        {
            List<ProjectEntry> entries;
            var employee = (Employee)Session["User"];
            if (employeeID == null)
            {
                employeeID = employee.EmployeeID.ToString();
            }

            var parsedEmployeeID = Guid.Parse(employeeID);

            var contextEmployee = _Context.Employees.
                Where(emp => emp.EmployeeID == parsedEmployeeID).
                FirstOrDefault();

            var properStartDate = startDate ?? DateTime.Now.Date;
            var weekStart = properStartDate.AddDays(-(int)properStartDate.DayOfWeek);
            var properEndDate = endDate ?? DateTime.Now;
            properEndDate = new DateTime(properEndDate.Year, 
                properEndDate.Month, 
                properEndDate.Day, 
                23, 59, 59);
            var endWeekStart = properEndDate.AddDays(-(int)properEndDate.DayOfWeek);
            var endOfWeek = endWeekStart.AddDays(6);

            if (getPendingCategoryEntries == true)
            {
                entries = contextEmployee.ProjectEntries.Where(entry =>
                    entry.SREDCategoryID == null).ToList();

            } else if ((searchString != null && 
                searchString != "") || 
                (currentFilter != null && 
                currentFilter != ""))
            {
                entries = contextEmployee.ProjectEntries.ToList();

            } else
            {
                entries = contextEmployee.ProjectEntries.Where(entry =>
                    entry.DateCreated >= weekStart &&
                    entry.DateCreated <= endOfWeek).
                    ToList();
            }

            var projectEntryTable = _projectEntryHandler.HandleGetProjectEntry(entries, 
                sortOrder, 
                currentFilter, 
                searchString, 
                page, 
                pageSize,
                weekStart,
                endOfWeek,
                parsedEmployeeID);
            projectEntryTable.PendingCategories = getPendingCategoryEntries ?? false;

            projectEntryTable.ProjectNames = _projectEntryHandler.GetProjectNameList(_Context);
            projectEntryTable.CategoryNames = _projectEntryHandler.GetCategoryNameList(_Context);
            projectEntryTable.UserName = employee.EmployeeID == contextEmployee.EmployeeID ? "My" : 
                contextEmployee.FirstName + " " + 
                contextEmployee.LastName.Substring(0, 1) + "'s";
            return View(projectEntryTable);
        }

        public JsonResult GetSingleProjectEntry(string id)
        {
            var result = _projectEntryHandler.HandleGetSingleProjectEntry(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Permissions(PermissionsEnum.CanAddEditOwnActivity)]
        public ActionResult Add(ProjectEntryTable projectEntry)
        {
            var newEntry = projectEntry.CreateProjectEntryModel;
            newEntry.CreatedBy = projectEntry.EmployeeID;

            var result = _projectEntryHandler.HandlePostProjectEntry(newEntry);
            _projectEntryRepository.Dispose();

            if (result)
            {
                return RedirectToAction("Index", new
                {
                    sortOrder = projectEntry.CurrentSort,
                    currentFilter = projectEntry.CurrentFilter,
                    searchString = projectEntry.CurrentFilter,
                    page = projectEntry.Page,
                    pageSize = projectEntry.PageSize,
                    startDate = projectEntry.StartDate,
                    endDate = projectEntry.EndDate,
                    employeeID = projectEntry.EmployeeID
                });
            }

            return Redirect("~/Views/Error/BadRequest.cshtml");
        }

        public JsonResult GetCategoriesByProject(Guid projectId)
        {
            return _projectEntryHandler.GetCategoriesByProject(projectId);
        }

        [Permissions(PermissionsEnum.CanAddEditOwnActivity)]
        public ActionResult Edit(ProjectEntryTable updatedEntry)
        {
            var entryToUpdate = updatedEntry.UpdatedProjectEntryModel;
            entryToUpdate.ModifiedBy = updatedEntry.EmployeeID;

            var result = _projectEntryHandler.HandlePatchProjectEntry(entryToUpdate);

            if (result)
            {
                return RedirectToAction("Index", new { sortOrder = updatedEntry.CurrentSort,
                    currentFilter = updatedEntry.CurrentFilter,
                    searchString = updatedEntry.CurrentFilter,
                    page = updatedEntry.Page,
                    pageSize = updatedEntry.PageSize,
                    startDate = updatedEntry.StartDate,
                    endDate = updatedEntry.EndDate,
                    employeeID = updatedEntry.EmployeeID,
                    getPendingCategoryEntries = updatedEntry.PendingCategories
                });
            }

            return Redirect("~/Views/Error/BadRequest.cshtml");
        }

        [Permissions(PermissionsEnum.CanDeleteActivity)]
        public ActionResult Delete(ProjectEntryTable deleteEntry)
        {
            var result = _projectEntryHandler.handleDeleteProjectEntry(deleteEntry.DeleteEntryId);

            if (result)
            {
                return RedirectToAction("Index", new
                {
                    sortOrder = deleteEntry.CurrentSort,
                    currentFilter = deleteEntry.CurrentFilter,
                    searchString = deleteEntry.CurrentFilter,
                    page = deleteEntry.Page,
                    pageSize = deleteEntry.PageSize,
                    startDate = deleteEntry.StartDate,
                    endDate = deleteEntry.EndDate,
                    employeeID = deleteEntry.EmployeeID
                });
            }

            return Redirect("~/Views/Error/BadRequest.cshtml");
        }

        public JsonResult GetHours(string employeeID, string startDate, string endDate)
        {
            var parsedStartDate = startDate != string.Empty && startDate != "undefined" ?
                DateTime.Parse(startDate) : DateTime.Now;
            var parsedEndDate = endDate != string.Empty && endDate != "undefined" ?
                DateTime.Parse(endDate) : DateTime.Now;

            var myHours = new Hours();
            var employee = (Employee)Session["User"];
            if (employeeID == null || employeeID == "undefined")
            {
                employeeID = employee.EmployeeID.ToString();
            }
            var parsedEmployeeID = Guid.Parse(employeeID);

            var baseStartDate = new DateTime(parsedStartDate.Year,
                parsedStartDate.Month,
                parsedStartDate.Day,
                0, 0, 0);
            var startOfWeek = baseStartDate.AddDays(-(int)baseStartDate.DayOfWeek);

            var baseEndDate = new DateTime(parsedEndDate.Year,
                parsedEndDate.Month,
                parsedEndDate.Day,
                0, 0, 0);
            var endWeekStart = baseEndDate.AddDays(-(int)baseEndDate.DayOfWeek);
            var endOfWeek = endWeekStart.AddDays(7);
            var weeks = (int)(endOfWeek - startOfWeek).TotalDays / 7;

            var entries = _Context.ProjectEntries.Where(entry =>
                entry.Employee.EmployeeID == parsedEmployeeID &&
                entry.DateCreated >= startOfWeek &&
                entry.DateCreated <= endOfWeek).ToList();

            myHours.PendingCategories = _Context.ProjectEntries.Where(entry => 
                entry.Employee.EmployeeID == parsedEmployeeID && 
                entry.SREDCategoryID == null).
                Count();
            myHours.LoggedHours = entries.Count != 0
                ? entries.Sum(entry => entry.Hours) : 0;
            myHours.BudgetHours = weeks * 40;
            myHours.RemainingHours = (myHours.BudgetHours - myHours.LoggedHours) < 0 ?
                0 : (myHours.BudgetHours - myHours.LoggedHours);

            return Json(myHours, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHoursForDateRange(string startDate, string endDate, string employeeID)
        {
            startDate = startDate != string.Empty ? startDate : DateTime.Now.ToShortDateString();
            endDate = endDate != string.Empty ? endDate : DateTime.Now.ToShortDateString();

            var myHours = new Hours();
            var parsedEmployeeID = Guid.Parse(employeeID);
            var entries = _Context.ProjectEntries.Where(entry =>
                entry.Employee.EmployeeID == parsedEmployeeID);

            var currentStartDate = Convert.ToDateTime(startDate);
            var baseStartDate = new DateTime(currentStartDate.Year,
                currentStartDate.Month,
                currentStartDate.Day,
                0, 0, 0);
            var startOfWeek = baseStartDate.AddDays(-(int)baseStartDate.DayOfWeek);

            var currentEndDate = Convert.ToDateTime(endDate);
            var baseEndDate = new DateTime(currentEndDate.Year,
                currentEndDate.Month,
                currentEndDate.Day,
                0, 0, 0);
            var endofWeekStart = baseEndDate.AddDays(-(int)baseEndDate.DayOfWeek);
            var endOfWeek = endofWeekStart.AddDays(7).AddSeconds(-1);

            var numberOfDays = (endOfWeek - startOfWeek).TotalDays;
            var numberOfWeeks = Math.Ceiling(numberOfDays / 7);
            var maxHours = numberOfWeeks * 40;
            var loggedHours = entries.Where(entry =>
                entry.DateCreated >= startOfWeek &&
                entry.DateCreated <= endOfWeek).
                AsEnumerable().
                Sum(entry => entry.Hours);

            myHours.RemainingHours = ((int)maxHours - loggedHours) < 0 ? 0 : ((int)maxHours - loggedHours);

            return Json(myHours, JsonRequestBehavior.AllowGet);
        }
    }
}