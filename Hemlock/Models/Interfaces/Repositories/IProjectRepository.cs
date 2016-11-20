using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.Interfaces.Repositories
{
    public interface IProjectRepository : IDisposable
    {
        IEnumerable<Project> GetProjects();
        Project GetProjectByID(Guid projectID);
        Project GetProjectByName(string projectName);
        void InsertProject(Project project);
        void DeleteProject(Guid projectID);
        void UpdateProject(Project project);
        void Save();
    }
}