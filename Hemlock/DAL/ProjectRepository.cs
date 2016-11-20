using System;
using System.Collections.Generic;
using System.Linq;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;
using Hemlock.Models;
using System.Data.Entity;

namespace Hemlock.DAL
{
    public class ProjectRepository : IProjectRepository, IDisposable
    {
        private ISREDContext _context;
        private bool _disposed = false;

        public ProjectRepository(ISREDContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects.ToList();
        }

        public Project GetProjectByID(Guid projectID)
        {
            return _context.Projects.Find(projectID);
        }

        public Project GetProjectByName(string projectName)
        {
            return _context.Projects.FirstOrDefault(p => p.ProjectName == projectName);
        }

        public void InsertProject(Project project)
        {
            try
            {
                _context.Projects.Add(project);
                Save();
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        public void DeleteProject(Guid projectID)
        {
            Project project = _context.Projects.Find(projectID);
            _context.Projects.Remove(project);
        }

        public void UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }
}