using System;
using System.Collections.Generic;
using System.Linq;

namespace Hemlock.Models
{
    public class FakeEmployee : FakeDbSet<Employee>
    {

        public override Employee Find(params object[] keyValues)
        {
            return this.SingleOrDefault(
                employee => employee.Email == (string)keyValues.Single());
        }
       
    }
}