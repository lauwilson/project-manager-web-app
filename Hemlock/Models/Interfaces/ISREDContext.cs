using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemlock.Models.Interfaces
{
    public interface ISREDContext
    {
        IDbSet<Employee> Employees { get; set; }
        IDbSet<Permission> Permissions { get; set; }
        IDbSet<Position> Positions { get; set; }
        IDbSet<Project> Projects { get; set; }
        IDbSet<ProjectEntry> ProjectEntries { get; set; }
        IDbSet<SREDCategory> SREDCategories { get; set; }
        IDbSet<TransactionLog> TransactionLogs { get; set; }

        int SaveChanges();
        void Dispose();

        DbEntityEntry Entry(object entity);
    }
}
