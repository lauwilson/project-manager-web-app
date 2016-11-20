using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.Interfaces.Repositories
{
    public interface ISREDCategoryRepository : IDisposable
    {
        IEnumerable<SREDCategory> GetSREDCategories();
        IEnumerable<SREDCategory> GetSREDCategoriesByProject(Project project);
        SREDCategory GetSREDCategoryByID(Guid categoryID);
        void InsertSREDCategory(SREDCategory category);
        void DeleteSREDCategory(Guid categoryID);
        void UpdateSREDCategory(SREDCategory category);
        void Save();

    }
}