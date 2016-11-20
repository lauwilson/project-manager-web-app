using System;
using System.Collections.Generic;
using System.Linq;
using Hemlock.Models.Interfaces;
using Hemlock.Models.Interfaces.Repositories;
using Hemlock.Models;
using System.Data.Entity;

namespace Hemlock.DAL
{
    public class SREDCategoryRepository : ISREDCategoryRepository, IDisposable
    {
        private ISREDContext _context;
        private bool _disposed = false;

        public SREDCategoryRepository(ISREDContext context)
        {
            _context = context;
        }

        public IEnumerable<SREDCategory> GetSREDCategories()
        {
            return _context.SREDCategories.ToList();
        }

        public IEnumerable<SREDCategory> GetSREDCategoriesByProject(Project project)
        {
            return _context.SREDCategories.Where(c => c.ProjectID == project.ProjectID);
        }

        public SREDCategory GetSREDCategoryByID(Guid categoryID)
        {
            return _context.SREDCategories.SingleOrDefault(c => c.SREDCategoryID == categoryID);
        }

        public void InsertSREDCategory(SREDCategory category)
        {
            try
            {
                _context.SREDCategories.Add(category);
                Save();
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        public void DeleteSREDCategory(Guid categoryID)
        {
            throw new NotImplementedException();
        }

        public void UpdateSREDCategory(SREDCategory category)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}