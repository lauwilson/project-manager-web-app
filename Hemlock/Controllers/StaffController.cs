using System;
using System.Linq;
using System.Web.Mvc;
using Hemlock.DAL;
using Hemlock.Handlers;
using Hemlock.Models;
using Hemlock.Models.Interfaces;
using System.Security.Claims;
using Hemlock.Controllers.ActionFilters;
using Hemlock.Models.Enum;
using System.Diagnostics;
using Hemlock.Models.Interfaces.Repositories;
using PagedList;
using System.Net.Mail;
using System.Net;

namespace Hemlock.Controllers
{/* created by Luda Shu */
    public class StaffController : Controller
    {
        private ISREDContext _Context;
        private IProjectEntryRepository _projectEntryRepository;
        private EmployeeRepository _EmployeeRepository;
        private EmployeeHandler _EmployeeHandler;
        private ProjectEntryHandler _projectEntryHandler;
        private StaffView _staffView;

        private FakeEmployee _fakeEmployee;

        public StaffController()
        {
            _Context = new SREDContext();
            _EmployeeRepository = new EmployeeRepository(_Context);
            _EmployeeHandler = new EmployeeHandler(_Context, _EmployeeRepository);
            _projectEntryHandler = new ProjectEntryHandler(_Context,_projectEntryRepository);
            _staffView = new StaffView();
            _fakeEmployee = new FakeEmployee();
        }
        /* goes to the selected user's activity page */
        public new ActionResult View(string username)
        {
            var employees = _Context.Employees.ToList();
            foreach (var e in employees)
            {
                if ((e.FirstName + e.LastName) == username)
                {                                    
                    return RedirectToAction("Index", "MyActivity", new { employeeID = e.EmployeeID });
                }
            }
            return RedirectToAction("NotFound", "Error");
        }
        /* sends email to the selected user */
        public ActionResult FlagUser(string username, string subject, string message, string cc)
        {
            var e = _Context.Employees.ToList();
            string ccAddress = (cc != null) ? cc : "";

            foreach(var o in e)
            {
                if (username == (o.FirstName + o.LastName))
                {
                    // Disable mail functionality for demo purposes.
                    /*
                    MailAddress from = new MailAddress("REDACTED EMAIL", "SR&ED Notification");
                    MailAddress to = new MailAddress(o.Email, o.FullName);
                    MailMessage mail = new MailMessage(from, to);
                    MailAddress copy = new MailAddress(ccAddress);
                    mail.CC.Add(copy);

                    //turn on google.com/settings/security/lesssecureapps
                    //enable IMAP
                    SmtpClient client = new SmtpClient();
                    client.Port = 587; //or 465
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("REDACTED EMAIL", "REDACTED PASSWORD");
                    mail.Subject = subject;
                    mail.Body = message;
                        try
                        {
                            client.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception caught in FlagUser(): {0} " 
                                + ex.ToString());
                        }
                    */
                    return RedirectToAction("Index", "Staff", new { username = o.Email } );
                }
            }
            return RedirectToAction("NotFound", "Error");
        }
        /* goes to selected user's profile page with option to edit */
        public ActionResult Edit(Guid EmployeeID)
        {
            var employee = _Context.Employees.SingleOrDefault(e => e.EmployeeID == EmployeeID);
            if (employee == null)
            {
                return RedirectToAction("BadRequest", "Error");
            }

            var editEmployeeViewModel = new EditEmployeeViewModel();
            editEmployeeViewModel.Employee = employee;
            editEmployeeViewModel.ListOfPositions = _Context.Positions.Select(p => new SelectListItem { Value = p.PositionID.ToString(), Text = p.PositionName });
            editEmployeeViewModel.HasAdminPrivileges = (employee.Permissions % (int)PermissionsEnum.Admin) == 0;
            ViewData["name"] = User;
            return View(editEmployeeViewModel);
        }
        /* addes employee into database */
        [HttpPost]
        public ActionResult Add(StaffView newStaff)
        {
            newStaff.newEmployee.EmployeeID = Guid.NewGuid();
            _EmployeeRepository.AddStaff(newStaff.newEmployee);

            return RedirectToAction("Index");
        }
        [Permissions(PermissionsEnum.ManageStaff)]
        public ActionResult Index(string from, string to, string pageSize, int page=1)
        {
            //handle hours
            DateTime now = DateTime.Now;
            DateTime weekStart = now.AddDays(-(int)now.DayOfWeek);
            DateTime fromDate = ((from == null) || (from == "")) ? weekStart : Convert.ToDateTime(from);
            DateTime toDate = ((to == null) || (to == "")) ? weekStart.AddDays(6) : Convert.ToDateTime(to);

            var budget = EmployeeHandler.totalHours(fromDate, toDate) *
                 _EmployeeHandler.all().LongCount();
            var hours = _EmployeeHandler.hours(fromDate, toDate);

            // Disable Google OAuth
            // var user = _EmployeeHandler.HandleGetEmployee((ClaimsIdentity)User.Identity);

            // Only check for user in session for demo.
            Employee user = (Employee)Session["User"];
            ;
            ViewData["User"] = user.FirstName + " " + user.LastName;
            
            //insert values into model
            _staffView.budget = budget;
            _staffView.hours = hours;
            _staffView.remaining = budget-hours;
            _staffView.from = fromDate;
            _staffView.to = toDate;
            _staffView.fromString = fromDate.ToString(@"MM\/dd\/yyyy");
            _staffView.toString = toDate.ToString(@"MM\/dd\/yyyy");
            _staffView.email = user.Email;
            _staffView.now = now;

            var staff = _EmployeeHandler.all().ToList();
            //handle pagination
            int changeSize = 0;
            bool isNum = Int32.TryParse(pageSize, out changeSize);
            int size = 20; //default no. of employees per page
            if(isNum && changeSize > 0) { size = changeSize; }
            _staffView.pageSize = size.ToString();

            _staffView.list = new PagedList<Employee>(staff, page, size);
            _staffView.ListOfPositions = _Context.Positions.Select(p => new SelectListItem
            { Value = p.PositionID.ToString(), Text = p.PositionName });

            return View(_staffView);
        }
    }
}