using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hemlock.Models;
using System.Data.Entity;

namespace Hemlock.DAL
{
    public class ProjectEntryRepository: IProjectEntryRepository, IDisposable
    {
        private ISREDContext _context;
        private bool _disposed = false;

        public ProjectEntryRepository(ISREDContext context)
        {
            _context = context;
        }

        public bool PostProjectEntry(ProjectEntry projectEntry)
        {
            try
            {
                _context.ProjectEntries.Add(projectEntry);
                Save();
            } catch (Exception)
            {
                Dispose(true);
                return false;
            }

            return true;
        }

        public bool PatchProjectEntry(ProjectEntry existingEntry)
        {
            try
            {
                _context.ProjectEntries.Attach(existingEntry);
                _context.Entry(existingEntry).State = EntityState.Modified;
                Save();
            } catch (Exception)
            {
                Dispose(true);
                return false;
            }

            return true;  
        }

        public bool DeleteProjectEntry(ProjectEntry entryToDelete)
        {
            try
            {
                _context.ProjectEntries.Remove(entryToDelete);
                Save();
            } catch (Exception)
            {
                Dispose(true);
                return false;
            }

            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
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