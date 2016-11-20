using Hemlock.Models;
using System;

namespace HemlockTests.Randomizer
{
    class RandomEmployee
    {
        private Randomizer _random = new Randomizer();
        private static readonly int firstNameLength = 5;
        private static readonly int lastNameLength = 10;
        private static readonly int _daysToAddEnd = 100;
        private static readonly int _daysToAddNotified = -30;
        private static readonly int _permissions = 1;

        public Employee createRandomEmployee()
        {
            var employee = new Employee();

            employee.EmployeeID = Guid.NewGuid();
            employee.Email = _random.RandomEmail();
            employee.FirstName = _random.RandomString(firstNameLength);
            employee.LastName = _random.RandomString(lastNameLength);
            employee.positionID = Guid.NewGuid();
            employee.StartDate = _random.RandomDate();
            employee.EndDate = employee.StartDate.AddDays(_daysToAddEnd);
            employee.Permissions = _permissions;
            employee.LastNotified = DateTime.Now.AddDays(_daysToAddNotified);

            return employee;
        }
    }
}
