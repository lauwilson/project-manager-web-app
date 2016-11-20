using Hemlock.DAL;
using Hemlock.Handlers;
using Hemlock.Models;
using Hemlock.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Hemlock.Controllers
{
    public class UserProfileController : Controller
    {
        private ISREDContext _Context;
        private EmployeeRepository _EmployeeRepository;
        private EmployeeHandler _EmployeeHandler;

        public UserProfileController()
        {
            _Context = new SREDContext();
            _EmployeeRepository = new EmployeeRepository(_Context);
            _EmployeeHandler = new EmployeeHandler(_Context, _EmployeeRepository);
        }
        // GET: UserProfile
        public ActionResult Index()
        {
            // Disable Google OAuth
            // var user = _EmployeeHandler.HandleGetEmployee((ClaimsIdentity)User.Identity);

            // Only check for user in session for demo.
            Employee user = (Employee)Session["User"];
            ViewData["User"] = user.FirstName + " " + user.LastName;

            return View();
        }
    }
}