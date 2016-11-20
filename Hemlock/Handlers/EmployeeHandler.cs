using System.Security.Claims;
using Hemlock.Models;
using Hemlock.Models.Interfaces.Repositories;
using Hemlock.Models.Interfaces;
using System.Web;
using Hemlock.Models.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace Hemlock.Handlers
{
    public class EmployeeHandler
    {
        private ISREDContext _Context;
        private IEmployeeRepository _EmployeeRepository;

        public EmployeeHandler(ISREDContext context, IEmployeeRepository employeeRepository)
        {
            _Context = context;
            _EmployeeRepository = employeeRepository;
        }

        public Employee HandleGetEmployee(ClaimsIdentity identity)
        {
            var email = string.Empty;
            if (HttpContext.Current.Session["User"] == null)
            {
                try
                {
                    email = HandleGetEmployeeLoginEmail(identity);
                }
                catch (InvalidUserException e)
                {
                    Console.WriteLine(e);
                    return null;
                }

                HttpContext.Current.Session["User"] = _EmployeeRepository.GetEmployeeByEmail(email);
            }
            return (Employee) HttpContext.Current.Session["User"];
        }

        private string HandleGetEmployeeLoginEmail(ClaimsIdentity identity)
        {
            var email = identity.FindFirst(ClaimTypes.Email).Value;
            VerifyEmployeeDomain(email);

            return email;
        }
        /* returns a list of all employees */
        public List<Employee> all()
        {
            List<Employee> allEmployees = new List<Employee>();
            var e = _Context.Employees.ToList();
            foreach(var o in e)
            {
                allEmployees.Add(o);
            }
            //sort employees table
            allEmployees= allEmployees.OrderBy(o => o.LastName).ToList();          
            return allEmployees;
        }
        /* returns the number of hours in the given daterange */
        public static double totalHours(DateTime start, DateTime end)
        {
            double totalDays = 0;
            List<DateTime> weekdays = new List<DateTime>();
            while(end >= start)
            {
                if(end.DayOfWeek != DayOfWeek.Saturday 
                    && end.DayOfWeek != DayOfWeek.Sunday)
                {
                    weekdays.Add(end);
                }
                end = end.AddDays(-1);
            }
            totalDays = weekdays.LongCount();
            int hours = 8;

            double result = (totalDays > 0) ? (totalDays * hours)
                : hours;

            return result;
        }
        /* finds all logged hours for ALL employees in the given daterange*/
        public double hours(DateTime fromDate, DateTime toDate)
        {
            double hr = 0;
            double totalHrs = 0;

            foreach (var e in all())
            {
                hr = loggedHours(e, fromDate, toDate);
                totalHrs += hr;
            }
            return totalHrs;
        }
        /* finds looged hours for the given employee in the given daterange */
        public static double loggedHours(Employee e, DateTime from, DateTime to)
        {
            var p = e.ProjectEntries.ToList();
            double logged = 0;
            double sum = 0;

            while (to >= from)
            {
                foreach (var hr in p)
                {
                    var lastUpdate = hr.DateCreated.ToShortDateString();

                    if (to.ToShortDateString() == lastUpdate)
                    {
                        logged = hr.Hours;
                        sum += logged;
                    }     
                }
                to = to.AddDays(-1);
            }
            return sum;
        }
        /* returns a list of values to display in the table on the staffview page */
        public static List<string> staffOneRow(Employee e, DateTime fromDate, DateTime toDate)
        {
            double logged = loggedHours(e, fromDate, toDate);
            double total = totalHours(fromDate, toDate);

            List<string> temp = new List<string>();
            temp.Add(e.FirstName + ' ' + e.LastName);
            temp.Add(e.Position.PositionName);
            temp.Add(logged.ToString());
            temp.Add((total-logged).ToString());
            temp.Add(Math.Round((logged / total) * 100).ToString() + "%");

            return temp;
        }

        private void VerifyEmployeeDomain(string email)
        {
            if (!email.Contains("REDACTED EMAIL"))
            {
                throw new InvalidUserException("Please sign in with your REDACTED EMAIL account.");
            }
        }
    }
}