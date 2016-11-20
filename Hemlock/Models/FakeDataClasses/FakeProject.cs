using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.FakeDataClasses
{
	public class FakeProject : FakeDbSet<Project>
    {
        public override Project Find(params object[] keyValues)
        {
            return this.SingleOrDefault(
                project => project.ProjectName == (string)keyValues.Single());
        }
    }
}