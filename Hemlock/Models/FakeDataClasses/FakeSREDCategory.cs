using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.FakeDataClasses
{
    public class FakeSREDCategory : FakeDbSet<SREDCategory>
    {
        public override SREDCategory Find(params object[] keyValues)
        {
            return this.SingleOrDefault(
                category => category.CategoryName == (string)keyValues.Single());
        }
    }
}