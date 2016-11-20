using Hemlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemlockTests.Randomizer
{
    class RandomProjectEntry
    {
        private Randomizer _random = new Randomizer();
        private readonly int _minHours = 1;
        private readonly int _maxHours = 24;
        private readonly int _stringLength = 10;

        public ProjectEntry CreateRandomProjectEntry(string description)
        {
            var projectEntry = new ProjectEntry();

            projectEntry.ProjectEntryID = Guid.NewGuid();
            projectEntry.CreatedBy = Guid.NewGuid();
            projectEntry.DateCreated = _random.RandomDate();
            projectEntry.ProjectID = Guid.NewGuid();
            projectEntry.Project = new Project();
            projectEntry.Project.ProjectName = _random.RandomString(_stringLength);
            projectEntry.ChangeListNo = Guid.NewGuid().ToString();
            projectEntry.SREDCategoryID = Guid.NewGuid();
            projectEntry.SREDCategory = new SREDCategory();
            projectEntry.SREDCategory.CategoryName = _random.RandomString(_stringLength);
            projectEntry.Hours = _random.RandomNumber(_minHours, _maxHours);
            projectEntry.Description = description;
            projectEntry.ModifiedBy = Guid.NewGuid();
            projectEntry.ModifiedDate = _random.RandomDate();

            return projectEntry;
        }
    }
}
