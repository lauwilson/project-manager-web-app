using Hemlock.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemlockTests
{
    [TestFixture]
    class ProjectEntryTableTest
    {
        [Test]
        public void ShouldCreateInstance_WhenAllParametersAreNull()
        {
            var result = new ProjectEntryTable(null, null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ShouldCreateInstance_WhenProvidedAllParameters()
        {
            var sortOrder = "project";
            var pageSize = 2;

            var result = new ProjectEntryTable(sortOrder, pageSize);

            Assert.That(result.CurrentSort, Is.EqualTo(sortOrder));
            Assert.That(result.PageSize, Is.EqualTo(pageSize));
        }
    }
}
