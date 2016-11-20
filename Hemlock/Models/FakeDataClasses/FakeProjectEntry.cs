using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.FakeDataClasses
{
    public class FakeProjectEntry : FakeDbSet<ProjectEntry>
    {
        public override ProjectEntry Find(params object[] keyValues)
        {
            //will return exception, convert to list
            return this.SingleOrDefault(
                projectEntry => projectEntry.CreatedBy == (Guid)keyValues.Single());
        }
    }
}