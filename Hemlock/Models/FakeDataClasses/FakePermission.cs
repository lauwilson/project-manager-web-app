using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.FakeDataClasses
{
    public class FakePermission : FakeDbSet<Permission>
    {
        public override Permission Find(params object[] keyValues)
        {
            return this.SingleOrDefault(
                permission => permission.Bit == (int)keyValues.Single());
        }
    }
}