using Hemlock.Models.Interfaces.Repositories;
using System;
using System.Linq;
using System.Web;
using Hemlock.Models;
using Hemlock.Models.Interfaces;
using System.Diagnostics;

namespace Hemlock.DAL
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private ISREDContext _Context;
        private bool _Disposed = false;

        public EmployeeRepository(ISREDContext context)
        {
            _Context = context;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee employee;

            try
            {
                employee = _Context.Employees.Where(e => e.Email == email).First();
                if (HttpContext.Current.Session["Name"] == null)
                {
                    HttpContext.Current.Session["Name"] = employee.FirstName + " " + employee.LastName;
                }
                return employee;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No User");
            }

            return null;
        }

        public Employee GetEmployeeByID(Guid id)
        {
            return _Context.Employees.SingleOrDefault(e => e.EmployeeID == id);
        }

        public void AddStaff(Employee e)
        {
            try
            {
                _Context.Employees.Add(e);
                Save();
            }
            catch (Exception)
            {               
                Dispose();
            }
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void UpdateStaff(Employee e)
        {
            _Context.Entry(e).State = System.Data.Entity.EntityState.Modified;
            _Context.SaveChanges();
        }
    }
}