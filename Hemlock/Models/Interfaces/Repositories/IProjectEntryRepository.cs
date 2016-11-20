using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemlock.Models.Interfaces.Repositories
{
    public interface IProjectEntryRepository
    {
        bool PostProjectEntry(ProjectEntry projectEntry);

        bool PatchProjectEntry(ProjectEntry existingEntry);

        bool DeleteProjectEntry(ProjectEntry entryToDelete);

        void Dispose();
    }
}
