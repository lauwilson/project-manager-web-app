using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hemlock.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Hemlock.Models.Interfaces;

namespace Hemlock.DAL
{
    public class SREDContext : DbContext, ISREDContext
    {
        public SREDContext() : base("SREDContext")
        {
        }

        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Permission> Permissions { get; set; }
        public IDbSet<Position> Positions { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<ProjectEntry> ProjectEntries { get; set; }
        public IDbSet<SREDCategory> SREDCategories { get; set; }
        public IDbSet<TransactionLog> TransactionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}