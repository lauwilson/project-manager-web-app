using Hemlock.Models;
using Hemlock.Models.Interfaces;
using HemlockTests.Randomizer;
using NUnit.Framework;

namespace HemlockTests
{
    [TestFixture]
    public class FakeEmployeeTest
    {
        private ISREDContext _context = new FakeSREDContext();
        private RandomEmployee _randomEmployee = new RandomEmployee();

        [SetUp]
        public void Initialize()
        {
            _context.Employees = new FakeEmployee
            {
                _randomEmployee.createRandomEmployee(),
                _randomEmployee.createRandomEmployee(),
                _randomEmployee.createRandomEmployee(),
                _randomEmployee.createRandomEmployee(),
                _randomEmployee.createRandomEmployee()
            };
        }

        [Test]
        public void CanCreateInstance()
        {
            var result = _context.Employees.Create();

            Assert.IsInstanceOf<Employee>(result);
        }

        [Test]
        public void ElementType_ShouldBeOfTypeEmployee()
        {
            var result = _context.Employees.ElementType;

            Assert.That(result.Name, Is.EqualTo("Employee"));
        }

        [Test]
        public void EmployeesDataSetShouldNotBeEmptyOrNull()
        {
            Assert.That(_context.Employees, Is.Not.Null);
        }
        
        [Test]
        public void ShouldBeAbleToAddNewEmployeeToDataSet()
        {
            var newEmployee = _randomEmployee.createRandomEmployee();

            var result = _context.Employees.Add(newEmployee);

            Assert.That(result.Email, Is.EqualTo(newEmployee.Email));
        }

        [Test]
        public void ShouldBeAbleToRemoveAnEmployeeFromDataSet()
        {
            var newEmployee = _randomEmployee.createRandomEmployee();
            var resultAdd = _context.Employees.Add(newEmployee);

            var resultRemove = _context.Employees.Remove(newEmployee);

            Assert.That(resultRemove.Email, Is.EqualTo(newEmployee.Email));
        }

        [Test]
        public void FindShouldReturnResultWithSameEmailAddressAsParameter()
        {
            var enumerator = _context.Employees.GetEnumerator();
            enumerator.MoveNext();
            var employee = enumerator.Current;
            var email = employee.Email;

            var result = _context.Employees.Find(email);

            Assert.That(result.Email, Is.EqualTo(employee.Email));
        }
    }
}
