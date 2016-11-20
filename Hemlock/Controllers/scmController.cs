using System;
using Hemlock.Models;
using Hemlock.Models.Interfaces;
using Hemlock.Handlers;
using Hemlock.Models.Interfaces.Repositories;
using Hemlock.DAL;
using System.Linq;
using System.Web.Mvc;

namespace Hemlock.Controllers
{
    public class scmController : Controller
    {
        private ISREDContext _context;
        private ProjectEntryHandler _projectEntryHandler;
        private IProjectEntryRepository _projectEntryRepository;

        public scmController()
        {
            _context = new SREDContext();
            _projectEntryRepository = new ProjectEntryRepository(_context);
            _projectEntryHandler = new ProjectEntryHandler(_context, _projectEntryRepository);
        }

        public void perforceEntries(string commit, string project, string user, string timestamp, int hours, string description)
        {
            var parsedCommit = commit.Length > 8 ? commit.Substring(0, 8) : commit;
            var employees = _context.Employees.ToList();
            var contextUser = employees.Where(employee =>
                employee.Email.ToLower() == user.Trim().ToLower()).
                FirstOrDefault();
            var projectObject = _context.Projects.Where(proj => 
                proj.ProjectName == project).
                FirstOrDefault();

            var newEntry = new CreateProjectEntryModel();
            newEntry.ChangeListNo = parsedCommit;
            newEntry.SelectProject = projectObject.ProjectID.ToString();
            newEntry.CreatedBy = contextUser.EmployeeID;
            newEntry.StartDate = DateTime.Parse(timestamp);
            newEntry.EntryHours = hours;
            newEntry.Description = description;

            _projectEntryHandler.HandlePostProjectEntry(newEntry, true);
            _projectEntryRepository.Dispose();
        }
    }
}
