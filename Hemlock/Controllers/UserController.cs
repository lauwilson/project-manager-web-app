using Hemlock.Models;
using Hemlock.Models.Enum;
using Hemlock.Handlers;
using System.Web.Mvc;
using Hemlock.DAL;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;
using System.Security.Claims;

namespace Hemlock.Controllers
{
    public class UserController : Controller
    {
        private EmployeeHandler _employeeHandler;
        private IEmployeeRepository _employeeRepository;
        private ISREDContext _sredContext;

        public UserController()
        {
            _sredContext = new SREDContext();
            _employeeRepository = new EmployeeRepository(_sredContext);
            _employeeHandler = new EmployeeHandler(_sredContext, _employeeRepository);
        }
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl == null)
            {
                returnUrl = "~/User/VerifyUser";
            }

            return RedirectToAction("VerifyUser", "User");
            /* Disable Google OAuth login
            var user = _employeeHandler.HandleGetEmployee((ClaimsIdentity)User.Identity);
            return new ChallengeResult("Google",
              Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = returnUrl }));
            */
        }

        public ActionResult VerifyUser()
        {
            /* Disable Google OAuth login
            var user = _employeeHandler.HandleGetEmployee((ClaimsIdentity)User.Identity);
            */

            // Enable hard-coded user for session for the demo.
            Employee user = _employeeHandler.all().Find(em => em.FirstName.CompareTo("Employee-3") == 0);
            if (user != null)
            {
                Session["User"] = user;
            } else
            {
                return RedirectToAction("Logout");
            }

            if (user.Permissions == (int)PermissionsEnum.Admin)
            {
                return RedirectToAction("Index", "Staff");
            }

            return RedirectToAction("Index", "MyActivity");
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            return new RedirectResult(returnUrl);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            HttpContext.Response.Redirect("https://www.google.com/accounts/Logout?" +
                "continue=https://appengine.google.com/_ah/logout?" +
                "continue=http://localhost:7550/Home/Index");

            return View();
        }
    }
}