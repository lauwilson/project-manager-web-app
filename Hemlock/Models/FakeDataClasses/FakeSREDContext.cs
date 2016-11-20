using Hemlock.Models.Interfaces;
using System.Data.Entity;
using System;
using Hemlock.Models.FakeDataClasses;
using System.Data.Entity.Infrastructure;

namespace Hemlock.Models
{
    public class FakeSREDContext : ISREDContext
    {
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Permission> Permissions { get; set; }
        public IDbSet<Position> Positions { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<ProjectEntry> ProjectEntries { get; set; }
        public IDbSet<SREDCategory> SREDCategories { get; set; }
        public IDbSet<TransactionLog> TransactionLogs { get; set; }

        public FakeSREDContext()
        {
            Employees = new FakeEmployee();
            Permissions = new FakePermission();
            Positions = new FakePosition();
            Projects = new FakeProject();
            ProjectEntries = new FakeProjectEntry();
            SREDCategories = new FakeSREDCategory();
            TransactionLogs = new FakeTransactionLog();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public DbEntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }
    }
}