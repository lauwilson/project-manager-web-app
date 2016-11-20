using System;
using System.Collections.Generic;
using System.Linq;
using Hemlock.Models;
using Hemlock.Models.Enum;

namespace Hemlock.DAL
{
    public class SREDInitializer : System.Data.Entity.DropCreateDatabaseAlways<SREDContext>
    {
        protected override void Seed(SREDContext context)
        {
            var positions = new List<Position>
            {
                new Position {PositionID=Guid.NewGuid(), PositionName="Project Manager" },
                new Position {PositionID=Guid.NewGuid(), PositionName="UX Designer" },
                new Position {PositionID=Guid.NewGuid(), PositionName="Students" }
            };
            positions.ForEach(p => context.Positions.Add(p));
            context.SaveChanges();

            var employees = new List<Employee>
            {
                new Employee {EmployeeID=Guid.NewGuid(),Email="employee1@gmail.com",FirstName="Employee-1",LastName="A",positionID=positions.FirstOrDefault(p => p.PositionName =="Project Manager").PositionID,StartDate=DateTime.Now},
                new Employee {EmployeeID=Guid.NewGuid(),Email="employee2@gmail.com",FirstName="Employee-2",LastName="B",positionID=positions.FirstOrDefault(p => p.PositionName =="Project Manager").PositionID,StartDate=DateTime.Now},
                new Employee {EmployeeID=Guid.NewGuid(),Email="employee3@gmail.com",FirstName="Employee-3",LastName="C",positionID=positions.FirstOrDefault(p => p.PositionName =="UX Designer").PositionID,StartDate=DateTime.Now,Permissions=(int)PermissionsEnum.Admin },
            };

            employees.ForEach(e => context.Employees.Add(e));
            context.SaveChanges();

            var projects = new List<Project>
            {
                new Project {ProjectID=Guid.NewGuid(),ProjectManagerID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                ProjectName="Project Alpha", ProjectCreatorID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                CreatedDate=DateTime.Now,LastModifiedDate=DateTime.Now},
                new Project {ProjectID=Guid.NewGuid(),ProjectManagerID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                ProjectName="Project Beta", ProjectCreatorID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                CreatedDate=DateTime.Now,LastModifiedDate=DateTime.Now},
                new Project {ProjectID=Guid.NewGuid(),ProjectManagerID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                ProjectName="Project Gamma", ProjectCreatorID=employees.FirstOrDefault(e => e.FirstName == "Employee-3").EmployeeID,
                                CreatedDate=DateTime.Now,LastModifiedDate=DateTime.Now},
            };

            projects.ForEach(p => context.Projects.Add(p));
            context.SaveChanges();

            var sredCategories = new List<SREDCategory>
            {
                new SREDCategory {SREDCategoryID=Guid.NewGuid(), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Alpha").ProjectID,
                                    CategoryName="UX Design"},
                new SREDCategory {SREDCategoryID=Guid.NewGuid(), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Alpha").ProjectID,
                                    CategoryName="AI Logic Design"},
                new SREDCategory {SREDCategoryID=Guid.NewGuid(), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Beta").ProjectID,
                                    CategoryName="UX Design"},
                new SREDCategory {SREDCategoryID=Guid.NewGuid(), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Gamma").ProjectID,
                                    CategoryName="AI Logic Design"},
            };

            sredCategories.ForEach(c => context.SREDCategories.Add(c));
            context.SaveChanges();

            var projectEntries = new List<ProjectEntry>
            {
                new ProjectEntry {ProjectEntryID=Guid.NewGuid(), CreatedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-2").EmployeeID,
                                    DateCreated=DateTime.Now.AddDays(-10), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Alpha").ProjectID,
                                    ChangeListNo="r1b2c3d4",SREDCategoryID=null,
                                    Hours=63,Description="Pretty water stuff.",ModifiedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-2").EmployeeID },

                new ProjectEntry {ProjectEntryID=Guid.NewGuid(), CreatedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID,
                                    DateCreated=DateTime.Now.AddHours(-1), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Alpha").ProjectID,
                                    ChangeListNo="u1b2c3d4",SREDCategoryID=null,
                                    Hours=8,Description="Burn it all down!",ModifiedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID },

                new ProjectEntry {ProjectEntryID=Guid.NewGuid(), CreatedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID,
                                    DateCreated=DateTime.Now.AddHours(-1), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Beta").ProjectID,
                                    ChangeListNo="kwb2cxd4",SREDCategoryID=null,
                                    Hours=8,Description="Did work today!",ModifiedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-1").EmployeeID },

                new ProjectEntry {ProjectEntryID=Guid.NewGuid(), CreatedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-2").EmployeeID,
                                    DateCreated=DateTime.Now.AddDays(-10), ProjectID=projects.FirstOrDefault(p => p.ProjectName == "Project Gamma").ProjectID,
                                    ChangeListNo="r1lem3d4",SREDCategoryID=null,
                                    Hours=63,Description="AI Logic prototype.",ModifiedBy=employees.FirstOrDefault(e => e.FirstName == "Employee-2").EmployeeID },
            };

            projectEntries.ForEach(pe => context.ProjectEntries.Add(pe));
            context.SaveChanges();
        }
    }
}