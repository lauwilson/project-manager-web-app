﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hemlock.DAL;
using Hemlock.Handlers;
using System.Security.Claims;
using Hemlock.Models;
using Hemlock.Models.Enum;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;

namespace Hemlock.Controllers
{
    public class ProjectsController : Controller
    {
        private ISREDContext _Context;
        private IEmployeeRepository _EmployeeRepository;
        private IProjectRepository _ProjectRepository;
        private ISREDCategoryRepository _SREDCategoryRepository;
        private EmployeeHandler _EmployeeHandler;
        private ProjectsViewModelHandler _ProjectsViewModelHandler;

        public ProjectsController()
        {
            _Context = new SREDContext();
            _EmployeeRepository = new EmployeeRepository(_Context);
            _ProjectRepository = new ProjectRepository(_Context);
            _SREDCategoryRepository = new SREDCategoryRepository(_Context);
            _EmployeeHandler = new EmployeeHandler(_Context, _EmployeeRepository);
        }

        // GET: Projects
        public ActionResult Index(string projectName, string fromDate, string toDate)
        {
            _ProjectsViewModelHandler = new ProjectsViewModelHandler();
            ProjectsViewModel projectsViewModel = new ProjectsViewModel();

            _ProjectsViewModelHandler.SetViewModelDates(ref projectsViewModel, fromDate, toDate);
            projectsViewModel.Projects = _ProjectRepository.GetProjects().OrderBy(p => p.ProjectName);
            projectsViewModel.SelectedProject = _ProjectRepository.GetProjectByName(projectName) ?? projectsViewModel.Projects.First();

            Dictionary<SREDCategory, Dictionary<Employee, float>> EmployeePercentageContributionPerCategory_Float = new Dictionary<SREDCategory, Dictionary<Employee, float>>();
            _ProjectsViewModelHandler.CalculatePercentageContributionPerCategory(ref projectsViewModel,
                                                                                 ref EmployeePercentageContributionPerCategory_Float,
                                                                                 DateTime.Parse(projectsViewModel.FromDateString),
                                                                                 DateTime.Parse(projectsViewModel.ToDateString));

            _ProjectsViewModelHandler.ConvertPercentageContributionsToString(ref projectsViewModel, EmployeePercentageContributionPerCategory_Float);

            projectsViewModel.ListOfManagers = _EmployeeHandler.all()
                .Where(e => (e.Permissions & (int) PermissionsEnum.ManageProjects) == (int) PermissionsEnum.ManageProjects)
                .Select(e => new SelectListItem { Value = e.EmployeeID.ToString(), Text = e.FullName });


            // Disable Google OAuth
            // var user = _EmployeeHandler.HandleGetEmployee((ClaimsIdentity)User.Identity);

            // Only check for user in session for demo.
            Employee user = (Employee)Session["User"];

            ViewData["User"] = user.FirstName + " " + user.LastName;
            return View(projectsViewModel);
        }

        // POST: Project
        [HttpPost]
        public ActionResult AddProject(ProjectsViewModel projectsModel)
        {
            projectsModel.NewProject.ProjectID = Guid.NewGuid();
            projectsModel.NewProject.ProjectCreatorID = ((Employee)HttpContext.Session["User"]).EmployeeID;
            projectsModel.NewProject.CreatedDate = DateTime.Now;
            projectsModel.NewProject.LastModifiedDate = DateTime.Now;

            _ProjectRepository.InsertProject(projectsModel.NewProject);
            return RedirectToAction("Index", new { projectName = projectsModel.SelectedProject.ProjectName,
                                                    toDate = projectsModel.ToDateString,
                                                    fromDate = projectsModel.FromDateString } );
        }

        [HttpPost]
        public ActionResult AddCategory(ProjectsViewModel projectsModel)
        {
            projectsModel.NewCategory.SREDCategoryID = Guid.NewGuid();
            projectsModel.NewCategory.ProjectID = Guid.Parse(projectsModel.NewCategoryProjectID);

            _SREDCategoryRepository.InsertSREDCategory(projectsModel.NewCategory);
            return RedirectToAction("Index", new { projectName = projectsModel.SelectedProject.ProjectName,
                                                    toDate = projectsModel.ToDateString,
                                                    fromDate = projectsModel.FromDateString } );
        }

        public ActionResult EditCategory(ProjectsViewModel projectsModel)
        {
            var categoryToUpdate = projectsModel.UpdatedCategory;
            var existingCategory = _Context.SREDCategories.Where(category => category.SREDCategoryID == categoryToUpdate.SREDCategoryID).SingleOrDefault();
            existingCategory.CategoryName = categoryToUpdate.CategoryName;
            _Context.Entry(existingCategory).State = System.Data.Entity.EntityState.Modified;
            _Context.SaveChanges();

            return RedirectToAction("Index", new { projectName = projectsModel.SelectedProject.ProjectName,
                                                    toDate = projectsModel.ToDateString,
                                                    fromDate = projectsModel.FromDateString } );
        }
    }
}