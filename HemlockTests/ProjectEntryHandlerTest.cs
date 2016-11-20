using Hemlock.DAL;
using Hemlock.Handlers;
using Hemlock.Models;
using Hemlock.Models.FakeDataClasses;
using Hemlock.Models.Interfaces.Repositories;
using HemlockTests.Randomizer;
using NUnit.Framework;
using System;
using System.Linq;

namespace HemlockTests
{
    [TestFixture]
    class ProjectEntryHandlerTest
    {
        private FakeSREDContext _context;
        private RandomProjectEntry _random = new RandomProjectEntry();
        private IProjectEntryRepository _projectEntryRepository;
        private Randomizer.Randomizer _randomizer = new Randomizer.Randomizer();
        private readonly int _minEntryIndex = 0;
        private readonly int _maxEntryIndex = 5;

        [SetUp]
        public void Initialize()
        {
            _context = new FakeSREDContext();
            _projectEntryRepository = new ProjectEntryRepository(_context);
            _context.ProjectEntries = new FakeProjectEntry {
                _random.CreateRandomProjectEntry("I made an entry!"),
                _random.CreateRandomProjectEntry("Water physics."),
                _random.CreateRandomProjectEntry("Fire."),
                _random.CreateRandomProjectEntry("Invented a new AI."),
                _random.CreateRandomProjectEntry("Added charisma to NPC.")
            };
        }

        [Test]
        public void HandleGetProjectEntry_ShouldReturnProjectEntryTableObject_WhenAllParametersButEntriesAreNull()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var result = sut.HandleGetProjectEntry(entries, 
                null, 
                null, 
                null, 
                null, 
                null,
                DateTime.MinValue,
                DateTime.MaxValue,
                Guid.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void HandleProjectEntry_ShouldReturnProjectEntryTableObject_WhenNoParametersAreNull()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var sortOrder = "project";
            var currentFilter = "water";
            var randomEntryNumber = _randomizer.RandomNumber(_minEntryIndex, _maxEntryIndex);
            var searchString = entries[randomEntryNumber].Description;
            var page = 1;
            var pageSize = 25;

            var result = sut.HandleGetProjectEntry(entries,
                sortOrder,
                currentFilter,
                searchString,
                page,
                pageSize,
                DateTime.MinValue,
                DateTime.MaxValue,
                Guid.Empty);

            CollectionAssert.IsNotEmpty(result.Entries);
        }

        [Test]
        public void HandleGetProjectEntry_ShouldDefaultPageNumberToOne_WhenGivenInvalidPageNumber()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var sortOrder = "project";
            var currentFilter = "water";
            var randomEntryNumber = _randomizer.RandomNumber(_minEntryIndex, _maxEntryIndex);
            var searchString = entries[randomEntryNumber].Description;
            var page = 100;
            var pageSize = 25;

            var result = sut.HandleGetProjectEntry(entries,
                sortOrder,
                currentFilter,
                searchString,
                page,
                pageSize,
                DateTime.MinValue,
                DateTime.MaxValue,
                Guid.Empty);

            CollectionAssert.IsNotEmpty(result.Entries);
        }


        [Test]
        public void SortProjectEntries_ShouldSortByDateCreated_WhenNotProvidedSortString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.DateCreated);
            var result = sut.SortEntries(null, entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByChangeListNo_WhenProvidedChangeListNoString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderBy(entry => entry.ChangeListNo);
            var result = sut.SortEntries("changeListNo", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByChangeListNo_WhenProvidedDescendingChangeListNoString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.ChangeListNo);
            var result = sut.SortEntries("changeListNo_desc", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByProjectName_WhenPassedProjectString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderBy(entry => entry.Project.ProjectName);
            var result = sut.SortEntries("project", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDescendingProjectName_WhenPassedDescendingProjectString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.Project.ProjectName);
            var result = sut.SortEntries("project_desc", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDateCreated_WhenPassedDateString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderBy(entry => entry.DateCreated);
            var result = sut.SortEntries("date", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDescendingDateCreated_WhenPassedDescendingDateString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.DateCreated);
            var result = sut.SortEntries("date_desc", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByCategoryName_WhenPassedCategoryString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderBy(entry => entry.SREDCategory.CategoryName);
            var result = sut.SortEntries("category", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDescendingCategoryName_WhenPassedDescendingCategoryString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.SREDCategory.CategoryName);
            var result = sut.SortEntries("category_desc", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByHours_WhenPassedHoursString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderBy(entry => entry.Hours);
            var result = sut.SortEntries("hours", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDescendingHours_WhenPassedDescendingHoursString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.Hours);
            var result = sut.SortEntries("hours_desc", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SortProjectEntries_ShouldSortByDefaultSort_WhenWrongStringIsPassed()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();

            var expected = entries.OrderByDescending(entry => entry.DateCreated);
            var result = sut.SortEntries("brabble", entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SearchProjectEntries_ShouldFindEntry_ThatContainsProvidedString()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var randomEntryNumber = _randomizer.RandomNumber(_minEntryIndex, _maxEntryIndex);
            var searchString = entries[randomEntryNumber].Description;

            var expected = entries.Where(entry => entry.Description == searchString);
            var result = sut.SearchProjectEntries(searchString, entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SearchProjectEntries_ShouldReturnAllEntries_WhenSearchStringIsNullOrEmpty()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var searchString = string.Empty;

            var result = sut.SearchProjectEntries(searchString, entries);

            Assert.That(result, Is.EqualTo(entries));
        }

        [Test]
        public void SearchProjectEntries_ShouldReturnNoResults_WhenSearchStringIsNotFoundInAnyEntry()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var randomEntryNumber = _randomizer.RandomNumber(_minEntryIndex, _maxEntryIndex);
            var searchString = "onomatopeia";

            var expected = entries.Where(entry => entry.Description == searchString);
            var result = sut.SearchProjectEntries(searchString, entries);

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void SearchProjectEntries_ShouldReturnResults_WhenProvidedFirstEightCharactersInChangeListNo()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var searchString = entries.First().ChangeListNo.Substring(0,8);

            var expected = entries.Where(entry =>
                entry.ChangeListNo.ToLower().Substring(0, 8).Contains(searchString.ToLower())).ToList();
            var result = sut.SearchProjectEntries(searchString, entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SearchProjectEntries_ShouldreturnResults_WhenProvidedMiddleCharactersInChangeListNo()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var entries = _context.ProjectEntries.ToList();
            var searchString = entries.First().ChangeListNo.Substring(2,4);

            var expected = entries.Where(entry => 
                entry.ChangeListNo.ToLower().Substring(0, 8).Contains(searchString.ToLower())).ToList();
            var result = sut.SearchProjectEntries(searchString, entries);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SearchProjectEntries_ShouldReturnNoEntries_WhenDescriptionIsNullAndSearchStringNotFound()
        {
            var sut = new ProjectEntryHandler(_context, _projectEntryRepository);
            var searchString = "test";

            var entries = _context.ProjectEntries.ToList();
            entries.First().Description = null;

            var result = sut.SearchProjectEntries(searchString, entries);

            CollectionAssert.IsEmpty(result);
        }
    }
}
