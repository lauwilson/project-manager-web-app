namespace Hemlock.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Hemlock.Models;
    using Hemlock.Models.Enum;

    internal sealed class Configuration : DbMigrationsConfiguration<Hemlock.DAL.SREDContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Hemlock.DAL.SREDContext context)
        {
            var positions = new List<Position>
            {
                new Position {PositionID=Guid.NewGuid(), PositionName="Project Manager"},
                new Position {PositionID=Guid.NewGuid(), PositionName="UX Designer"},
                new Position {PositionID=Guid.NewGuid(), PositionName="Web Developer"}
            };
            positions.ForEach(position => context.Positions.AddOrUpdate(oldPosition => oldPosition.PositionName, position));
            context.SaveChanges();

            var employees = new List<Employee>
            {
                // TODO Production: Non-Admins should not be seeded possibly
                // TODO Production: Proper Permissions should be seeded, not all Admin
                // NOTE: Emails are all redacted for the demo code
                new Employee {EmployeeID=Guid.NewGuid(),Email="employee1@example.com",FirstName="Employee-1",LastName="A",positionID=positions.FirstOrDefault(p => p.PositionName == "Project Manager").PositionID,StartDate=DateTime.Now,Permissions=(int)PermissionsEnum.Admin},
            };

            employees.ForEach(employee => context.Employees.AddOrUpdate(oldEmployee => oldEmployee.Email, employee));
            context.SaveChanges();

            var projects = new List<Project>
            {
                // TODO Production: Only Default Project seeded should be the "Absent From Work" project.
                new Project {ProjectID=Guid.NewGuid(),ProjectManagerID=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID,
                                ProjectName="Absent From Work", ProjectCreatorID=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID,
                                CreatedDate=DateTime.Now,LastModifiedDate=DateTime.Now}
            };

            projects.ForEach(project => context.Projects.AddOrUpdate(oldProject => oldProject.ProjectName, project));
            context.SaveChanges();
        }
    }
}
