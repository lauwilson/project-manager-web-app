using System;
using System.Linq;
using NUnit.Framework;
using Hemlock.DAL;
using Hemlock.Models.Interfaces.Repositories;
using Hemlock.Models.Exceptions;
using Hemlock.Handlers;
using System.Security.Claims;
using Hemlock.Models;
using HemlockTests.Randomizer;
using System.Web;

namespace HemlockTests
{
    [TestFixture]
    class EmployeeHandlerTest
    {
        private FakeSREDContext _context;
        private IEmployeeRepository _employeeRepository;
        private RandomEmployee _random = new RandomEmployee();

        [SetUp]
        public void Initialize()
        {
            _context = new FakeSREDContext();
            _context.Employees = new FakeEmployee
            {
                _random.createRandomEmployee(),
                _random.createRandomEmployee(),
                _random.createRandomEmployee(),
                _random.createRandomEmployee(),
                _random.createRandomEmployee()
            };
            _employeeRepository = new EmployeeRepository(_context);
            HttpContext.Current = FakeHttpContext.CreateFakeContext();
        }

        [Test]
        public void ShouldBeAbleToCreateInstance()
        {
            var sut = new EmployeeHandler(_context, _employeeRepository);

            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void HandleGetEmployee_ShouldReturnEmployee_BasedOnEmailAddress()
        {
            var randomNumber = new Random();
            var identity = new ClaimsIdentity();
            var sut = new EmployeeHandler(_context, _employeeRepository);

            var randomEmployee = _context.Employees.
                ElementAt(randomNumber.Next(_context.Employees.Count()));

            var expected = randomEmployee.Email;

            identity.AddClaim(new Claim(ClaimTypes.Email, expected));

            var result = sut.HandleGetEmployee(identity);

            Assert.That(result.Email, Is.EqualTo(expected));
        }

        [Test]
        public void HandleGetEmployee_ShouldReturnNull_WhenEmailDomainIsIncorrect()
        {
            var identity = new ClaimsIdentity();
            var sut = new EmployeeHandler(_context, _employeeRepository);
            var newEmployee = new Employee
            {
                Email = "jasonK58@gmail.com"
            };

            identity.AddClaim(new Claim(ClaimTypes.Email, newEmployee.Email));

            Assert.IsNull(sut.HandleGetEmployee(identity));
        }
    }
}
