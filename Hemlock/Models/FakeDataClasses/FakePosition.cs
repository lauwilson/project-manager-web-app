using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.FakeDataClasses
{
	public class FakePosition : FakeDbSet<Position>
    {
        public override Position Find(params object[] keyValues)
        {
            //will return exception, convert to list
            return this.SingleOrDefault(
                position => position.PositionName == (string)keyValues.Single());
        }
    }
}