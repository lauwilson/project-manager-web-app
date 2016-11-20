using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hemlock.DAL;
using Hemlock.Models;
using Hemlock.Models.Enum;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;

namespace Hemlock.Controllers
{
    public class EmployeeController : Controller
    {
        private ISREDContext _Context;
        private IEmployeeRepository _EmployeeRepository;

        public EmployeeController()
        {
            _Context = new SREDContext();
            _EmployeeRepository = new EmployeeRepository(_Context);
        }

        // GET: Employee/Edit
        public ActionResult Edit(EditEmployeeViewModel employeeViewModel)
        {
            var updatedEmployee = employeeViewModel.Employee;
            var existingEmployee = _EmployeeRepository.GetEmployeeByID(employeeViewModel.Employee.EmployeeID);
            if (existingEmployee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.positionID = updatedEmployee.positionID;
            existingEmployee.StartDate = updatedEmployee.StartDate;
            existingEmployee.EndDate = updatedEmployee.EndDate;
            existingEmployee.Permissions = (employeeViewModel.HasAdminPrivileges ? (int)PermissionsEnum.Admin : (int)PermissionsEnum.User);

            _EmployeeRepository.UpdateStaff(existingEmployee);
            return RedirectToAction("Index", "Staff");
        }
    }
}
